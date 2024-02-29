using Microsoft.AspNetCore.SignalR;

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
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            string id = Context.ConnectionId;
            Console.WriteLine($"已斷線: {dictConnectionID[id].doctorId} {dictConnectionID[id].department} {dictConnectionID[id].room} {dictConnectionID[id].doctorName}");
            

            dictConnectionID.Remove(Context.ConnectionId);  
            return base.OnDisconnectedAsync(exception);
        }

        //自訂的連線事件
        public async Task DoctorLoginCall(string doctorId, string department, string room, string doctorName)
        {
            dictConnectionID.Add(Context.ConnectionId, new {
                doctorId = doctorId,
                department = department,
                room = room,
                doctorName = doctorName
            });
            string id = Context.ConnectionId;
            //Console.WriteLine($"已連線: {dictConnectionID[id].doctorId} {dictConnectionID[id].department} {dictConnectionID[id].room} {dictConnectionID[id].doctorName}");
            await Clients.All.SendAsync("ReceiveMessage", );
        }
        //時段更改事件
        public async Task DoctorShiftChange(string user, string message)
        {
            //var content = $"{user} 於{DateTime.Now.ToShortTimeString()}說：{message}";

            //await Clients.All.SendAsync("ReceiveMessage", content);
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public async Task DoctorNumberChange(string user, string message)
        {
            //var content = $"{user} 於{DateTime.Now.ToShortTimeString()}說：{message}";

            //await Clients.All.SendAsync("ReceiveMessage", content);
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }




        public async Task SendMessage(string user, string message)
        {
            //var content = $"{user} 於{DateTime.Now.ToShortTimeString()}說：{message}";

            //await Clients.All.SendAsync("ReceiveMessage", content);
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
