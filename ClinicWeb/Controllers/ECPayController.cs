using Microsoft.AspNetCore.Mvc;
using ECPay.Payment.Integration;
using NuGet.Packaging;
using System.Collections;
using System.Text;
using System.Web;
using System.Security.Cryptography;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using ClinicWeb.Areas.Appointment.Models;
using System.Globalization;

namespace ClinicWeb.Controllers
{
    public class ECPayController : Controller
    {
        private readonly ClinicSysContext _context;
        public ECPayController(ClinicSysContext context)
        {
            this._context = context;
        }

        public IActionResult Index() { return View(); }

        //產生訂單
        //step1 : 網頁導入傳值到前端
        public IActionResult CheckOut()
        {
            var orderId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
            var website = $"https://localhost:7071"; //記得確認一下數字有沒有一樣
            var order = new Dictionary<string, string>
                {
                    { "MerchantTradeNo",  orderId},
                    { "MerchantTradeDate",  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},
                    { "TotalAmount",  "150"},
                    { "TradeDesc",  "無"},
                    { "ItemName",  "測試商品"},
                    { "ExpireDate",  "3"},
                    { "CustomField1",  ""},
                    { "CustomField2",  ""},
                    { "CustomField3",  ""},
                    { "CustomField4",  ""},
                    { "ReturnURL",  $"{website}/ECPay/AddPayInfo"}, //付款完成通知回傳的網址
                    { "OrderResultURL", $"{website}/ECPay/PayInfo/{orderId}"},//瀏覽器端回傳付款結果網址
                    { "PaymentInfoURL",  $"{website}/ECPay/AddAccountInfo"},//伺服器端回傳付款相關資訊
                    { "ClientRedirectURL",  $"{website}/ECPay/AccountInfo/{orderId}"},//Client 端回傳付款相關資訊
                    { "MerchantID",  "2000132"},
                    { "IgnorePayment",  "GooglePay#WebATM#CVS#BARCODE"},
                    { "PaymentType",  "aio"},
                    { "ChoosePayment",  "ALL"},
                    { "EncryptType",  "1"},
                };
            order["CheckMacValue"] = GetCheckMacValue(order);
            return View(order);
        }
        private string GetCheckMacValue(Dictionary<string, string> order)
        {
            var param = order.Keys.OrderBy(x => x).Select(key => key + "=" + order[key]).ToList();
            var checkValue = string.Join("&", param);
            //測試用的 HashKey
            var hashKey = "5294y06JbISpM5x9";
            //測試用的 HashIV
            var HashIV = "v77hoKGq4kWxNNIS";
            checkValue = $"HashKey={hashKey}" + "&" + checkValue + $"&HashIV={HashIV}";
            checkValue = HttpUtility.UrlEncode(checkValue).ToLower();
            checkValue = GetSHA256(checkValue);
            return checkValue.ToUpper();
        }
        private string GetSHA256(string value)
        {
            var result = new StringBuilder();
            var sha256 = SHA256.Create();
            var bts = Encoding.UTF8.GetBytes(value);
            var hash = sha256.ComputeHash(bts);
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }

