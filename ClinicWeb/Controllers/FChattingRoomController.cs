using ClinicWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Controllers
{
    [Authorize(Policy = "frontendpolicy")]
    public class FChattingRoomController : Controller
	{
        private readonly IWebHostEnvironment _webHostEnvironment;
        public IActionResult FChattingRoom(MemberMemberList user)
		{
			return View(user);
		}
        public FChattingRoomController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        // POST 請求以保存 Canvas 圖像
        [HttpPost]
        public async Task<IActionResult> SaveCanvasImage([FromBody] ImageData imageData)
        {
            try
            {
                // 將 base64 圖片數據轉換為 byte[]
                var imgData = Convert.FromBase64String(imageData.Image.Replace("data:image/png;base64,", ""));

                // 生成唯一的檔名
                var uniqueFileName = $"{Guid.NewGuid().ToString()}.png";

                // 指定文件路徑（存儲到 wwwroot/images 資料夾下）
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", uniqueFileName);

                // 寫入文件
                await System.IO.File.WriteAllBytesAsync(filePath, imgData);

                // 返回成功消息和檔名
                return Ok(new { message = "Image saved successfully.", fileName = uniqueFileName });
            }
            catch (Exception ex)
            {
                // 如果出現異常，返回錯誤消息
                return StatusCode(500, "Error saving image: " + ex.Message);
            }
        }

        // 定義接收圖片數據的模型類
        public class ImageData
        {
            public string Image { get; set; }
        }
    }
}
