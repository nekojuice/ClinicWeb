
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
            { "data": "mRid", "visible": false },
            { "data": "就診日期" },
            { "data": "醫師" },
            { "data": "時段" },
            { "data": "科別" },
            { "data": "診間" },
            { "data": "patientSatisfaction", "visible": false },
            { "data": "docSatisfaction", "visible": false },
            { "data": "clinicSatisfaction", "visible": false },
            { "data": "sysSatisfaction", "visible": false },
            {
                "data": "mRid",
                "render": function (data, type, row) {

                    // 檢查是否有評價
                    var hiddenValue = row.sysSatisfaction;
                    //console.log(hiddenValue);

                    // 如果有，顯示已評價按鈕
                    if (hiddenValue!=null) {
                        return `<button type="button" class="btn btn-secondary" disabled>已評價</button>`;
                    } else {
                        // 沒有顯示評價按鈕
                        return `<button id="btnscore" type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#MBReviews" onclick="addMrid(${data})">評價</button>`;
                    }

                   
                   /* return `<button id="btnscore" type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#MBReviews" onclick="addMrid(${data})">評價</button>`;*/
                }
            }
        ],  
        fixedHeader: {
            header: true
        },
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
        },
        order: [[2, 'asc']],
    
    });
}

QueryMedicalRecords();

