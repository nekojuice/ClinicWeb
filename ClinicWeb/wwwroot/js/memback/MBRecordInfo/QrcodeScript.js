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
//function updateCurrentTime() {
//    const currentTime = new Date();
//    $("#currentTimeLabel").text("現在時間: " + formattedTime);
//}
/*const formattedTime = currentTime.toLocaleTimeString();*/

//上班打卡
const startbutton = document.getElementById('startbutton');
startbutton.addEventListener('click', function () {
    /*const qrCodeContent = document.getElementById('qrCodeContent');*/
    const html5QrCodeon = new Html5Qrcode("cameraPreviewon");
    html5QrCodeon.start(
        { facingMode: "environment" },
        {
            fps: 30,
            qrbox: { width: 300, height: 300 }  // QR 大小
        },
        (qrCodeMessage) => {
            /*qrCodeContent.textContent = "Scanned QR code: " + qrCodeMessage;*/
            const message = qrCodeMessage;
            console.log(qrCodeMessage);
			check(message);
			html5QrCodeon.stop(); // 停止
			$('#getOnModal').modal('hide');
        },
        (errorMessage) => {
            console.error(errorMessage);
        }
    );
});
//下班打卡
const endbutton = document.getElementById('endbutton');
endbutton.addEventListener('click', function () {
    /*const qrCodeContent = document.getElementById('qrCodeContent');*/

    const html5QrCodeoff = new Html5Qrcode("cameraPreviewoff");

    html5QrCodeoff.start(
        { facingMode: "environment" },
        {
            fps: 30,
            qrbox: { width: 300, height: 300 }  // QR 大小
        },
        (qrCodeMessage) => {
            /*qrCodeContent.textContent = "Scanned QR code: " + qrCodeMessage;*/
			console.log(qrCodeMessage); 
			const message = qrCodeMessage;
			console.log(qrCodeMessage);
			out(message);
			html5QrCodeoff.stop(); // 停止
			$('#getOffModal').modal('hide');
        },
        (errorMessage) => {
            console.error(errorMessage);
        }
    );
});


//$("#btn1").click(function (e) {
//	e.preventDefault();

//	const currentDate = new Date();
//	const isOnTime = currentDate.getHours() < 9;
//	const requestData = {
//		"FEmployeeId": empID,
//		"FCheckInTime": currentDate.toISOString(),
//		"FWorkDate": currentDate.toISOString(),
//		"fAttendanceCIS": isOnTime ? "準時到" : "遲到",
//	};

//	$.ajax({
//		type: "POST",
//		url: "/Fqrcode/CHeckIn",
//		contentType: "application/json",
//		data: JSON.stringify(requestData),
//		cache: false,
//		success: function (response) {
//			console.log("Success:", response);
//			updateDataTable();
//			toastr.success(response)
//		},
//		error: function (error) {
//			console.error("Error:", error.responseText);
//			toastr.error(error.responseText)
//		}
//	});
//});
////下班
//$(document).ready(function () {
//	$("#btn2").click(function (e) {
//		e.preventDefault();

//		var currentDate = new Date();
//		var isOnTime = currentDate.getHours() > 17;
//		var requestData = {
//			"FEmployeeId": empID,
//			"FCheckOutTime": currentDate.toISOString(),
//			"FWorkDate": currentDate.toISOString(),
//			"fAttendanceCOS": isOnTime ? "正常下班" : "早退",
//		};

//		$.ajax({
//			type: "POST",
//			url: "/Fqrcode/CHeckOut",
//			contentType: "application/json",
//			data: JSON.stringify(requestData),
//			success: function (response) {
//				console.log("Success:", response);
//				updateDataTable();
//				toastr.success(response)
//			},
//			error: function (error) {
//				console.error("Error:", error.responseText);
//				toastr.error(error.responseText)
//			}
//		});
//	});
function check(empID) {
	const currentDate = new Date();
	const isOnTime = currentDate.getHours() < 9;
	const requestData = {
		"FEmployeeId": empID,
		"FCheckInTime": currentDate.toISOString(),
		"FWorkDate": currentDate.toISOString(),
		"fAttendanceCIS": isOnTime ? "準時到" : "遲到",
	};

	$.ajax({
		type: "POST",
		url: "/Fqrcode/CHeckIn",
		contentType: "application/json",
		data: JSON.stringify(requestData),
		cache: false,
		success: function (response) {
			console.log("Success:", response);
			/*toastr.success(response)*/
			Swal.fire({
				title: "Good job!",
				text: response,
				icon: "success"
			});
		},
		error: function (error) {
			console.error("Error:", error.responseText);
			Swal.fire({
				icon: "error",
				title: "Oops...",
				text: error.responseText,
			});
		}
	});
}

function out(empID) {
	var currentDate = new Date();
	var isOnTime = currentDate.getHours() > 17;
	var requestData = {
		"FEmployeeId": empID,
		"FCheckOutTime": currentDate.toISOString(),
		"FWorkDate": currentDate.toISOString(),
		"fAttendanceCOS": isOnTime ? "正常下班" : "早退",
	};

	$.ajax({
		type: "POST",
		url: "/Fqrcode/CHeckOut",
		contentType: "application/json",
		data: JSON.stringify(requestData),
		success: function (response) {
			console.log("Success:", response);
			Swal.fire({
				title: "Good job!",
				text: response,
				icon: "success"
			});
		},
		error: function (error) {
			console.error("Error:", error.responseText);
			Swal.fire({
				icon: "error",
				title: "Oops...",
				text: error.responseText,
			});
		}
	});

}