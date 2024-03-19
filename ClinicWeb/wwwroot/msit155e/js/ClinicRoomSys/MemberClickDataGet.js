﻿//datatable初始化
$('#recordDataTable').DataTable({
    columns: [
        { title: "看診紀錄ID", data: "recordID", visible: false },
        { title: "日期", data: "date" },
        { title: "血壓", data: "bloodPresure" },
        { title: "脈搏", data: "pulse" },
        { title: "體溫", data: "bodyTemparture" },
        { title: "主述", data: "chiefComplaint" },
        { title: "醫囑", data: "disposal" },
        { title: "處方", data: "prescribe" },
        {
            title: "功能", data: null, render: function (data, type, row) { return '<button id="Regist" type="button" class="btn btn-warning btn-sm"><i class="bi bi-pencil-square"></i></button> ' + '<button id="Delete" type="button" class="btn btn-danger btn-sm"><i class="bi bi-trash"></i></button>' },
        }
    ],
    fixedHeader: {
        header: true
    },
    language: {
        url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
    }
});

$('#reportDataTable').DataTable({
    columns: [
        { title: "報告ID", data: "reportID", visible: false },
        { title: "檢查名稱", data: "testName" },
        { title: "檢查日期", data: "testDate" },
        { title: "報告日期", data: "reportDate" },
        { title: "結果", data: "result" },
        {
            data: null, title: "功能",  // 這邊是欄位
            render: function (data, type, row) {
                return '<button id="Regist" type="button" class="btn btn-warning btn-sm"><i class="bi bi-pencil-square"></i></button> ' +
                    '<button id="Delete" type="button" class="btn btn-danger btn-sm"><i class="bi bi-trash"></i></button>'
            }
        },
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
        {
            data: null, title: "功能",  // 這邊是欄位
            render: function (data, type, row) {
                return '<button id="Regist" type="button" class="btn btn-warning btn-sm"><i class="bi bi-pencil-square"></i></button> ' +
                    '<button id="Delete" type="button" class="btn btn-danger btn-sm"><i class="bi bi-trash"></i></button>'
                }
            },
    ],
    fixedHeader: {
        header: true
    },
    language: {
        url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
    }
});

$('#prescriptionListDataTable').DataTable({
    columns: [
        { title: "處方ID", data: "prescriptionID", visible: false },
        { title: "藥品", data: "DrugID" },
        { title: "開立天數", data: "days" },
        { title: "總量", data: "Total" },
        {
            data: null, title: "功能",  // 這邊是欄位
            render: function (data, type, row) {
                return '<button id="Regist" type="button" class="btn btn-warning btn-sm"><i class="bi bi-pencil-square"></i></button> ' +
                    '<button id="Delete" type="button" class="btn btn-danger btn-sm"><i class="bi bi-trash"></i></button>'
            }
        },
    ],
    fixedHeader: {
        header: true
    },
    language: {
        url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
    }
});
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
    getRecord(CASE_ID);
    getReport(CASE_ID);
    getPrescription(CASE_ID);
}

//獲得看診紀錄資料
async function getRecord(id) {
    const response = await fetch(`/ClinicRoomSys/Cases/GRD/${id}`, { method: "POST" })
    const data = await response.json();
    
    $('#recordDataTable').DataTable().clear();
    $('#recordDataTable').DataTable().rows.add(data).draw();
}
//獲得檢查報告資料
async function getReport(id) {
    const response = await fetch(`/ClinicRoomSys/Cases/GRT/${id}`, { method: "POST" })
    const data = await response.json();
    console.log(data);
    //return data;
    $('#reportDataTable').DataTable().clear();
    $('#reportDataTable').DataTable().rows.add(data).draw();
}
//獲得處方資料
async function getPrescription(id) {
    const response = await fetch(`/ClinicRoomSys/Cases/GP/${id}`, { method: "POST" })
    const data = await response.json();
    console.log(data);
    //return data;
    $('#prescriptionDataTable').DataTable().clear();
    $('#prescriptionDataTable').DataTable().rows.add(data).draw();
}


