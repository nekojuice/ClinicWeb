using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using QRCoder;
using ClinicWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Controllers
{
    public class FqrcodeController : Controller
    {
        private readonly ClinicSysContext _context;
        public IActionResult Index()
        {
            return View();
        }

        //QrCode圖片生成及發送範例
        public ActionResult GenerateQRCode(string qrText) //製作圖片陣列
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
            using PngByteQRCode qRCode = new PngByteQRCode(qrCodeData);
            byte[] image = qRCode.GetGraphic(10);

            using (MemoryStream ms = new MemoryStream())
            {
                var imgSrc = "data:image/png;base64," + Convert.ToBase64String(image);
                return View((object)imgSrc);
            }
        }
        public IActionResult SendQRCodeEmail()
        {
            // 生成QR碼
            string qrText = "https://www.google.com";//QRCode內容改成員工ID
            byte[] qrCodeImage = GetQRCodeByteArray(qrText);

            // 設定SMTP郵件相關資訊
            string smtpServer = "your_smtp_server";
            int smtpPort = 587;
            string smtpUsername = "your_username";
            string smtpPassword = "your_password";
            string senderEmail = "sender@example.com";
            string recipientEmail = "recipient@example.com";
            string subject = "QR Code Email";
            string body = "Please find the QR code attached.";

            // 建立郵件訊息
            MailMessage mailMessage = new MailMessage(senderEmail, recipientEmail, subject, body);
            mailMessage.Attachments.Add(new Attachment(new MemoryStream(qrCodeImage), "QRCode.png"));

            // 設定SMTP客戶端
            SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
            smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
            smtpClient.EnableSsl = true;

            try
            {
                // 發送郵件
                smtpClient.Send(mailMessage);
                return View("EmailSent");
            }
            catch (Exception ex)
            {
                // 處理郵件發送失敗的例外
                return View("EmailError", ex);
            }
        }
        public byte[] GetQRCodeByteArray(string qrText)
        {
            IActionResult result = GenerateQRCode(qrText);

            if (result is FileContentResult fileContentResult)
            {
                return fileContentResult.FileContents;
            }
            else return null;
        }

        //打卡系統
        //[Route("{area}/{controller}/{action}")]
        [HttpPost]
        public IActionResult CheckIn([FromBody] AttendanceTAttendance data)
        {

            if (data == null)
            {
                return BadRequest("Invalid data");
            }


            var attendanceRecord = _context.AttendanceTAttendance
                .FirstOrDefault(x => x.FEmployeeId == data.FEmployeeId && x.FWorkDate == data.FWorkDate);
            if (attendanceRecord != null)
            {
                return BadRequest("已打上班卡");
            }


            try
            {
                _context.AttendanceTAttendance.Add(data);
                _context.SaveChanges();
                return Ok("上班打卡成功");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //[Route("{area}/{controller}/{action}")]
        [HttpPost]
        public IActionResult CheckOut([FromBody] AttendanceTAttendance data)
        {
            if (data == null)
            {
                return BadRequest("Invalid data");
            }
            var attendanceRecord = _context.AttendanceTAttendance
                .FirstOrDefault(x => x.FEmployeeId == data.FEmployeeId && x.FWorkDate == data.FWorkDate);

            if (attendanceRecord == null)
            {
                return BadRequest("未打上班卡");
            }

            if (attendanceRecord.FAttendanceCos != null)
            {
                return BadRequest("已打下班卡");
            }

            try
            {
                attendanceRecord.FCheckOutTime = data.FCheckOutTime;
                attendanceRecord.FAttendanceCos = data.FAttendanceCos;


                _context.AttendanceTAttendance.Update(attendanceRecord);
                _context.SaveChanges();
                return Ok("下班打卡成功");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }


        }
    }
}
