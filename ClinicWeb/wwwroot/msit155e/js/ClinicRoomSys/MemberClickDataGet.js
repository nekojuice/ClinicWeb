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
    },
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
    },
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
    },
});

$('#prescriptionListDataTable').DataTable({
    searching: false,
    paging: false,
    info: false,
    columns: [
        { title: "處方ID", data: "drugId", visible: false },
        { title: "藥品", data: "name" },
        { title: "開立天數", data: "days" },
        { title: "總量", data: "totalAmount" },
        {
            data: null, title: "功能",  // 這邊是欄位
            render: function (data, type, row) {
                return '<button id="Delete" type="button" class="btn btn-danger btn-sm"><i class="bi bi-trash"></i></button>'
            }
        },
    ],
    fixedHeader: {
        header: true
    },
    language: {
        url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
    },
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
    /*console.log(data);*/
    CASE_ID = data.casesID;
    getRecord(CASE_ID);
    getReport(CASE_ID);
    getPrescription(CASE_ID);
}

//獲得看診紀錄資料
async function getRecord(id) {
    const response = await fetch(`/ClinicRoomSys/Cases/GRD/${id}`, { method: "POST" })
    const data = await response.json();
    /*console.log(data);*/
    $('#recordDataTable').DataTable().clear();
    $('#recordDataTable').DataTable().rows.add(data).draw();
}
//獲得檢查報告資料
async function getReport(id) {
    const response = await fetch(`/ClinicRoomSys/Cases/GRT/${id}`, { method: "POST" })
    const data = await response.json();
    /*console.log(data);*/
    $('#reportDataTable').DataTable().clear();
    $('#reportDataTable').DataTable().rows.add(data).draw();
}
//獲得處方資料
async function getPrescription(id) {
    const response = await fetch(`/ClinicRoomSys/Cases/GP/${id}`, { method: "POST" })
    const data = await response.json();
    /*console.log(data);*/
    //return data;
    $('#prescriptionDataTable').DataTable().clear();
    $('#prescriptionDataTable').DataTable().rows.add(data).draw();
}
//獲得處方藥單資料
async function getPrescriptionL(id) {
    const response = await fetch(`/ClinicRoomSys/Cases/GPL/${id}`, { method: "POST" })
    const data = await response.json();
    console.log(data);
    $('#prescriptionListDataTable').DataTable().clear();
    $('#prescriptionListDataTable').DataTable().rows.add(data).draw();
    //return data;
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

//獲得欲修改紀錄表單
async function getRecordForm(id) {
    const response = await fetch(`/ClinicRoomSys/Cases/GURD/${id}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        }
    });
    const data = await response.json();
    console.log(data);
    $('#addbp').val(data.bloodPresure);
    $('#addpulse').val(data.pulse);
    $('#addbt').val(data.bodyTemparture);
    $('#addcc').val(data.chiefComplaint);
    $('#adddisposal').val(data.disposal);
    $('#addprescribe').val(data.prescribe);
    $('#recordModal').modal('show');
    $('#submitrd').hide();
    $('#updaterd').show();
}

//修改看診紀錄資料
async function uploadRecordForm(id) {
    const record = {
        Bp: $('#addbp').val(),
        Pulse: $('#addpulse').val(),
        Bt: $('#addbt').val(),
        Cc: $('#addcc').val(),
        Disposal: $('#adddisposal').val(),
        Prescribe: $('#addprescribe').val(),
    };

    try {
        const response = await fetch(`/ClinicRoomSys/Cases/URD/${id}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(record),
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        //const data = await response.json();
        //console.log('Success:', data);
        // 這裡可以添加一些成功後的操作，比如更新UI或者是頁面導覽
        new PNotify({
            title: '成功',
            text: '記錄修改成功',
            type: 'info',
            styling: 'bootstrap3',
            setTimeout: 500
        })
        return response;
        $('#recordModal').modal('hide');
    } catch (error) {
        console.error('Error:', error);
        // 這裡可以處理錯誤，比如提示用户操作失敗
    }
}

//獲得欲修改報告表單
async function getReportForm(id) {
    const response = await fetch(`/ClinicRoomSys/Cases/GURT/${id}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        }
    });
    const data = await response.json();
    console.log(data);
    $('#addTestName').val(data.testName);
    $('#addTestDate').val(data.testDate);
    $('#addReportDate').val(data.reportDate);
    $('#addTestResult').val(data.result);
    $('#TDLabel').show();
    $('#addReportDate').show();
    $('#TRLabel').show();
    $('#addTestResult').show();
    $('#updatert').show();
    $('#submitrt').hide();
    $('#reportModal').modal('show');
}

//修改檢查報告資料
async function uploadReportForm(id) {
    const record = {
        TestName: $('#addTestName').val(),
        TestDate: $('#addTestDate').val(),
        ReportDate: $('#addReportDate').val(),
        Result: $('#addTestResult').val(),
    };

    try {
        const response = await fetch(`/ClinicRoomSys/Cases/URT/${id}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(record),
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        //const data = await response.json();
        //console.log('Success:', data);
        // 這裡可以添加一些成功後的操作，比如更新UI或者是頁面導覽
        new PNotify({
            title: '成功',
            text: '報告修改成功',
            type: 'info',
            styling: 'bootstrap3',
            setTimeout: 500
        })
        $('#reportModal').modal('hide');
        return response;
    } catch (error) {
        console.error('Error:', error);
        // 這裡可以處理錯誤，比如提示用户操作失敗
    }
}

//獲得欲修改處方表單
async function getPrescriptionForm(id) {
    const response = await fetch(`/ClinicRoomSys/Cases/GUP/${id}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        }
    });
    const data = await response.json();
    console.log(data);
    $('#adddispensing').val(data.dispensing);
}

//修改處方資料
async function uploadPrescriptionForm(id) {
    const record = {
        Dispensing: $('#adddispensing').val(),
    };

    try {
        const response = await fetch(`/ClinicRoomSys/Cases/UP/${id}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(record),
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        //const data = await response.json();
        //console.log('Success:', data);
        // 這裡可以添加一些成功後的操作，比如更新UI或者是頁面導覽
        new PNotify({
            title: '成功',
            text: '處方籤修改成功',
            type: 'info',
            styling: 'bootstrap3',
            setTimeout: 500
        })
        return response;
    } catch (error) {
        console.error('Error:', error);
        // 這裡可以處理錯誤，比如提示用户操作失敗
    }
}

//日期設定
const datenow = new Date().toISOString().split('T')[0];
let PID;

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
            text: '新增紀錄成功',
            type: 'info',
            styling: 'bootstrap3',
            setTimeout: 500
        })
        $('#recordModal').modal('hide');
        return response;

    } catch (error) {
        console.error('Error:', error);
        // 這裡可以處理錯誤，比如提示用户操作失敗
    }
}

