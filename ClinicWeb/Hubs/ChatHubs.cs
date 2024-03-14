using ClinicWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ClinicWeb.Hubs
{
    [Authorize(Policy = "frontendpolicy")]
    public class ChatHubs : Hub
    {
        private readonly ClinicSysContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;//抓到現在使用者資訊

        public ChatHubs(ClinicSysContext testContext, IHttpContextAccessor httpContextAccessor)
        {
            _context = testContext;
            _httpContextAccessor = httpContextAccessor;
        }


        public static List<UserInfo> usersList = new List<UserInfo>();

        public override async Task OnConnectedAsync()
        {
            // 取得目前使用者
            var Claim = _httpContextAccessor.HttpContext?.User.Claims.ToList();//取使用者資料
            var userId = Claim?.Where(a => a.Type == "MemberID").First().Value;


            var user = _context.MemberMemberList.Find(int.Parse(userId));

            if (user != null)
            {
                usersList.Add(new UserInfo
                {
                    id = user.MemberId,
                    Name = user.Name,
                    connectionID = Context.ConnectionId
                });
            }


            // 更新連線 ID 列表
            string jsonString = JsonConvert.SerializeObject(usersList);
            await Clients.All.SendAsync("UpdList", jsonString);

            // 更新聊天內容
            //await Clients.All.SendAsync("UpdContent", user.Name + "已連線");

            await base.OnConnectedAsync();
        }


        /// <summary>
        /// 離線事件
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var user = usersList.Where(p => p.connectionID == Context.ConnectionId).FirstOrDefault();
            if (user != null)
            {
                usersList.Remove(user);
            }
            // 更新連線 ID 列表
            string jsonString = JsonConvert.SerializeObject(usersList);
            await Clients.All.SendAsync("UpdList", jsonString);

            // 更新聊天內容
            //await Clients.All.SendAsync("UpdContent", user.Name + "已離線");

            await base.OnDisconnectedAsync(ex);
        }

        /// <summary>
        /// 傳遞訊息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task SendMessage(string selfname, string message, string sendToID)
        {
            var Claim = _httpContextAccessor.HttpContext?.User.Claims.ToList();//取使用者資料
            var userId = Claim?.Where(a => a.Type == "MemberID").First().Value;


            var user = _context.MemberMemberList.Find(int.Parse(userId));

            var messageContent = new
            {
                SenderID = user.MemberId,
                SenderName = selfname,
                Message = message
            };

            if (string.IsNullOrEmpty(sendToID))
            {
                await Clients.All.SendAsync("UpdContentAll", selfname + " 說: " + message);
            }
            else
            {
                // 接收人
                await Clients.Client(sendToID).SendAsync("UpdContentCatch", messageContent);

                // 發送人
                await Clients.Client(Context.ConnectionId).SendAsync("UpdContentSend", messageContent);
            }
        }

    }
}

