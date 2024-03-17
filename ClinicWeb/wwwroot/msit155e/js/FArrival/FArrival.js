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
                { "data": "報到狀態" }
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