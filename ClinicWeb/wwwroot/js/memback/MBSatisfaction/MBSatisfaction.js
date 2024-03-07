
const table = document.querySelector('#datatable');

function QueryMedicalRecords() {
    $('#MedicalRecordsTable').DataTable({
        ajax: {
            type: 'POST',
            url: '/MBSatisfaction/Get_MedicalRecords',
            dataSrc: function (json) { return json; }
        },
        destroy: true,
        columns: [
            { "data": "caseid", "visible": false },
            { "data": "看診日期" },
            { "data": "醫師" },
            { "data": "時段" },
            { "data": "科別" },
            { "data": "診間" },
            {
                "data": "評價",
                "render": function (data, type, row) {
                    return '<button class="btn btn-primary">評價</button>';
                }
            }
        ],
        fixedHeader: {
            header: true
        },
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
        },
        order: [[2, 'asc']]
    });
}

QueryMedicalRecords();