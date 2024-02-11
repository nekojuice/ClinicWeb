//順便作為筆記區用

//bugs:
//1. 重繪翻頁問題 現在是右表刪除時
//2. 粉紅色bg沒有對其，用事件偵測 ?

//function GetTable() {
//    fetch("/Appointment/ApptSys/ClinicInfo/" + $("#Date").val())
//        .then((response) => { return response.text(); })
//        .then((data) => { $("#clinicDataDiv").html(data); })
//        .then((x) => { QueryClinicInfo(); })
//        .catch()
//}

//設法解決快速查詢時的錯誤
//停止fetch控制器
const controller = new AbortController();
const signal = controller.signal;

//停止datatable ajax方法
function stop_datatableAjax($tableSelector) {
    $tableSelector.DataTable().context[0].jqXHR.abort()
}


//$('#clinicDataTable').dataTable({
//    ajax: {
//        type: 'GET',
//        url: ("/Appointment/ApptSys/GET_ClinicInfoList/" + $("#Date").val()),
//        dataSrc: function (json) {
//            searchDept();
//            return json;
//        }
//    },
//    destroy: true,
//    columns: [
//        { "data": "id", "visible": false },
//        { "data": "日期" },
//        { "data": "時段" },
//        { "data": "科別" },
//        { "data": "醫師名稱" },
//        { "data": "上限人數" },
//        { "data": "預約人數" }
//    ],
//    fixedHeader: {
//        header: true
//    },
//    language: {
//        url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
//    },
//    order: [[1, 'asc']]
//});



//$("#apptDataTable").dataTable({
//    ajax: {
//        type: 'GET',
//        url: ("/Appointment/ApptSys/GET_ApptRecordList/" + clinicID),
//        dataSrc: function (json) { return json; }
//    },
//    destroy: true,
//    columns: [
//        { "data": "clinic_id", "visible": false },
//        { "data": "member_id", "visible": false },
//        { "data": "診號" },
//        { "data": "姓名" },
//        { "data": "生日" },
//        { "data": "性別" },
//        { "data": "身分證字號" },
//        { "data": "退掛" },
//        { "data": "看診狀態" }
//    ],
//    fixedHeader: {
//        header: true
//    },
//    language: {
//        url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
//    },
//    order: [[2, 'asc']]
//});


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