        ///step4 : 新增訂單
        [HttpPost]
        public string AddOrders([FromBody]OrdersJsonViewModel json)
        {
            string num = "0";
            try
            {
                //ApptEcpay Orders = new ApptEcpay();
                var Orders = _context.ApptEcpay.Where(x => x.FClinicListId == Convert.ToInt32(json.apptlistid)).FirstOrDefault();
                Orders.MemberId = json.MerchantID;
                Orders.MerchantTradeNo = json.MerchantTradeNo;
                Orders.RtnCode = 0; //未付款
                Orders.RtnMsg = "訂單成功尚未付款";
                Orders.TradeNo = json.MerchantID;
                Orders.TradeAmt = Convert.ToInt32(json.TotalAmount);
                //Orders.PaymentDate = Convert.ToDateTime(json.MerchantTradeDate);
                Orders.PaymentDate = DateTime.ParseExact(json.MerchantTradeDate, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
                Orders.PaymentType = json.PaymentType;
                Orders.PaymentTypeChargeFee = "0";
                Orders.TradeDate = json.MerchantTradeDate;
                Orders.SimulatePaid = 0;
                _context.SaveChanges();
                num = "OK";
            }
            catch (Exception ex)
            {
                num = ex.ToString();
            }
            return num;
        }
        /// step5 : 取得付款資訊，更新資料庫
        [HttpPost]
        public ActionResult PayInfo(IFormCollection id)
        {
            var data = new Dictionary<string, string>();
            foreach (string key in id.Keys)
            {
                data.Add(key, id[key]);
            }
            //Database1Entities db = new Database1Entities();
            string temp = id["MerchantTradeNo"]; //寫在LINQ(下一行)會出錯，
            var ecpayOrder = _context.ApptEcpay.Where(m => m.MerchantTradeNo == temp).FirstOrDefault();
            if (ecpayOrder != null)
            {
                ecpayOrder.RtnCode = int.Parse(id["RtnCode"]);
                if (id["RtnMsg"] == "Succeeded") ecpayOrder.RtnMsg = "已付款";
                ecpayOrder.PaymentDate = Convert.ToDateTime(id["PaymentDate"]);
                ecpayOrder.SimulatePaid = int.Parse(id["SimulatePaid"]);
                _context.SaveChanges();
            }
            //return View("EcpayView", data);
            return RedirectToAction("Index", "MBAppointmentInfo", new { id = "CatReceivedMoney" });
        }
        /// step5 : 取得虛擬帳號 資訊
        [HttpPost]
        public ActionResult AccountInfo(IFormCollection id)
        {
            var data = new Dictionary<string, string>();
            foreach (string key in id.Keys)
            {
                data.Add(key, id[key]);
            }
            //Database1Entities db = new Database1Entities();
            string temp = id["MerchantTradeNo"]; //寫在LINQ會出錯
            var ecpayOrder = _context.ApptEcpay.Where(m => m.MerchantTradeNo == temp).FirstOrDefault();
            if (ecpayOrder != null)
            {
                ecpayOrder.RtnCode = int.Parse(id["RtnCode"]);
                if (id["RtnMsg"] == "Succeeded") ecpayOrder.RtnMsg = "已付款";
                ecpayOrder.PaymentDate = Convert.ToDateTime(id["PaymentDate"]);
                ecpayOrder.SimulatePaid = int.Parse(id["SimulatePaid"]);
                _context.SaveChanges();
            }
            return View("EcpayView", data);
        }

        ///模擬付款 應該用不到
        //[System.Web.Http.HttpPost]
        //[System.Web.Http.Route("api/Ecpay/AddPayInfo")]
        //public HttpResponseMessage AddPayInfo(JObject info)
        //{
        //    try
        //    {
        //        var cache = MemoryCache.Default;
        //        cache.Set(info.Value<string>("MerchantTradeNo"), info, DateTime.Now.AddMinutes(60));
        //        return ResponseOK();
        //    }
        //    catch (Exception e)
        //    {
        //        return ResponseError();
        //    }
        //}
        //[System.Web.Http.HttpPost]
        //[System.Web.Http.Route("api/Ecpay/AddAccountInfo")]
        //public HttpResponseMessage AddAccountInfo(JObject info)
        //{
        //    try
        //    {
        //        var cache = MemoryCache.Default;
        //        cache.Set(info.Value<string>("MerchantTradeNo"), info, DateTime.Now.AddMinutes(60));
        //        return ResponseOK();
        //    }
        //    catch (Exception e)
        //    {
        //        return ResponseError();
        //    }
        //}
        //private HttpResponseMessage ResponseError()
        //{
        //    var response = new HttpResponseMessage();
        //    response.Content = new StringContent("0|Error");
        //    response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
        //    return response;
        //}
        //private HttpResponseMessage ResponseOK()
        //{
        //    var response = new HttpResponseMessage();
        //    response.Content = new StringContent("1|OK");
        //    response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
        //    return response;
        //}
    }

    public class OrdersJsonViewModel
    {
        public string MerchantTradeNo { get; set; }
        public string MerchantTradeDate { get; set; }
        public string TotalAmount { get; set; }
        public string TradeDesc { get; set; }
        public string ItemName { get; set; }
        public string ExpireDate { get; set; }
        public string CustomField1 { get; set; }
        public string CustomField2 { get; set; }
        public string CustomField3 { get; set; }
        public string CustomField4 { get; set; }
        public string ReturnURL { get; set; }
        public string OrderResultURL { get; set; }
        public string PaymentInfoURL { get; set; }
        public string ClientRedirectUR { get; set; }
        public string MerchantID { get; set; }
        public string IgnorePayment { get; set; }
        public string PaymentType { get; set; }
        public string ChoosePayment { get; set; }
        public string EncryptType { get; set; }
        public string CheckMacValue { get; set; }
        
        public string apptlistid { get; set; }//指定掛號序列
    }
}
