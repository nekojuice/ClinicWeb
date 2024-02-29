﻿function init_callingTable() {
    if (!$.fn.DataTable.isDataTable('#callingTable')) {
        $('#callingTable').dataTable({
            columns: [
                { "data": "member_id", "visible": false },
                { "data": "clinicListId", "visible": false },
                { "data": "status_id", "visible": false },
                { "data": "診號" },
                { "data": "姓名" },
                { "data": "性別" },
                { "data": "狀態" }
            ],
            fixedHeader: {
                header: false
            },
            language: {
                url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
            },
            //排序目標
            order: [[1, 'asc'], [2, 'asc']], //優先度:狀態id > 診號
            //關閉排序和搜尋目標
            aoColumnDefs: [
                { "bSortable": false, "aTargets": [0, 1, 2, 3, 4, 5] },
                { "bSearchable": false, "aTargets": [0, 1, 2, 3, 4, 5] }
            ],
            //搜尋框
            searching: false,
            //分頁
            bPaginate: false,
            //筆數頁數資訊
            info: false
        });
    }
}
init_callingTable()

async function QueryCallingData() {
    //引入全域變數
    const doctorId = DOCTOR_ID
    const date = CLINIC_DATE

    const datedata = `${date.getFullYear()}/${(date.getMonth() + 1).toString().padStart(2, '0')}/${(date.getDate()).toString().padStart(2, '0')}`

    const shiftId = +$("#ClinicTime").val()

    const jsonData = { 'doctorId': doctorId, 'date': datedata, 'shiftId': shiftId }

    //先清空資料
    $("#callingTable").DataTable().clear();
    //要資料
    const response = await fetch(`/ClinicRoomSys/CallingSys/Get_CallingList`,
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                // Content-Type: 'application/x-www-form-urlencoded',
            },
            body: JSON.stringify(jsonData)
        })
    const data = await response.json()
    //填入資料
    $("#callingTable").DataTable().rows.add(data).draw()
}

$("#ClinicTime").on('change', function () {

    $("#ClinicTime option[value='0']").remove();

    QueryCallingData()
})




//左表點擊事件
$("#callingTable tbody").on('mousedown', 'tr', function () {
    const _index_DataTable = $('#callingTable').DataTable().row(this).index();
    if (_index_DataTable == null) { return; } //忽略無選擇時
    if ($(this).hasClass('selected')) { return; }   //忽略選擇同一row
    
    const dataObject = $('#callingTable').DataTable().row(_index_DataTable).data()
    MEMBER_ID = dataObject.member_id;
    CLINICLIST_ID = dataObject.clinicListId;

    $(this).siblings().removeClass('selected');
    $(this).addClass('selected');

    //點擊後觸發的事件
    //fetch右表
    on_memberClick()

    //下方console邏輯
    $("#call_currentNumber").html(dataObject.診號)
    $("#call_currentName").html(dataObject.姓名)

});


//signalR
let connection = new signalR.HubConnectionBuilder().withUrl("/CallingHub").build();

//[宣告on] 進入畫面時註冊
connection.on("DoctorLoginCall", function (doctorId, department, room, doctorName) {
    console.log(`${doctorId}, ${department}, ${room}, ${doctorName}`)
});
//[宣告on] 選擇時段
connection.on("DoctorShiftChange", function (doctorId, shift) {
    console.log(`${doctorId}, ${shift}`)
});
//[宣告on] 叫號
connection.on("DoctorNumberChange", function (doctorId, number) {
    console.log(`${doctorId}, ${number}`)
});
//signalR連線初始化
connection.start().then(function () {
    //document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

//進入畫面時註冊
(async () => {
    //引入全域變數
    const response1 = await fetch(URL_Get_EmpId, { method: 'POST' });
    const doctor_ID = await response1.text()

    const date = CLINIC_DATE
    const datedata = `${date.getFullYear()}/${(date.getMonth() + 1).toString().padStart(2, '0')}/${(date.getDate()).toString().padStart(2, '0')}`

    const jsonData = { 'doctorId': doctor_ID, 'date': datedata}
    const response2 = await fetch(Get_EmpInfo,
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                // Content-Type: 'application/x-www-form-urlencoded',
            },
            body: JSON.stringify(jsonData)
        });
    const data = await response2.json();
    console.log(data);

    //更改當前診號
    $("#spanShowDept").text(data.department)
    $("#spanShowRoom").text(data.room)

    //註冊signalR
    connection.invoke("DoctorLoginCall", DOCTOR_ID, data.department, data.room, data.doctorName).catch(function (err) {
        return console.error(err.toString());
    });
})();
