using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CardReader
{
    public class CardReaderHttpClient
    {
        //寫死的報到日期
        static string Arrivaldate = "2023/12/01";

        HttpClient httpClient = new HttpClient();
        string apiUrl = $"https://localhost:7071/Appointment/Arrival/GET_Arrival/{Arrivaldate}";

        async public Task<string> GET_ApptData(string nationalId) 
        {
            HttpResponseMessage response = await httpClient.GetAsync($"{apiUrl}/{nationalId}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return content;
            }
            return null;
        }
    }
}
