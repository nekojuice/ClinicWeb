using ClinicWeb.Models;
using Microsoft.AspNetCore.SignalR;

namespace ClinicWeb.Hubs
{
    public class DrawHub : Hub
    {
        public async Task SendDraw(DrawModel drawData)
        {
            await Clients.All.SendAsync("ReceiveDraw", drawData);
        }
        public async Task ClearCanvas()
        {
            await Clients.All.SendAsync("ClearCanvas");
        }
    }
}