//修改主病例資料
async function uploadFormData(id) {
    const record = {
        Height: $('#inputHeight').val(),
        Weight: $('#inputWeight').val(),
        PastHistory: $('#inputPastHistory').val(),
        AllergyRecord: $('#inputAllergyRecord').val(),
    };

    try {
        const response = await fetch(`/ClinicRoomSys/Cases/UpdateCase/${id}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(record),
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data = await response.json();
        console.log('Success:', data);
        // 這裡可以添加一些成功後的操作，比如更新UI或者是頁面導覽
        new PNotify({
            title: '成功',
            text: '病歷表修改成功',
            type: 'info',
            styling: 'bootstrap3',
            setTimeout: 500
        })
    } catch (error) {
        console.error('Error:', error);
        // 這裡可以處理錯誤，比如提示用户操作失敗
    }
}

//新增看診紀錄
async function AddNMR() {
    const addrecord = {
        CaseId: CASE_ID,
        ClinicListId: CLINICLIST_ID,
        Bp: $('#addbp').val(),
        Pulse: $('#addpulse').val(),
        Bt: $('#addbt').val(),
        Cc: $('#addcc').val(),
        Disposal: $('#adddisposal').val(),
        Prescribe: $('#addprescribe').val(),
        };

    try {
        const response = await fetch(`/ClinicRoomSys/Cases/AddMedicalRecord`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(addrecord),
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        //const data = await response.json();
        //console.log('Success:', data);
        // 這裡可以添加一些成功後的操作，比如更新UI或者是頁面導覽
        new PNotify({
            title: '成功',
            text: '新增成功',
            type: 'info',
            styling: 'bootstrap3',
            setTimeout: 500
        })
            (#recordModal).modal(hide);

    } catch (error) {
        console.error('Error:', error);
        // 這裡可以處理錯誤，比如提示用户操作失敗
    }
}

//新增處方籤
async function AddNPre() {
    const addrecord = {
        CaseId: CASE_ID,
        ClinicListId: CLINICLIST_ID,
        Bp: $('#addbp').val(),
        Pulse: $('#addpulse').val(),
        Bt: $('#addbt').val(),
        Cc: $('#addcc').val(),
        Disposal: $('#adddisposal').val(),
        Prescribe: $('#addprescribe').val(),
    };

    try {
        const response = await fetch(`/ClinicRoomSys/Cases/AddMedicalRecord`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(addrecord),
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data = await response.json();
        console.log('Success:', data);
        // 這裡可以添加一些成功後的操作，比如更新UI或者是頁面導覽
        new PNotify({
            title: '成功',
            text: '新增成功',
            type: 'info',
            styling: 'bootstrap3',
            setTimeout: 500
        })
    } catch (error) {
        console.error('Error:', error);
        // 這裡可以處理錯誤，比如提示用户操作失敗
    }
}

//新增藥品
async function AddNPreL() {
    const addrecord = {
        CaseId: CASE_ID,
        ClinicListId: CLINICLIST_ID,
        Bp: $('#addbp').val(),
        Pulse: $('#addpulse').val(),
        Bt: $('#addbt').val(),
        Cc: $('#addcc').val(),
        Disposal: $('#adddisposal').val(),
        Prescribe: $('#addprescribe').val(),
    };

    try {
        const response = await fetch(`/ClinicRoomSys/Cases/AddMedicalRecord`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(addrecord),
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data = await response.json();
        console.log('Success:', data);
        // 這裡可以添加一些成功後的操作，比如更新UI或者是頁面導覽
        new PNotify({
            title: '成功',
            text: '新增成功',
            type: 'info',
            styling: 'bootstrap3',
            setTimeout: 500
        })
    } catch (error) {
        console.error('Error:', error);
        // 這裡可以處理錯誤，比如提示用户操作失敗
    }
}


//修改主病例資料-事件
document.getElementById("submit").addEventListener("click", function (event) {
    event.preventDefault(); // 防止表單提交
    uploadFormData(CASE_ID);
});

//新增看診紀錄-事件
document.getElementById("submitrd").addEventListener("click", function (event) {
    event.preventDefault(); // 防止表單提交
    AddNMR();
});

//新增處方籤-主要
document.getElementById("submit").addEventListener("click", function (event) {
    event.preventDefault(); // 防止表單提交
    uploadFormData(CASE_ID);
});
