using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR;
using System.Net;
using System.Security.Cryptography.Xml;

namespace ClinicWeb.Hubs
{
    public interface IArrivalHub
    {
        Task CardInsert(string ip, string memInfo, string jsondata);
        Task CardPull(string ip);
        Task Listener_ApptInfo(string jsondata);
    }


    public class ArrivalHub : Hub<IArrivalHub>
    {
        private static Dictionary<string, string> _dictConnID_IP = new Dictionary<string, string>();

        //連線事件
        public override Task OnConnectedAsync()
        {
            string id = Context.ConnectionId;
            //string ip = Context.Features.Get<IHttpConnectionFeature>().RemoteIpAddress.ToString(); //ip錯誤
            string ip = Context.GetHttpContext().Connection.RemoteIpAddress.ToString();
            Console.WriteLine($"{id}, {ip}");

            _dictConnID_IP.Add(id, ip);
            return base.OnConnectedAsync();
        }

        //斷線事件
        public override async Task<Task> OnDisconnectedAsync(Exception? exception)
        {
            //string id = Context.ConnectionId;
            //Console.WriteLine($"已斷線: {dictConnectionID[id].doctorId} {dictConnectionID[id].department} {dictConnectionID[id].room} {dictConnectionID[id].doctorName}");

            _dictConnID_IP.Remove(Context.ConnectionId);
            //await Clients.All.SendAsync("Listener_ClinicInfo", JsonSerializer.Serialize(dictConnectionID.Values.ToList()));
            return base.OnDisconnectedAsync(exception);
        }

        //controller插卡時聯絡hub，給報到機資料
        public async Task CardInsert(string ip, string memInfo, string jsondata)
        {
            string id = _dictConnID_IP.FirstOrDefault(x => x.Value == ip).Key;
            Console.WriteLine($"{id}, {ip}, {jsondata}");
            await Clients.User(id).CardInsert(ip, memInfo, jsondata);
        }
        public async Task CardPull(string ip)
        {
            string id = _dictConnID_IP.FirstOrDefault(x => x.Value == ip).Key;
            await Clients.User(id).CardPull(ip);
        }
    }
}
