function init_payment_Table() {
    if (!$.fn.DataTable.isDataTable('#paymentDataTable')) {
        $('#paymentDataTable').dataTable({
            columns: [
                { "data": "id", "visible": false },
                { "data": "日期" },
                { "data": "時段" },
                { "data": "科別" },
                { "data": "醫師名稱" },
                { "data": "診間" },
                { "data": "診號" },
                { "data": "看診狀態" },
                {
                    "data": "修改",
                    "render": function (data, type, row) { return '<button type="button" class="btn btn-danger .indexSelector" data-bs-toggle="modal" data-bs-target="#cancelApptModal" onclick="rowBtnOnClick(' + row.id + ')">退掛</button>' }
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
                { "bSortable": false, "aTargets": [0, 1, 2, 3, 4, 5, 6, 7, 8] },
                { "bSearchable": false, "aTargets": [0, 1, 2, 3, 4, 5, 6, 7, 8] }
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
init_payment_Table()