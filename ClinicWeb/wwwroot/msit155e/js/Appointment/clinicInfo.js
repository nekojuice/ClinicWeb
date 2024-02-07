﻿
//左表
$("#Date").change(() => {
    DateOnChange();
    $("#addAppt").prop("disabled", true);
    $("#modAppt").prop("disabled", true);
});

function DateOnChange() {
    if ($("#Date").val() != '--請選擇--') {
        QueryClinicInfo();
    }
    else {
        $('#clinicDataTable').DataTable().clear().draw();
    }
}

//function GetTable() {
//    fetch("/Appointment/ApptSys/ClinicInfo/" + $("#Date").val())
//        .then((response) => { return response.text(); })
//        .then((data) => { $("#clinicDataDiv").html(data); })
//        .then((x) => { QueryClinicInfo(); })
//        .catch()
//}

function QueryClinicInfo() {
    $('#clinicDataTable').dataTable({
        ajax: {
            type: 'GET',
            url: ("/Appointment/ApptSys/ClinicInfo/" + $("#Date").val()),
            dataSrc: function (json) { return json; }
        },
        destroy: true,
        columns: [
            { "data": "id", "visible": false },
            { "data": "日期" },
            { "data": "時段" },
            { "data": "科別" },
            { "data": "醫師名稱" },
            { "data": "上限人數" },
            { "data": "預約人數" }
        ],
        fixedHeader: {
            header: true
        },
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
        },
        order: [[1, 'asc']]
    });
    searchDept()
}

$("#Doctor_Department").change(() => { searchDept() });

function searchDept() {
    if ($("#Doctor_Department").val() == '全部') {
        $('#clinicDataTable').DataTable().column(3).search("").draw();
    }
    else {
        $('#clinicDataTable').DataTable().column(3).search($("#Doctor_Department").val()).draw();
    }
}

$("#ClinicTime_ClinicShifts").change(() => { searchTime() });

function searchTime() {
    if ($("#ClinicTime_ClinicShifts").val() == '全部') {
        $('#clinicDataTable').DataTable().column(2).search("").draw();
    }
    else {
        $('#clinicDataTable').DataTable().column(2).search($("#ClinicTime_ClinicShifts").val()).draw();
    }
}
//左表點擊事件
let $ClinicTBODY = $("#clinicDataTable tbody")
$ClinicTBODY.on('mousedown', 'tr', function () {
    let index = $('#clinicDataTable').DataTable().row(this).index();
    if (index == null) { return; } //忽略無選擇時
    if ($(this).hasClass('selected')) { return; }   //忽略選擇同一row
    //console.log(index)
    let clinicID = $('#clinicDataTable').DataTable().row(index).data().id;

    $(this).siblings().removeClass('selected');
    $(this).addClass('selected');

    getApptData(clinicID);
    $("#addAppt").prop("disabled", false);
    $("#modAppt").prop("disabled", true);
});

let $ApptDataTBODY = $("#apptDataTable tbody")
$ApptDataTBODY.on('mousedown', 'tr', function () {
    let index = $('#apptDataTable').DataTable().row(this).index();
    if (index == null) { return; } //忽略無選擇時
    if ($(this).hasClass('selected')) { return; }   //忽略選擇同一row

    $(this).siblings().removeClass('selected');
    $(this).addClass('selected');

    let clinicId = $('#apptDataTable').DataTable().row(index).data().clinic_id;
    let memberId = $('#apptDataTable').DataTable().row(index).data().member_id;
    console.log(clinicId + " - " + memberId + " / " + index)

    $("#modAppt").prop("disabled", false);
});

//右表
function getApptData(clinicID) {
    $("#apptDataTable").dataTable({
        ajax: {
            type: 'GET',
            url: ("/Appointment/ApptSys/ApptRecord/" + clinicID),
            dataSrc: function (json) { return json; }
        },
        destroy: true,
        columns: [
            { "data": "clinic_id", "visible": false },
            { "data": "member_id", "visible": false },
            { "data": "診號" },
            { "data": "姓名" },
            { "data": "生日" },
            { "data": "性別" },
            { "data": "身分證字號" },
            { "data": "退掛" },
            { "data": "看診狀態" }
        ],
        fixedHeader: {
            header: true
        },
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
        }
    });
}

