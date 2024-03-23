function init_ApptTable() {
    if (!$.fn.DataTable.isDataTable('#apptDataTable')) {
        $("#apptDataTable").dataTable({
            columns: [
                { "data": "clinicAppt_id", "visible": false },
                { "data": "日期" },
                { "data": "時段" },
                { "data": "科別" },
                { "data": "醫師名稱" },
                { "data": "診號" },
                {
                    "data": "報到狀態",
                    "render":
                        function (data, type, row) {
                            if (data == '未報到') {
                                return `<button type="button" class="btn btn-danger indexSelector" onclick="goCheckIn('${row.clinicAppt_id}')">進行報到</button>`
                            }
                            else {
                                return data;
                            }
                        }
                }
            ],
            fixedHeader: {
                header: false
            },
            language: {
                url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
            },
            order: [[0, 'asc']],            //關閉排序和搜尋目標
            aoColumnDefs: [
                { "bSortable": false, "aTargets": [0, 1, 2, 3, 4, 5, 6] },
                { "bSearchable": false, "aTargets": [0, 1, 2, 3, 4, 5, 6] }
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
init_ApptTable()

let _index_DataTable = -1
$("#apptDataTable tbody").on('click', '.indexSelector', function () {
    const selectTr = $(this).closest('tr')
    _index_DataTable = $('#apptDataTable').DataTable().row(selectTr).index();   
});


async function goCheckIn(clinicAppt_id) {
    const result = await fetch(`${Url_goCheckIn}/${clinicAppt_id}`)
    const data = await result.json()
    $('#apptDataTable').DataTable().row(_index_DataTable).data(data);
}

let connection = new signalR.HubConnectionBuilder()
    .withUrl("/ArrivalHub")
    .withAutomaticReconnect()
    .build();

connection.on("CardInsert", function (ip, memInfo, jsonstring) {
    const memInfodata = JSON.parse(memInfo)
    const jsondata = JSON.parse(jsonstring)
    console.log(memInfodata)
    console.log(jsondata)

    $("#c_name").text(memInfodata.c_name);
    $("#c_gender").text(memInfodata.c_gender);
    $("#c_nationalid").text(memInfodata.c_nationalid);
    $("#c_birthday").text(memInfodata.c_birthday);
    $("#apptDataTable").DataTable().clear();
    $("#apptDataTable").DataTable().rows.add(jsondata).draw()
    $("#dataSection").toggle()
    $("#msgSection").toggle()
});
connection.on("CardPull", function (ip) {
    $("#c_name").text("");
    $("#c_gender").text("");
    $("#c_nationalid").text("");
    $("#c_birthday").text("");
    $("#apptDataTable").DataTable().clear().draw()
    $("#dataSection").toggle()
    $("#msgSection").toggle()
})
connection.start().then(function () {
    //try {
    //    connection.invoke("Get_ClinicInfo");
    //} catch (err) {
    //    console.error(err);
    //}
}).catch(function (err) {
    return console.error(err.toString());
});