//獲得主要病歷表資料
var CASE_ID;
(async () => {
    const response = await fetch(`/MBRecordInfo/Get_Memberdata`, { method: "GET" })
    const data = await response.json();
    const nameresponse = await fetch(`/MBRecordInfo/Get_MemberName`, { method: "GET" })
    const namedata = await nameresponse.json();
    CASE_ID = data.caseId;
    console.log(namedata);
    getRecord(CASE_ID);
    getReport(CASE_ID);
    getPrescription(CASE_ID);
})();
async function getCase(id) {
    const response = await fetch(`/ClinicRoomSys/Cases/GM/${id}`, { method: "POST" })
    const data = await response.json();
    console.log(data);
    CASE_ID = data.casesID;
    console.log(CASE_ID);
    const record = getRecord(CASE_ID);
    console.log(record);
    //$('#recordDataTable').DataTable({
    //    columns: [
    //        { title: "看診紀錄ID", data: "recordID", visible: false },
    //        { title: "血壓", data: "bloodPresure" },
    //        { title: "脈搏", data: "pulse" },
    //        { title: "體溫", data: "bodyTemparture" },
    //        { title: "主述", data: "chiefComplaint" },
    //        { title: "醫囑", data: "disposal" },
    //        { title: "處方", data: "prescribe" },
    //    ],
    //    fixedHeader: {
    //        header: true
    //    },
    //    language: {
    //        url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
    //    }
    //});
    // $('#recordDataTable').DataTable().clear();
    // $('#recordDataTable').DataTable().rows.add(record).draw();
  
    //$('#prescriptionDataTable').DataTable({
    //    columns: [
    //        { title: "處方ID", data: "prescriptionID", visible: false },
    //        { title: "處方日期", data: "prescriptionDate" },
    //        { title: "調劑方式", data: "dispensing" },
    //    ],
    //    fixedHeader: {
    //        header: true
    //    },
    //    language: {
    //        url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
    //    }
    //});
    const report = getReport(CASE_ID);
    console.log(report);
    const prescription = getPrescription(CASE_ID);
    console.log(prescription);
}

//獲得看診紀錄資料
async function getRecord(id) {
    const response = await fetch(`/MBRecordInfo/GRD/${id}`, { method: "POST" })
    const data = await response.json();
    // return data;
    $('#recordDataTable').DataTable({
        columns: [
            { title: "看診紀錄ID", data: "recordID", visible: false },
            { title: "血壓", data: "bloodPresure" },
            { title: "脈搏", data: "pulse" },
            { title: "體溫", data: "bodyTemparture" },
            { title: "主述", data: "chiefComplaint" },
            { title: "醫囑", data: "disposal" },
            { title: "處方", data: "prescribe" },
        ],
        fixedHeader: {
            header: true
        },
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
        }
    });
    $('#recordDataTable').DataTable().rows.add(data).draw();
}
//獲得檢查報告資料
async function getReport(id) {
    const response = await fetch(`/MBRecordInfo/GRT/${id}`, { method: "POST" })
    const data = await response.json();
    //return data;
    $('#reportDataTable').DataTable({
        columns: [
            { title: "報告ID", data: "reportID", visible: false },
            { title: "檢查名稱", data: "testName" },
            { title: "檢查日期", data: "testDate" },
            { title: "報告日期", data: "reportDate" },
            { title: "結果", data: "result" },
        ],
        fixedHeader: {
            header: true
        },
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
        }
    });
    $('#reportDataTable').DataTable().rows.add(data).draw();
}
var ptable;

//獲得處方資料
async function getPrescription(id) {
    const response = await fetch(`/MBRecordInfo/GP/${id}`, { method: "POST" })
    const data = await response.json();
    //return data;
    ptable = $('#prescriptionDataTable').DataTable({
        columns: [
            { title: "處方ID", data: "prescriptionID", visible: false },
            { title: "處方日期", data: "prescriptionDate" },
            { title: "調劑方式", data: "dispensing" },
        ],
        fixedHeader: {
            header: true
        },
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
        }
    });
    ptable.rows.add(data).draw();
    $('#prescriptionDataTable tbody').on('click', 'tr', function () {
        var rowData = ptable.row(this).data();
        var ID = rowData["prescriptionID"];
        myModal.show();
        getPrescriptionList(ID);
        console.log(rowData["prescriptionID"]);
    });
}




async function getPrescriptionList(id) {
    const response = await fetch(`/MBRecordInfo/GPL/${id}`, { method: "POST" })
    const data = await response.json();
    //return data;
    var PLDT = $('#prescriptionListDataTable').DataTable({
        "bDestroy": true,
        columns: [
            { title: "藥品ID", data: "drugId", visible: false },
            { title: "藥品名稱", data: "name" },
            { title: "開立天數", data: "days" },
            { title: "總量", data: "total" },
            {
                title: "藥品明細查詢", data: null, // 這邊是欄位
                render: function (data, type, row) {
                    return `<button id="detail" type="button" class="btn btn-primary btn-sm style = "padding-left:10px"">明細</button> `
                }
            },
        ],
        fixedHeader: {
            header: true
        },
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
        },
        hover: true,
    });
    PLDT.rows.add(data).draw();
}

    PLDT.on('click', 'button', function (e) {
        let data = PLDT.row(e.target.closest('tr')).data();
        console.log(data);
        let id = data['drugId'];
        alert('You clicked on :' + id);


        //$.ajax({
        //    type: 'GET',
        //    url: `/MBRecordInfo/GP/${id}`,
        //    //data: { id: mlaId },
        //    //cache: false,
        //    success: function (result) {

        //    }
        //});
    });
    async function getName() {

    }