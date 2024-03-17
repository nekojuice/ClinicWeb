//獲得主要病歷表資料
var CASE_ID;
(async () => {
    const response = await fetch(`/MBRecordInfo/Get_Memberdata`, { method: "GET" })
    const data = await response.json();
    CASE_ID = data.caseId;
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



// 分支:DrugsDetails_20240312
var currentDrugId;
var PLDT;

// DataTable 設定加上 hover:true跟className，並把藥品明細查詢欄位移除
async function getPrescriptionList(id) {
    const response = await fetch(`/MBRecordInfo/GPL/${id}`, { method: "POST" })
    const data = await response.json();
    //return data;
   PLDT =$('#prescriptionListDataTable').DataTable({
        "bDestroy": true,
        columns: [
            { title: "藥品ID", data: "drugId", visible: false },
            { title: "藥品名稱", data: "name" },
            { title: "開立天數", data: "days" },
            { title: "總量", data: "total" }
            //{title: "藥品明細查詢", data:null, // 這邊是欄位
            //render: function (data, type, row) {
            //    return `<button id="detail" type="button" class="btn btn-primary btn-sm style = "padding-left:10px"">明細</button> `
            //    }
            //},
        ],
        fixedHeader: {
            header: true
        },
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
        },
        hover: true,
        columnDefs: [{
            "targets": 1,
            "className": 'DrugName'
        }]
    });
    PLDT.rows.add(data).draw();
}

    //PLDT.on('click', 'button', function (e) {
    //    let data = PLDT.row(e.target.closest('tr')).data();
    //    console.log(data);
    //    let id = data['drugId'];
    //    alert('You clicked on :' + id);


    //    $.ajax({
    //        type: 'GET',
    //        url: `/MBRecordInfo/GP/${id}`,
    //        //data: { id: mlaId },
    //        //cache: false,
    //        success: function (result) {

    //        }
    //    });
    //});

    // 分支:DrugsDetails_20240312
    
    $('#prescriptionListDataTable tbody').on('mouseover', 'td.DrugName', function (event) {
        
        //測試Hover功能
        //alert('TEST');
        // 使用 DataTable API 取得被隱藏的 藥品ID
        var cell = PLDT.cell($(this).closest('td'));
        if (cell.node() !== null) {
            var rowIndex = cell.index().row;
            var rowData = PLDT.row(rowIndex).data();
            currentDrugId = rowData.drugId;  //(要跟columns的data名稱相同)

            console.log('catch');
            console.log('currentDrugId:',currentDrugId);
        }
        //設定CSS樣式，mouseleave時取消樣式
        $(this).css({
            'background-color': '#E6CAFF',
            'cursor': ' pointer'
        });
        loadDetails();
        //loadClinicalUseDetails();

        //小框框infoBox位子，固定從網頁邊界出現 right,topy在 MBRecordInfo/index的 style中
        var infoBox = document.getElementById("infoBox");
        
        console.log(event.pageX);
        console.log(event.pageY);
        
        infoBox.style.display = "block"
    });
    
    $("#prescriptionListDataTable tbody").on('mouseleave', 'td.DrugName', function () {
        $(this).css({
            'background-color': '', // 移除背景颜色
            'cursor': '' // 恢復默認滑鼠樣式
        });
        infoBox.style.display = "none"; // 隱藏小框框
    });

    $("#prescriptionListDataTable tbody").on('click', 'td.DrugName', function () {
        //測試 click
        //alert('click');

        // Ajax換頁 帶出 DrugDetails.cshtml
        var cell = PLDT.cell($(this).closest('td'));
        if (cell.node() !== null) {
            var rowIndex = cell.index().row;
            var rowData = PLDT.row(rowIndex).data();
            currentDrugId = rowData.drugId;  //(要跟columns的data名稱相同)
            
            console.log('currentDrugId:', currentDrugId);
        }
        $.ajax({
            url: `/MBRecordInfo/GetData`,
            type: 'GET',
            data: { drugId: currentDrugId },
            success: function (response) {
                console.log(response);
                window.location.href = '/MBRecordInfo/GetPage?drugId=' + currentDrugId;

            },
            error: function (error) {
                console.error('Ajax request failed:', error);
            }
        })

    })
//var infoBox = document.getElementById("infoBox"); 

//小框框 infoBox 的內容

const loadDetails = async () => {
    console.log('loadDetails function called');     //檢查
    console.log('currentDrugId', currentDrugId);   //確認 currentDrugId 是否正確

    //POST無法回傳currentDrugId給後端API

    //GET測試可以-------

    //------------藥品明細(含劑型)------------------
    const urlDrugs = `/MBRecordInfo/DrugsDetails?drugId=${currentDrugId}`
    const response = await fetch(urlDrugs, {
        method: 'GET'       
    });

    if (!response.ok) {
        const errorMessage = await response.text();
        console.error(`Error: ${response.status} - ${errorMessage}`);
        return;
    }

    const datas = await response.json();
    console.log(datas) 

    //------------適應症明細-------------
    
    const urlCU = `/MBRecordInfo/ClinicalUseDetails?drugId=${currentDrugId}`
    const CUresponse = await fetch(urlCU, {
        method: 'GET'        
    });
   
    if (!CUresponse.ok) {
        const errorCUMessage = await CUresponse.text();
        console.error(`Error: ${CUresponse.status} - ${errorCUMessage}`);
        return;
    }

    const CUdatas = await CUresponse.json();

    //------------副作用明細-------------

    const urlSE = `/MBRecordInfo/SideEffectDetails?drugId=${currentDrugId}`
    const SEresponse = await fetch(urlSE, {
        method: 'GET'
    });
    if (!SEresponse.ok) {
        const errorSEMessage = await SEresponse.text();
        console.error(`Error:${SEresponse.status} - ${errorSEMessage}`);
        return;
    }

    const SEdatas = await SEresponse.json();

    //-------display呈現-----------

    const display = datas.map(drug => {
        return (
            `<ul>
                    <li>藥品代碼: ${drug.drugCode}</li>
                    <li>學名：${drug.genericName}</li>
                    <li>商品名: ${drug.tradeName}</li>
                    <li>中文名: ${drug.drugName}</li>
                    <li>懷孕分類: ${drug.pregnancyCategory}</li>
                    <li>藥品劑型: ${drug.type}</li>
                                                                 </ul>`
        );
    }).join('');

    const CUdisplay = CUdatas.map(details => {
        return (`<li>${details.適應症}</li>`);
    }).join('');
    const display2 = `<ul>適應症:${CUdisplay}</ul>`;

    const SEdisplay = SEdatas.map(details => {
        return (`<li>${details.副作用}</li>`);
    }).join('');
    const display3 = `<ul>副作用:${SEdisplay}</ul>`;

    infoBox.innerHTML = display + display2 + display3;
}



    async function getName() {
        const response = await fetch(`/MBRecordInfo/Get_MemberName`, { method: "GET" })
        const data = await response.json();
    }
