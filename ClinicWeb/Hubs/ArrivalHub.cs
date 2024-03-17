using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR;
using System.Net;

namespace ClinicWeb.Hubs
{
    public class ArrivalHub : Hub
    {
        public static Dictionary<string, string> dictConnID_IP = new Dictionary<string, string>();

        //連線事件
        public override Task OnConnectedAsync()
        {
            string id = Context.ConnectionId;
            string ip = Context.Features.Get<IHttpConnectionFeature>().RemoteIpAddress.ToString();
            dictConnID_IP.Add(id,ip);
            return base.OnConnectedAsync();
        }

        //斷線事件
        public override async Task<Task> OnDisconnectedAsync(Exception? exception)
        {
            //string id = Context.ConnectionId;
            //Console.WriteLine($"已斷線: {dictConnectionID[id].doctorId} {dictConnectionID[id].department} {dictConnectionID[id].room} {dictConnectionID[id].doctorName}");

            dictConnID_IP.Remove(Context.ConnectionId);
            //await Clients.All.SendAsync("Listener_ClinicInfo", JsonSerializer.Serialize(dictConnectionID.Values.ToList()));
            return base.OnDisconnectedAsync(exception);
        }

        //controller插卡時聯絡hub
        public async Task CardInsert(string ip, string jsondata)
        {
            string id = dictConnID_IP.FirstOrDefault(x => x.Value == ip).Key;
            //Console.WriteLine($"已連線: {dictConnectionID[id].doctorId} {dictConnectionID[id].department} {dictConnectionID[id].room} {dictConnectionID[id].doctorName}");
            await Clients.User(id).SendAsync("Listener_ApptData", "123");
        }

    }
}
