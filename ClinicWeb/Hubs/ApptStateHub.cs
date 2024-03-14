using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using NuGet.Protocol;

namespace ClinicWeb.Hubs
{
    public interface IApptStateHub 
    {
        Task Set_State(string jsonMsg);
    }
    public class ApptStateHub : Hub<IApptStateHub>
    {
        public async Task Set_State(string jsonMsg)
        {
            //string id = Context.ConnectionId;
            ////Console.WriteLine($"已連線: {dictConnectionID[id].doctorId} {dictConnectionID[id].department} {dictConnectionID[id].room} {dictConnectionID[id].doctorName}");
            //var data = new
            //{
            //    doctorId = doctorId,
            //    apptListId = apptListId,
            //    stateId = stateId
            //};
            await Clients.All.Set_State(jsonMsg);
            //await Clients.All.SendAsync("Listener_PatientState", data.ToJson());
        }
    }
}
