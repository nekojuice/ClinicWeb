function init_payment_Table() {
    if (!$.fn.DataTable.isDataTable('#paymentDataTable')) {
        $('#paymentDataTable').dataTable({
            columns: [
                { "data": "apptlistid", "visible": false },
                { "data": "日期" },
                { "data": "時段" },
                { "data": "科別" },
                { "data": "醫師名稱" },
                { "data": "看診狀態" },
                { "data": "繳費狀態" },
                {
                    "data": "繳費",
                    "render": function (data, type, row) { return `<button type="button" class="btn btn-info .indexSelector" data-bs-toggle="modal" data-bs-target="#goPayModal" onclick="showPayInfo('${row.apptlistid}','${row.繳費狀態}','${row.日期}','${row.時段}','${row.科別}','${row.醫師名稱}')">查看</button>` }
                }
            ],
            fixedHeader: {
                header: false
            },
            order: [[1, 'asc']],
            language: {
                url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
            },            //關閉排序和搜尋目標
            aoColumnDefs: [
                { "bSortable": false, "aTargets": [0, 1, 2, 3, 4, 5, 6, 7] },
                { "bSearchable": false, "aTargets": [0, 1, 2, 3, 4, 5, 6, 7] }
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
init_payment_Table();

//立即搜尋未繳費資料
(async () => {
    const response = await fetch(`/MBAppointmentInfo/Get_PaitientPayment`, { method: "GET" })
    const data = await response.json()
    $('#paymentDataTable').DataTable().clear().draw
    $("#paymentDataTable").DataTable().rows.add(data).draw()
})();

let _selected_apptlistid = -1;
function showPayInfo(apptlistid, paystate, date, shift, dept, docname) {
    _selected_apptlistid = apptlistid
    if (paystate != '未付款') {
        $("#btn_goPay").prop('disabled', true)
    } else {
        $("#btn_goPay").prop('disabled', false)
    }
    $("#span-paymag").text(`${date} ${shift}, ${dept} ${docname} 醫師`)
    $("#span-state").text(`繳費狀況: ${paystate}`);
};

$("#btn_goPay").on('click', async function (e) {
    //防呆判斷
    if (_selected_apptlistid == -1) { return; }


    //e.preventDefault(); //因為送出就跳轉到綠界，這個可以停住確認自己的console.log的內容
    let formData = $("#form").serializeArray();
    var json = {};
    $.each(formData, function () {
        json[this.name] = this.value || "";
    });
    console.log(json);
    json.apptlistid = _selected_apptlistid;
    const jsonString = JSON.stringify(json)

    const response = await fetch('https://localhost:7071/Ecpay/AddOrders',
        {
            'method': 'POST',
            'headers': {
                'Content-Type': 'application/json;charset=UTF-8'
            },
            'body': jsonString
        });
    const msg = await response.text()
    console.log(msg)
})