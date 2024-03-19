document.addEventListener('DOMContentLoaded', function () {
    const qrCodeContent = document.getElementById('qrCodeContent');

    const html5QrCode = new Html5Qrcode("cameraPreview");

    html5QrCode.start(
        { facingMode: "environment" },
        {
            fps: 30,    // 每秒处理的帧数
            qrbox: 300  // QR 码区域的大小
        },
        (qrCodeMessage) => {
            qrCodeContent.textContent = "Scanned QR code: " + qrCodeMessage;
            console.log(qrCodeMessage); // 打印扫描到的QR码值到控制台
            html5QrCode.stop(); // 停止扫描
        },
        (errorMessage) => {
            console.error(errorMessage);
        }
    );
});