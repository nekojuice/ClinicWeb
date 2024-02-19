function init_callingTable() {
    if (!$.fn.DataTable.isDataTable('#callingTable')) {
        $('#callingTable').dataTable({
            columns: [
                { "data": "member_id", "visible": false },
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
            order: [[1, 'asc']],
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