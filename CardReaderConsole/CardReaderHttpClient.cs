using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CardReader
{
    public class CardReaderHttpClient
    {
        //寫死的報到日期
        static string Arrivaldate = "2023/12/01";
        string ip = GetLocalIPAddress();


        //string apiUrl = $"https://localhost:7071/Appointment/Arrival/GET_Arrival/{Arrivaldate}";//舊報到路由
        //string apiUrl = $"https://localhost:7071/FArrival/Remote_CardInsert";

        //string serverUrl = "https://192.168.0.20:7071"; //nkj家裡測試
        //string serverUrl = "https://192.168.21.14:7071"; //classroom 201 nkj位置 14號
        string serverUrl = "https://192.168.71.174:7071"; //classroom 201 wifi
        
        string insertCardUrl = "FArrival/Remote_CardInsert";
        string pullCardUrl = "FArrival/Remote_PullCard";

        async public Task<string> GET_ApptData(string nationalId) 
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient httpClient = new HttpClient(clientHandler);
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync($"{serverUrl}/{insertCardUrl}/{Arrivaldate}/{nationalId}/{ip}");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return content;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }
        async public Task<string> PullCard()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient httpClient = new HttpClient(clientHandler);

            await httpClient.GetAsync($"{serverUrl}/{pullCardUrl}/{ip}");

            return "卡片已拔除";
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}