$("#isCancelled").change(() => { searchIsCancelled() });
function searchIsCancelled() {
    if ($("#isCancelled").val() == '全部') {
        $('#apptDataTable').DataTable().column(7).search("").draw();
    }
    else {
        $('#apptDataTable').DataTable().column(7).search($("#isCancelled").val()).draw();
    }
}

//載入完成時啟動
//載入時選取當前月份
$(document).ready(() => {
    let year = new Date().getFullYear().toString()
    let month = (+(new Date().getMonth().toString()) + 1).toString().padStart(2, 0)
    let yyyymm = `${year}/${month}`

    for (let option of document.getElementById('Date').options) {
        if (option.innerText == yyyymm) {
            document.getElementById('Date').selectedIndex = option.index
        }
    }
    DateOnChange()
})

$("#memberNationalIdSearch").on('input', async (e) => {
    //不符合規則則不查詢
    let regexNatId = new RegExp('^[A-Za-z0-9]{1}[0-9]*$')
    if ($("#memberNationalIdSearch").val() == "" || !regexNatId.test($("#memberNationalIdSearch").val())) {
        $("#memberNationalIdSearchData").html("")
        return;
    }
    const url = "ApptSys/MemberSnap/" + $("#memberNationalIdSearch").val();
    const response = await fetch(url);
    const data = await response.json();

    //有空將顯字串改成一個個label 小標籤樣式
    const dataHtml = (data.map(x => `<button type="button" class="list-group-item list-group-item-action" onclick="memberNationalIdSearchClick(${x.id},'${x.身分證字號} | ${x.姓名} | ${x.性別} | ${x.生日}')">
    ${x.身分證字號} | ${x.姓名} | ${x.性別} | ${x.生日}</button>`)).join("")

    $("#memberNationalIdSearchData").html(dataHtml)
    $("#memberNationalIdSearchData").show()
})

//收起搜尋結果事件
$("#addApptForm").on('click', (e) => {
    if ($(e.target).is($("#memberNationalIdSearch"))) {
        $("#memberNationalIdSearchData").show();
    }
    else { 
        $("#memberNationalIdSearchData").hide()
    }
})
//填入當前目標與執行搜尋
function memberNationalIdSearchClick(id, searchResult) {
    $("#memberNationalIdSearch").val(searchResult)
    $("#memberNationalIdSearchData").hide()
}
//function searchMemberOnType() {
//    const response = await fetch(`${_MemberSnapURL}+${$("#memberNationalIdSearch").val()}`)
//    const data = await response.json()
//    console.log(data)
//}
/*
$(document).ready(function () {
    $('#clinicDataTable').DataTable({
        "proccessing": true,
        "serverSide": true,
        "ajax": {
            url: "/customers",
            type: 'POST',
            headers: { 'RequestVerificationToken': $('@Html.AntiForgeryToken()').val() }
        },
        "columnDefs": [
            {
                "targets": -1,
                "data": null,
                "render": function (data, type, row, meta) {
                    return '<a href="/customers/edit?id=' + row.id + '">Edit</a> | <a href="/customers/details?id=' + row.id + '">Details</a> | <a href="/customers/delete?id=' + row.id + '">Delete</a>';
                },
                "sortable": false
            },
            { "name": "Id", "data": "id", "targets": 0, "visible": false },
            { "name": "Name", "data": "name", "targets": 1 },
            { "name": "PhoneNumber", "data": "phoneNumber", "targets": 2 },
            { "name": "Address", "data": "address", "targets": 3 },
            { "name": "PostalCode", "data": "postalCode", "targets": 4 }
        ],
        "order": [[0, "desc"]]
    });
});*/