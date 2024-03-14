using MSIT155_E_MID;
using MSIT155_E_MID.ApptSystem.Extension;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PCSC;
using PCSC.Monitoring;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CardReader.Form1;
using res = CardReader.Properties.Resources;

namespace CardReader
{
    public delegate void D_info(EntitySmartCardInfoType info);
    public delegate void D_infoclear();

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            FrmCallingUnit_Load2(sender, e);
        }

        //-----------------------------------------------------------------------
        //讀卡機
        private ISCardContext oReaders = PCSC.ContextFactory.Instance.Establish(PCSC.SCardScope.User);
        private ISCardMonitor oMonitor;
        private void FrmCallingUnit_Load2(object sender, EventArgs e)
        {
            EntitySmartCardInfoType info;
            string cReader = oReaders.GetReaders().FirstOrDefault();
            if (string.IsNullOrEmpty(cReader))
            {
                MessageBox.Show("找不到任何讀卡機，請重新檢查硬體");
                return;     //找不到任何PSCS讀卡機就跳走
            }
            else
            {
                //MessageBox.Show($"已偵測到讀卡機{cReader}，程式運行期間請勿任意移除設備。");
                oMonitor = PCSC.Monitoring.MonitorFactory.Instance.Create(PCSC.SCardScope.System);
            }

            oMonitor.CardRemoved += (oSender, oArgs) =>
            {
                //MessageBox.Show("# 偵測到晶片卡移除。");
                if (this.InvokeRequired)
                {
                    D_infoclear del = new D_infoclear(SmartCardRemove);
                    this.Invoke(del);
                }
            };
            oMonitor.CardInserted += (oSender, oArgs) =>
            {
                //MessageBox.Show("# 偵測到晶片卡插入。");
                info = GetCardInfo(cReader);  //讀取健保卡顯性資料
                if (this.InvokeRequired)
                {
                    D_info del = new D_info(SmartCardRead);
                    this.Invoke(del, info);
                }
            };
            oMonitor.MonitorException += (oSender, oArgs) =>
            {
                MessageBox.Show("讀卡機被移除或是讀取晶片卡出現異常，請重新啟動程式。");
                return;
            };
            oMonitor.Start(cReader);
            //有可能執行程式前讀卡機與卡片就都已經準備好，如此一來並不會觸發事件，因此先強制執行一次讀取看看
            try
            {
                info = GetCardInfo(cReader);
                if (this.InvokeRequired)
                {
                    D_info del = new D_info(SmartCardRead);
                    this.Invoke(del, info);
                }
            }
            catch
            { return; }
        }

        //插卡事件執行方法
        private async void SmartCardRead(EntitySmartCardInfoType info)
        {
            patientInfo1.info = info;
            CardReaderHttpClient client = new CardReaderHttpClient();

            string response = await client.GET_ApptData(info.cID);
            ApptDataModel data = response == null ? new ApptDataModel() { } : JsonConvert.DeserializeObject<ApptDataModel>(response);

            checkInPlaySound(data.number);
        }
        //拔卡事件執行方法
        public void SmartCardRemove()
        {
            new PatientInfo().Clear(patientInfo1);
        }

        //音效
        private void checkInPlaySound(int clinicNum)
        {
            //0 為無操作，直接停止撥放
            if (clinicNum == 0) { return; }
            //-1 為NotFuund，播放報到失敗
            if (clinicNum == -1) { new SoundPlayer(res.報到失敗).Play(); return; }
            //-2 為重複報到 尚未更換音效
            if (clinicNum == -2) { new SoundPlayer(res.報到成功).Play(); return; }

            string xx = clinicNum.ToString().Substring(0, 1);
            string x = clinicNum.ToString().Substring(1, 1);
            switch (xx)
            {
                case "9": new SoundPlayer(res.九).PlaySync(); new SoundPlayer(res.十).PlaySync(); break;
                case "8": new SoundPlayer(res.八).PlaySync(); new SoundPlayer(res.十).PlaySync(); break;
                case "7": new SoundPlayer(res.七).PlaySync(); new SoundPlayer(res.十).PlaySync(); break;
                case "6": new SoundPlayer(res.六).PlaySync(); new SoundPlayer(res.十).PlaySync(); break;
                case "5": new SoundPlayer(res.五).PlaySync(); new SoundPlayer(res.十).PlaySync(); break;
                case "4": new SoundPlayer(res.四).PlaySync(); new SoundPlayer(res.十).PlaySync(); break;
                case "3": new SoundPlayer(res.三).PlaySync(); new SoundPlayer(res.十).PlaySync(); break;
                case "2": new SoundPlayer(res.二).PlaySync(); new SoundPlayer(res.十).PlaySync(); break;
                case "1": new SoundPlayer(res.十).PlaySync(); break;
                case "0": break;
            }
            switch (x)
            {
                case "9": new SoundPlayer(res.九).PlaySync(); break;
                case "8": new SoundPlayer(res.八).PlaySync(); break;
                case "7": new SoundPlayer(res.七).PlaySync(); break;
                case "6": new SoundPlayer(res.六).PlaySync(); break;
                case "5": new SoundPlayer(res.五).PlaySync(); break;
                case "4": new SoundPlayer(res.四).PlaySync(); break;
                case "3": new SoundPlayer(res.三).PlaySync(); break;
                case "2": new SoundPlayer(res.二).PlaySync(); break;
                case "1": new SoundPlayer(res.一).PlaySync(); break;
                case "0": break;
            }
            new SoundPlayer(res.號).PlaySync();
            new SoundPlayer(res.報到成功).Play();
        }

        //讀卡核心
        private EntitySmartCardInfoType GetCardInfo(string cReader)
        {
            using (var oContext = PCSC.ContextFactory.Instance.Establish(PCSC.SCardScope.User))
            using (var oReader = new PCSC.Iso7816.IsoReader(
              context: oContext,
              readerName: cReader,
              mode: PCSC.SCardShareMode.Shared,
              protocol: PCSC.SCardProtocol.Any
            ))
            {
                //Console.WriteLine("－－－－－");
                //初始化健保卡
                var oAdpuInit = new PCSC.Iso7816.CommandApdu(PCSC.Iso7816.IsoCase.Case4Short, oReader.ActiveProtocol)
                {
                    CLA = 0x00,
                    INS = 0xA4,
                    P1 = 0x04,
                    P2 = 0x00,
                    Data = new byte[] { 0xD1, 0x58, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x11 }
                };
                //Console.WriteLine($"@ APDU InitCard:   {System.BitConverter.ToString(oAdpuInit.ToArray())}");
                //取得初始化健保卡回應
                var oAdpuInitResponse = oReader.Transmit(oAdpuInit);
                //Console.WriteLine($"@ Response:        SW1={oAdpuInitResponse.SW1.ToString("X")}｜SW2={oAdpuInitResponse.SW2.ToString("X")}");
                //檢查回應是否正確（144；00）
                if (!(oAdpuInitResponse.SW1.Equals(144) && oAdpuInitResponse.SW2.Equals(0)))
                {
                    //Console.WriteLine("－－－－－");
                    MessageBox.Show("# 晶片卡並非健保卡，請換張卡片試看看。");
                    return null;
                }
                //讀取健保顯性資訊
                var oAdpuProfile = new PCSC.Iso7816.CommandApdu(PCSC.Iso7816.IsoCase.Case4Short, oReader.ActiveProtocol)
                {
                    CLA = 0x00,
                    INS = 0xCA,
                    P1 = 0x11,
                    P2 = 0x00,
                    Data = new byte[] { 0x00, 0x00 }
                };
                //Console.WriteLine($"@ APDU GetProfile: {System.BitConverter.ToString(oAdpuProfile.ToArray())}");
                //取得讀取健保卡顯性資訊回應
                var oAdpuProfileResponse = oReader.Transmit(oAdpuProfile);
                //Console.WriteLine($"@ Response:        SW1={oAdpuProfileResponse.SW1.ToString("X")}｜SW2={oAdpuProfileResponse.SW2.ToString("X")}");
                //檢查回應是否正確（144；00）
                if (!(oAdpuInitResponse.SW1.Equals(144) && oAdpuInitResponse.SW2.Equals(0)))
                {
                    //Console.WriteLine("－－－－－");
                    System.Windows.Forms.MessageBox.Show("# 健保卡讀取錯誤，請換張卡片試看看。");
                    return null;
                }
                //如果有回應且具備資料的話，就將其輸出到畫面上
                if (oAdpuProfileResponse.HasData)
                { //播放提示音
                    //Console.Beep();
                    //位元組資料
                    byte[] aryData = oAdpuProfileResponse.GetData();
                    //文字編碼解碼器
                    var oUTF8 = System.Text.Encoding.UTF8;
                    var oBIG5 = System.Text.Encoding.GetEncoding("big5");
                    //建立使用者匿名物件
                    var oUser = new
                    {
                        cCardNumber = oUTF8.GetString(aryData.Take(12).ToArray()),
                        cName = oBIG5.GetString(aryData.Skip(12).Take(20).ToArray()),
                        cID = oUTF8.GetString(aryData.Skip(32).Take(10).ToArray()),
                        cBirthday = oUTF8.GetString(aryData.Skip(42).Take(7).ToArray()),
                        cGender = oUTF8.GetString(aryData.Skip(49).Take(1).ToArray()) == "M" ? "男" : "女",
                        cCardPublish = oUTF8.GetString(aryData.Skip(50).Take(7).ToArray())
                    };
                    return new EntitySmartCardInfoType()
                    {
                        cCardNumber = oUser.cCardNumber,
                        cName = (oUser.cName).Trim(),
                        cID = oUser.cID,
                        cBirthday = Extension_DateTime.TWdateFormate(oUser.cBirthday),
                        cGender = oUser.cGender,
                        cCardPublish = oUser.cCardPublish
                    };
                }
                return null;
            }
        }
        //-----------------------------------------------------------

        public class ApptDataModel 
        {
            public string department { get; set; }
            public string shift { get; set; }
            public string doctor { get; set; }
            public int number { get; set; }
            public int state { get; set; }

            public ApptDataModel() 
            {
                department = "";
                shift = "";
                doctor = "";
                number = -1; 
                state = -1;
            }
        }
    }
}