//新增檢查報告
async function AddNTR() {

    const addreport = {
        CaseId: CASE_ID,
        TestName: $('#addTestName').val(),
        TestDate: $('#addTestDate').val()
    };

    try {
        const response = await fetch(`/ClinicRoomSys/Cases/AddTestReport`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(addreport),
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        //const data = await response.json();
        //console.log('Success:', data);
        // 這裡可以添加一些成功後的操作，比如更新UI或者是頁面導覽
        new PNotify({
            title: '成功',
            text: '新增報告成功',
            type: 'info',
            styling: 'bootstrap3',
            setTimeout: 500
        })
        $('#reportModal').modal('hide');
        return response;

    } catch (error) {
        console.error('Error:', error);
        // 這裡可以處理錯誤，比如提示用户操作失敗
    }
}


//新增處方籤
async function AddNPre() {
    const addrecord = {
        CaseId: CASE_ID,
        PrescriptionDate: datenow,
    };

    try {
        const response = await fetch(`/ClinicRoomSys/Cases/AddPrescription`, {
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
            text: '新增處方籤成功',
            type: 'info',
            styling: 'bootstrap3',
            setTimeout: 300
        })
        PID = data;
        console.log(PID);
    } catch (error) {
        console.error('Error:', error);
        // 這裡可以處理錯誤，比如提示用户操作失敗
    }
}

//新增藥品
async function AddNPreL() {
    const addrecord = {
        PrescriptionId: PID,
        DrugID: $('#medicine').val(),
        Days: $('#adddays').val(),
        TotalAmount: $('#addtotal').val(),
    };
    console.log(addrecord);

    try {
        const response = await fetch(`/ClinicRoomSys/Cases/AddPrescriptionL`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(addrecord),
        });

        if (!response.ok) {
            new PNotify({
                title: '失敗',
                text: '處方籤藥品重複添加',
                type: 'error',
                styling: 'bootstrap3',
                setTimeout: 500
            })
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        //const data = await response.json();
        //console.log('Success:', data);
        // 這裡可以添加一些成功後的操作，比如更新UI或者是頁面導覽
        new PNotify({
            title: '成功',
            text: '新增藥品成功',
            type: 'info',
            styling: 'bootstrap3',
            setTimeout: 500
        })
    } catch (error) {
        console.error('Error:', error);
        // 這裡可以處理錯誤，比如提示用户操作失敗
    }
}

//藥品清單增加
async function AddDL() {
    try {
        // 使用fetch API调用后端方法
        const response = await fetch(`/ClinicRoomSys/Cases/GetDrugList`);
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        const drugs = await response.json();
        console.log(drugs);

        // 获取<select>元素
        const selectElement = document.getElementById('medicine');

        // 清空<select>以防有预先填充的<option>
        selectElement.innerHTML = '';

        // 为每个药品创建<option>元素并添加到<select>
        drugs.forEach(drug => {
            const option = new Option(drug.name, drug.drugId);
            selectElement.add(option);
        });
    } catch (error) {
        console.error('Failed to fetch drugs:', error);
    }
}

//修改主病例資料-事件
document.getElementById("submit").addEventListener("click", function (event) {
    event.preventDefault(); // 防止表單提交
    uploadFormData(CASE_ID);

});

//新增看診紀錄-事件
document.getElementById("submitrd").addEventListener("click", async function (event) {
    event.preventDefault(); // 防止表單提交
    const response = await AddNMR();
    if (response.ok) {
        getRecord(CASE_ID);
    }
});

//新增檢查報告-事件
document.getElementById("submitrt").addEventListener("click",async function (event) {
    event.preventDefault(); // 防止表單提交
    const response =await AddNTR();
    if (response.ok) {
        getReport(CASE_ID);
    }
});

//新增處方籤-藥單事件
document.getElementById("addPL").addEventListener("click",async function (event) {
    await AddNPreL();
    await getPrescriptionL(PID);


});

//初始化紀錄表單
document.getElementById("addrecord").addEventListener("click", function (event) {
    $('#submitrt').show();
    $('#addbp').val('');
    $('#addpulse').val('');
    $('#addbt').val('');
    $('#addcc').val('');
    $('#adddisposal').val('');
    $('#addprescribe').val('');
    //$('#recordModal').modal('show');
    $('#submitrd').show();
    $('#updaterd').hide();
   
});

//初始化報告表單
document.getElementById("addreport").addEventListener("click", function (event) {
    $('#TDLabel').hide();
    $('#addReportDate').hide();
    $('#TRLabel').hide();
    $('#addTestResult').hide();
    $('#updatert').hide();
    $('#submitrt').show();
    $('#addTestName').val('');
    $('#addTestDate').val(datenow);
    //$('#recordModal').modal('show');

});

//初始化處方表單,創建處方籤主表單
document.getElementById("addpre").addEventListener("click", function (event) {
    $('#addDispensing').val('');
    AddDL();
    AddNPre();
    //$('#recordModal').modal('show');
});

//看診紀錄資料表按鍵事件
let Rid;
$('#recordDataTable').on('click', '#Delete', function (e) {
    let data = $('#recordDataTable').DataTable().row(e.target.closest('tr')).data();
    console.log(data);
    let id = data['recordID'];
/*    alert('You clicked delete on :' + id);*/

    Swal.fire({
        title: "確定要刪除嗎?",
        showDenyButton: true,
        showCancelButton: false,
        confirmButtonText: "確定",
        denyButtonText: `取消`,
    }).then(async (result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {
            try {
                const response =await fetch(`/ClinicRoomSys/Cases/DMedicalRecord/${id}`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
/*                    body: JSON.stringify(addreport),*/
                });
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                Swal.fire("已刪除", "", "success");
                getRecord(CASE_ID);
                //const data = await response.json();
                //console.log('Success:', data);
                // 這裡可以添加一些成功後的操作，比如更新UI或者是頁面導覽
            } catch (error) {
                console.error('Error:', error);
                // 這裡可以處理錯誤，比如提示用户操作失敗
            }
        } else if (result.isDenied) {
            Swal.fire("取消操作", "", "info");
        }
    });

});

