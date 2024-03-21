//獲得主要病歷表資料
async function getCase(id) {
    const response = await fetch(`/ClinicRoomSys/Cases/GM/${id}`, { method: "POST" })
    const data = await response.json();
    document.getElementById("inputName").value = data.name;
    document.getElementById("inputGender").value = data.gender;
    document.getElementById("inputBirthDate").value = data.birthDate;
    document.getElementById("inputBloodType").value = data.bloodType;
    document.getElementById("inputHeight").value = data.height;
    document.getElementById("inputWeight").value = data.weight;
    document.getElementById("inputPastHistory").value = data.pastHistory;
    document.getElementById("inputAllergyRecord").value = data.allergyRecord;
    console.log(data);
    CASE_ID = data.casesID;
    console.log(CASE_ID);
    const record = getRecord(CASE_ID);
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
    const prescription = getPrescription(CASE_ID);
}

//獲得看診紀錄資料
async function getRecord(id) {
    const response = await fetch(`/ClinicRoomSys/Cases/GRD/${id}`, { method: "POST" })
    const data = await response.json();
    console.log(data);
    // return data;
    $('#recordDataTable').DataTable().rows.add(data).draw();
}
//獲得檢查報告資料
async function getReport(id) {
    const response = await fetch(`/ClinicRoomSys/Cases/GRT/${id}`, { method: "POST" })
    const data = await response.json();
    console.log(data);
    //return data;
    $('#reportDataTable').DataTable().rows.add(data).draw();
}
//獲得處方資料
async function getPrescription(id) {
    const response = await fetch(`/ClinicRoomSys/Cases/GP/${id}`, { method: "POST" })
    const data = await response.json();
    console.log(data);
    //return data;
    $('#prescriptionDataTable').DataTable().rows.add(data).draw();
}

//async function uploadFormData(formData) {
//    try {
//        const response = await fetch('/upload', {
//            method: 'PUT',
//            body: formData
//        });
//        if (response.ok) {
//            console.log('上传成功');
//        } else {
//            console.error('上传失败');
//        }
//    } catch (error) {
//        console.error('上传出错', error);
//    }
//}

//async function getCase(id) {
//    // 读取表单数据
//    const form = document.getElementById('MainForm');
//    const formData = new FormData(form);
    

//    // 上传表单数据
//    await uploadFormData(formData);
//}
//document.getElementById("submitButton").addEventListener("click", function (event) {
//    event.preventDefault(); // 防止表單提交
//    getCase(CASE_ID);
//});
