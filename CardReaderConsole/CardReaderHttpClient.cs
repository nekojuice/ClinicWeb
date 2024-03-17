using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        HttpClient httpClient = new HttpClient();
        //string apiUrl = $"https://localhost:7071/Appointment/Arrival/GET_Arrival/{Arrivaldate}";//舊報到路由
        string apiUrl = $"https://localhost:7071/FArrival/Remote_CardInsert";

        async public Task<string> GET_ApptData(string nationalId) 
        {
            
            HttpResponseMessage response = await httpClient.GetAsync($"{apiUrl}/{Arrivaldate}/{nationalId}/{ip}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return content;
            }
            return null;
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