$('#recordDataTable').on('click', '#Regist',async function (e) {
    let data = $('#recordDataTable').DataTable().row(e.target.closest('tr')).data();
    console.log(data);
    Rid = data['recordID'];
    console.log(Rid);
    /*alert('You clicked regist on :' + id);*/
    await getRecordForm(Rid);

    //$.ajax({
    //    type: 'GET',
    //    url: `/MBRecordInfo/GP/${id}`,
    //    //data: { id: mlaId },
    //    //cache: false,
    //    success: function (result) {

    //    }
    //});
});

document.getElementById("updaterd").addEventListener("click", async function (event) {
    event.preventDefault(); // 防止表單提交
    const response = await uploadRecordForm(Rid);
    if (response.ok) {
        getRecord(CASE_ID);
    }
});

//檢查報告資料表按鍵事件
let Rtid;
$('#reportDataTable').on('click', '#Delete', function (e) {
    let data = $('#reportDataTable').DataTable().row(e.target.closest('tr')).data();
/*    console.log(data);*/
    let id = data['reportID'];
/*    alert('You clicked delete on :' + id);*/

    Swal.fire({
        title: "確定要刪除嗎?",
        showDenyButton: true,
        showCancelButton: false,
        confirmButtonText: "確定",
        denyButtonText: `取消`,
    }).then(async (result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {
            try {
                const response = await fetch(`/ClinicRoomSys/Cases/DTestReport/${id}`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    /*                    body: JSON.stringify(addreport),*/
                });
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                Swal.fire("已刪除", "", "success");
                getReport(CASE_ID);
                //const data = await response.json();
                //console.log('Success:', data);
                // 這裡可以添加一些成功後的操作，比如更新UI或者是頁面導覽
            } catch (error) {
                console.error('Error:', error);
                // 這裡可以處理錯誤，比如提示用户操作失敗
            }
        } else if (result.isDenied) {
            Swal.fire("取消操作", "", "info");
        }
    });
});

