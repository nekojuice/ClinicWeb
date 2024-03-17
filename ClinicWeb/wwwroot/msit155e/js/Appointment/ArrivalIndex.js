//鎖定demo日期
const today = "2023/12/01"

//初始化表格
function init_ArrivalTable() {
    if (!$.fn.DataTable.isDataTable('#arrivalDataTable')) {
        $('#arrivalDataTable').dataTable({
            columns: [
                { "data": "clinicAppt_id", "visible": false },
                { "data": "national_id", "visible": false },
                { "data": "姓名" },
                { "data": "日期" },
                { "data": "時段" },
                { "data": "科別" },
                { "data": "醫師名稱" },
                { "data": "診號" },
                { "data": "繳費狀態" },
                {
                    "data": "看診狀態", "render": function (data, type, row) {
                        if (data=="未報到") {
                            return `<button type="button" class="btn btn-danger indexSelector" onclick="set_arrival('${row.clinicAppt_id}','${row.national_id}')">進行報到</button>`;
                        }
                        else {
                            return data;
                        }
                    }
                }
                /*,{ "data": "編輯", "render": function (data, type, row) { return `<button type="button" class="btn btn-info btn_edit indexSelector" data-toggle="modal" data-target=".modApptModal" onclick="ModApptForm_OPEN('${row.clinicAppt_id}')">編輯</button>` } }*/
            ],
            fixedHeader: {
                header: true
            },
            language: {
                url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
            },
            order: [[1, 'asc']]
        });
    }
}
init_ArrivalTable();

//欄位index監聽器
let _index_apptDataTable;
$("#arrivalDataTable tbody").on('click', '.indexSelector', function () {
    const selectTr = $(this).closest('tr')
    _index_apptDataTable = $('#arrivalDataTable').DataTable().row(selectTr).index()
    console.log(_index_apptDataTable)
});

//執行今天查詢
(async () => {
    
    $("#h3_todayString").text(today + ' 掛號病患一覽');


    const response = await fetch(`/Appointment/Arrival/Get_ArrivalManagerList/${today}`, { method: "GET" })
    const data = await response.json()
    $('#arrivalDataTable').DataTable().clear().draw
    $("#arrivalDataTable").DataTable().rows.add(data).draw()
    //預設只顯示未報到
    $("#isContainArrival").click()
})();

//過濾 是否只顯示未報到
$("#isContainArrival").on('click', function () {
    if ($("#isContainArrival").prop('checked') == true) {
        $('#arrivalDataTable').DataTable().column(9).search('進行報到').draw();
    } else {
        $('#arrivalDataTable').DataTable().column(9).search("").draw();
    }
})

async function set_arrival(clinicAppt_id, national_id) {
    const response1 = await fetch(`/Appointment/Arrival/GET_Arrival/${today}/${national_id}`, { method: "GET" })
    const data1 = await response1.text()
    console.log(data1)

    const response2 = await fetch(`/Appointment/Arrival/Get_ArrivalManagerList/${today}/${clinicAppt_id}`, { method: "GET" })
    const data2 = await response2.json()
    //console.log(data2)
    $('#arrivalDataTable').DataTable().row(_index_apptDataTable).data(data2)
}