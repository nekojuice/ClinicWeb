using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClinicWeb.Hubs
{
    public class CallingHub : Hub
    {
        //儲存連線ID與資料的字典
        public static Dictionary<string, dynamic> dictConnectionID = new Dictionary<string, dynamic>();

        //連線事件
        //public override Task OnConnectedAsync()
        //{
        //    return base.OnConnectedAsync();
        //}

        //斷線事件
        public override async Task<Task> OnDisconnectedAsync(Exception? exception)
        {
            //string id = Context.ConnectionId;
            //Console.WriteLine($"已斷線: {dictConnectionID[id].doctorId} {dictConnectionID[id].department} {dictConnectionID[id].room} {dictConnectionID[id].doctorName}");

            dictConnectionID.Remove(Context.ConnectionId);
            await Clients.All.SendAsync("Listener_ClinicInfo", JsonSerializer.Serialize(dictConnectionID.Values.ToList()));
            return base.OnDisconnectedAsync(exception);
        }

        //註冊診間資訊(醫生面板)
        public async Task Set_ClinicInfo(string doctorId, string department, string room, string doctorName)
        {
            dictConnectionID.Add(Context.ConnectionId, new CallingHubModel
            {
                doctorId = doctorId,
                department = department,
                room = room,
                doctorName = doctorName
            });
            string id = Context.ConnectionId;
            //Console.WriteLine($"已連線: {dictConnectionID[id].doctorId} {dictConnectionID[id].department} {dictConnectionID[id].room} {dictConnectionID[id].doctorName}");
            await Clients.All.SendAsync("Listener_ClinicInfo", JsonSerializer.Serialize(dictConnectionID.Values.ToList()));
        }
        //獲取診間資訊(顯示螢幕)
        public async Task Get_ClinicInfo()
        {
            await Clients.All.SendAsync("Listener_ClinicInfo", JsonSerializer.Serialize(dictConnectionID.Values.ToList()));
        }

        //時段更改事件(醫生面板)
        public async Task Set_Shift(string shift)
        {
            CallingHubModel model = dictConnectionID[Context.ConnectionId];
            model.shift = shift;
            model.number = "";
            dictConnectionID[Context.ConnectionId] = model;
            await Clients.All.SendAsync("Listener_ClinicInfo", JsonSerializer.Serialize(dictConnectionID.Values.ToList()));
        }

        //叫號(醫生面板)
        public async Task Set_Number(string number)
        {
            CallingHubModel model = dictConnectionID[Context.ConnectionId];
            model.number = number;
            dictConnectionID[Context.ConnectionId] = model;
            await Clients.All.SendAsync("Listener_ClinicInfo", JsonSerializer.Serialize(dictConnectionID.Values.ToList()));
        }
    }

    class CallingHubModel
    {
        public string? doctorId { get; set; }
        public string? department { get; set; }
        public string? room { get; set; }
        public string? doctorName { get; set; }
        public string? shift { get; set; }
        public string? number { get; set; }
    }
}