$('#reportDataTable').on('click', '#Regist',async function (e) {
    let data = $('#reportDataTable').DataTable().row(e.target.closest('tr')).data();
    /*console.log(data);*/
    Rtid = data['reportID'];
    /*alert('You clicked regist on :' + Rtid);*/
    await getReportForm(Rtid);
});

document.getElementById("updatert").addEventListener("click", async function (event) {
    event.preventDefault(); // 防止表單提交
    const response = await uploadReportForm(Rtid);
    if (response.ok) {
        getReport(CASE_ID);
    }
});


//處方資料表按鍵事件
$('#prescriptionDataTable').on('click', '#Delete',async function (e) {
    let data = $('#prescriptionDataTable').DataTable().row(e.target.closest('tr')).data();
    console.log(data);
    let id = data['prescriptionID'];
    /*    alert('You clicked delete on :' + id);*/

    Swal.fire({
        title: "確定要刪除嗎?",
        showDenyButton: true,
        showCancelButton: false,
        confirmButtonText: "確定",
        denyButtonText: `取消`,
    }).then(async (result) => {
        /*Read more about isConfirmed, isDenied below*/
        if (result.isConfirmed) {
            try {
                const responsel = await fetch(`/ClinicRoomSys/Cases/DPrescriptionL/${id}`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    /*body: JSON.stringify(addreport),*/
                });
                if (!responsel.ok) {
                    throw new Error(`HTTP error! status: ${responsel.status}`);
                }
                const response = await fetch(`/ClinicRoomSys/Cases/DPrescription/${id}`, {
                    method: 'POST',
                    headers: {
                     'Content-Type': 'application/json',
                    },
                });
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                Swal.fire("已刪除", "", "success");
                getPrescription(CASE_ID);
                //const data = await response.json();
                //console.log('Success:', data);
                // 這裡可以添加一些成功後的操作，比如更新UI或者是頁面導覽
            } catch (error) {
                console.error('Error:', error);
                // 這裡可以處理錯誤，比如提示用户操作失敗
            }
        } else if (result.isDenied) {
            Swal.fire("取消操作", "", "info");
        }
    });

});

$('#prescriptionDataTable').on('click', '#Regist', async function (e) {
    let data = $('#prescriptionDataTable').DataTable().row(e.target.closest('tr')).data();
    console.log(data);
    PID = data['prescriptionID'];
     
    /*alert('You clicked regist on :' + id);*/
    await AddDL();
    await getPrescriptionForm(PID)
    await getPrescriptionL(PID);
    $('#preModal').modal('show');
});

document.getElementById("updatepre").addEventListener("click", async function (event) {
    event.preventDefault(); // 防止表單提交
    const response = await uploadPrescriptionForm(PID);
    if (response.ok) {
        await getPrescription(CASE_ID);
    }
});

$('#prescriptionListDataTable').on('click', '#Delete',async function (e) {
    let data = $('#prescriptionListDataTable').DataTable().row(e.target.closest('tr')).data();
    /*    console.log(data);*/
    let id = data['drugId'];
    /*    alert('You clicked delete on :' + id);*/

    Swal.fire({
        title: "確定要刪除嗎?",
        showDenyButton: true,
        showCancelButton: false,
        confirmButtonText: "確定",
        denyButtonText: `取消`,
    }).then(async (result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {
            try {
                const data = {
                    Variable1: id,
                    Variable2: PID
                };
                const response = await fetch(`/ClinicRoomSys/Cases/DPrescriptionLItem/`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify(data),
                });
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                Swal.fire("已刪除", "", "success");
                getPrescriptionL(PID);
                //const data = await response.json();
                //console.log('Success:', data);
                // 這裡可以添加一些成功後的操作，比如更新UI或者是頁面導覽
            } catch (error) {
                console.error('Error:', error);
                // 這裡可以處理錯誤，比如提示用户操作失敗
            }
        } else if (result.isDenied) {
            Swal.fire("取消操作", "", "info");
        }
    });
});

//demo按鍵
document.getElementById("demord").addEventListener("click", function (event) {
   $('#addbp').val('120/80');
    $('#addpulse').val('80');
    $('#addbt').val('36.5');
    $('#addcc').val('頭痛');
    $('#adddisposal').val('休息');
    $('#addprescribe').val('開立處方籤');
});
