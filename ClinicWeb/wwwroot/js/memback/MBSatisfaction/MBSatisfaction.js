
const table = document.querySelector('#datatable');

function QueryMedicalRecords() {
    $('#MedicalRecordsTable').dataTable({
        ajax: {
            type: 'POST',
            url: (`/MBSatisfactionController/Get_MedicalRecords`),
            dataSrc: function (json) { return json; }
        },
        destroy: true,
        columns: [
            // { "data": "id", "visible": false },
            { "data": "caseid", "visible": false },
            { "data": "mrid", "visible": false },
            { "data": "看診日期" },
            { "data": "醫師" },
            { "data": "時段" },
            { "data": "科別" },
            { "data": "診間" }
        ],
        fixedHeader: {
            header: true
        },
        order: [[2, 'asc']]
    });

}

QueryMedicalRecords();