//document.addEventListener('DOMContentLoaded', function () {
//    const qrCodeContent = document.getElementById('qrCodeContent');

//    const html5QrCodeon = new Html5Qrcode("cameraPreviewon");

//    html5QrCodeon.start(
//        { facingMode: "environment" },
//        {
//            fps: 30,
//            qrbox: 300  // QR 大小
//        },
//        (qrCodeMessage) => {
//            qrCodeContent.textContent = "Scanned QR code: " + qrCodeMessage;
//            console.log(qrCodeMessage); // 打印扫描到的QR码值到控制台
//            html5QrCodeon.stop(); // 停止
//        },
//        (errorMessage) => {
//            console.error(errorMessage);
//        }
//    );
//});

//上班打卡
const startbutton = document.getElementById('startbutton');
startbutton.addEventListener('click', function () {
    const qrCodeContent = document.getElementById('qrCodeContent');

    const html5QrCodeon = new Html5Qrcode("cameraPreviewon");

    html5QrCodeon.start(
        { facingMode: "environment" },
        {
            fps: 30,
            qrbox: { width: 300, height: 300 }  // QR 大小
        },
        (qrCodeMessage) => {
            qrCodeContent.textContent = "Scanned QR code: " + qrCodeMessage;
            console.log(qrCodeMessage); // 打印扫描到的QR码值到控制台

            html5QrCodeon.stop(); // 停止
        },
        (errorMessage) => {
            console.error(errorMessage);
        }
    );
});
//下班打卡
const endbutton = document.getElementById('endbutton');
endbutton.addEventListener('click', function () {
    const qrCodeContent = document.getElementById('qrCodeContent');

    const html5QrCodeoff = new Html5Qrcode("cameraPreviewoff");

    html5QrCodeoff.start(
        { facingMode: "environment" },
        {
            fps: 30,
            qrbox: { width: 300, height: 300 }  // QR 大小
        },
        (qrCodeMessage) => {
            qrCodeContent.textContent = "Scanned QR code: " + qrCodeMessage;
            console.log(qrCodeMessage); // 打印扫描到的QR码值到控制台

            html5QrCodeoff.stop(); // 停止
        },
        (errorMessage) => {
            console.error(errorMessage);
        }
    );
});