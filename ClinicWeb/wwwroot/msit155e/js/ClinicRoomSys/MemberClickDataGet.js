//獲得主要病歷表資料
async function getCase(id) {
    const response = await fetch(`/ClinicRoomSys/Cases/GM/${id}`, { method: "POST" })
    const data = await response.json();
    console.log(data);
    CASE_ID = data.casesID;
    console.log(CASE_ID);
    const record = getRecord(CASE_ID);
    console.log(record);
    $('#recordDataTable').DataTable({
        columns: [
            { title: "看診紀錄ID", data: "recordID", visible: false },
            { title: "日期", data: "date"},
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
    // $('#recordDataTable').DataTable().clear();
    // $('#recordDataTable').DataTable().rows.add(record).draw();
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
    $('#prescriptionDataTable').DataTable({
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
    const report = getReport(CASE_ID);
    console.log(report);
    const prescription = getPrescription(CASE_ID);
    console.log(prescription);
}

//獲得看診紀錄資料
async function getRecord(id) {
    const response = await fetch(`/ClinicRoomSys/Cases/GRD/${id}`, { method: "POST" })
    const data = await response.json();
    // return data;
    $('#recordDataTable').DataTable().rows.add(data).draw();
}
//獲得檢查報告資料
async function getReport(id) {
    const response = await fetch(`/ClinicRoomSys/Cases/GRT/${id}`, { method: "POST" })
    const data = await response.json();
    //return data;
    $('#reportDataTable').DataTable().rows.add(data).draw();
}
//獲得處方資料
async function getPrescription(id) {
    const response = await fetch(`/ClinicRoomSys/Cases/GP/${id}`, { method: "POST" })
    const data = await response.json();
    //return data;
    $('#prescriptionDataTable').DataTable().rows.add(data).draw();
}