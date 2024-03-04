function init_Table() {
    if (!$.fn.DataTable.isDataTable('#apptDataTable')) {
        $('#apptDataTable').dataTable({
            columns: [
                { "data": "id", "visible": false },
                { "data": "日期" },
                { "data": "時段" },
                { "data": "科別" },
                { "data": "醫師名稱" },
                { "data": "診間" },
                { "data": "診號" },
                { "data": "看診狀態" }
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
init_Table()