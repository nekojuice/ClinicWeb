function init_callingTable() {
    if (!$.fn.DataTable.isDataTable('#callingTable')) {
        $('#callingTable').dataTable({
            columns: [
                { "data": "member_id", "visible": false },
                { "data": "clinicListId", "visible": false },
                { "data": "status_id", "visible": false },
                { "data": "診號" },
                { "data": "姓名" },
                { "data": "性別" },
                //{ "data": "狀態" },
                {
                    "data": "狀態",
                    "render": function (data, type, row) {
                        let colorString = '';
                        switch (row.status_id) {
                            case 1: colorString = 'success'; break;
                            case 2:
                            case 3:
                            case 4: colorString = 'info'; break;
                            case 5: colorString = 'primary'; break;
                            case 6: colorString = 'warning'; break;
                            case 7: colorString = 'danger'; break;
                            case 8: colorString = 'secondary'; break;
                            default: colorString = 'secondary'; break;
                        }
                        return `<div class="dropdown"><button type="button" class="btn btn-${colorString} dropdown-toggle" data-toggle="dropdown" aria-expanded="false">${data}</button>` +
                            '<div class="dropdown-menu">' +
                            '<button type="button" class="btn btn-success dropdown-item" onclick="process_state(1)">看診中</button>' +
                            '<button type="button" class="btn btn-info dropdown-item" onclick="process_state(2)">覆診 已報到</button>' +
                            '<button type="button" class="btn btn-info dropdown-item" onclick="process_state(3)">已報到</button>' +
                            '<button type="button" class="btn btn-info dropdown-item" onclick="process_state(4)">過號 已報到</button>' +
                            '<button type="button" class="btn btn-primary dropdown-item" onclick="process_state(5)">離場 檢查中</button>' +
                            '<button type="button" class="btn btn-warning dropdown-item" onclick="process_state(6)">離場 叫號未到</button>' +
                            '<button type="button" class="btn btn-danger dropdown-item" onclick="process_state(7)">已結束</button>' +
                            '<button type="button" class="btn btn-secondary dropdown-item" onclick="process_state(8)">未報到</button>' +
                            '</div>' +
                            '</div>';
                    }
                }
            ],
            fixedHeader: {
                header: false
            },
            language: {
                url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
            },
            //排序目標
            order: [[2, 'asc'], [3, 'asc']], //優先度:狀態id > 診號
            //關閉排序和搜尋目標
            aoColumnDefs: [
                { "bSortable": false, "aTargets": [0, 1, 2, 3, 4, 5, 6] },
                { "bSearchable": false, "aTargets": [0, 1, 2, 3, 4, 5, 6] }
            ],
            //搜尋框
            searching: false,
            //分頁
            bPaginate: false,
            //筆數頁數資訊
            info: false
        });
    }
}
init_callingTable()

async function QueryCallingData() {
    //引入全域變數
    const doctorId = DOCTOR_ID
    const date = CLINIC_DATE

    const datedata = `${date.getFullYear()}/${(date.getMonth() + 1).toString().padStart(2, '0')}/${(date.getDate()).toString().padStart(2, '0')}`

    const shiftId = +$("#ClinicTime").val()

    const jsonData = { 'doctorId': doctorId, 'date': datedata, 'shiftId': shiftId }

    //先清空資料
    $("#callingTable").DataTable().clear();
    //要資料
    const response = await fetch(`/ClinicRoomSys/CallingSys/Get_CallingList`,
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                // Content-Type: 'application/x-www-form-urlencoded',
            },
            body: JSON.stringify(jsonData)
        })
    const data = await response.json()
    //填入資料
    $("#callingTable").DataTable().rows.add(data).draw()
}

$("#ClinicTime").on('change', function () {

    $("#ClinicTime option[value='0']").remove();

    QueryCallingData()
})



let _index_DataTable = -1
//左表 左鍵點擊事件
$("#callingTable tbody").on('mousedown', 'tr', function () {
    _index_DataTable = $('#callingTable').DataTable().row(this).index();
    if (_index_DataTable == null) { return; } //忽略無選擇時
    if ($(this).hasClass('selected')) { return; }   //忽略選擇同一row

    const dataObject = $('#callingTable').DataTable().row(_index_DataTable).data()
    MEMBER_ID = dataObject.member_id;
    CLINICLIST_ID = dataObject.clinicListId;

    $(this).siblings().removeClass('selected');
    $(this).addClass('selected');

    //點擊後觸發的事件
    //fetch右表
    on_memberClick()

    //下方console邏輯
    $("#call_currentNumber").html(dataObject.診號)
    $("#call_currentName").html(dataObject.姓名)

    if (dataObject.status_id == 8) {
        btn_disable()
    } else {
        btn_enable()
    }
    
});

//左表 右鍵點擊事件
//$("#callingTable tbody").on('contextmenu', 'tr', function (e) {
//    e.preventDefault();
//    _index_DataTable = $('#callingTable').DataTable().row(this).index();
//    if (_index_DataTable == null) { return; } //忽略無選擇時
//    const dataObject = $('#callingTable').DataTable().row(_index_DataTable).data()
//    MEMBER_ID = dataObject.member_id;
//    CLINICLIST_ID = dataObject.clinicListId;

//    $(this).siblings().removeClass('selected');
//    $(this).addClass('selected');

//    //下方console邏輯
//    $("#call_currentNumber").html(dataObject.診號)
//    $("#call_currentName").html(dataObject.姓名)

//    if (dataObject.status_id == 8) {
//        btn_disable()
//    } else {
//        btn_enable()
//    }
//});

//signalR
let connection = new signalR.HubConnectionBuilder()
    .withUrl("/CallingHub")
    .withAutomaticReconnect()
    .build();

//signalR連線初始化
connection.start()
    //.then(function () { })
    .catch(function (err) {
        return console.error(err.toString());
    });

//進入畫面時註冊
(async () => {
    //引入全域變數
    const response1 = await fetch(URL_Get_EmpId, { method: 'POST' });
    const doctor_ID = await response1.text()

    const date = CLINIC_DATE
    const datedata = `${date.getFullYear()}/${(date.getMonth() + 1).toString().padStart(2, '0')}/${(date.getDate()).toString().padStart(2, '0')}`

    const jsonData = { 'doctorId': doctor_ID, 'date': datedata }
    const response2 = await fetch(Get_EmpInfo,
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                // Content-Type: 'application/x-www-form-urlencoded',
            },
            body: JSON.stringify(jsonData)
        });
    const data = await response2.json();
    console.log(data);

    //更改當前診號
    $("#spanShowDept").text(data.department)
    $("#spanShowRoom").text(data.room)

    //註冊signalR
    connection.invoke("Set_ClinicInfo", DOCTOR_ID, data.department, data.room, data.doctorName)
        .catch(function (err) {
            return console.error(err.toString());
        });
})();

//選擇時段
$("#ClinicTime").on('change', function () {
    if ($("#ClinicTime option:selected").text() != '--選擇時段--') {
        connection.invoke("Set_Shift", $("#ClinicTime option:selected").text())
            .catch(function (err) {
                return console.error(err.toString());
            });
        //刪除選擇的人
        $("#call_currentNumber").text('')
        $("#call_currentName").text('')
        //刪除叫號機上號碼
        connection.invoke("Set_Number", $("#call_currentNumber").text())
            .catch(function (err) {
                return console.error(err.toString());
            });
    }
    btn_disable()
})

function btn_enable() {
    $("#btn_call").prop('disabled', false);
    $("#btn_start").prop('disabled', false);
    $("#btn_late").prop('disabled', false);
    $("#btn_exam").prop('disabled', false);
    $("#btn_fin").prop('disabled', false);
}
function btn_disable() {
    $("#btn_call").prop('disabled', true);
    $("#btn_start").prop('disabled', true);
    $("#btn_late").prop('disabled', true);
    $("#btn_exam").prop('disabled', true);
    $("#btn_fin").prop('disabled', true);
}
btn_disable()

//按下叫號
$("#btn_call").on('click', function () {
    if ($("#call_currentNumber").text() != '') {
        connection.invoke("Set_Number", $("#call_currentNumber").text())
            .catch(function (err) {
                return console.error(err.toString());
            });
        $($('#callingTable').DataTable().row(_index_DataTable).node()).siblings().removeClass('patient-called')
        $($('#callingTable').DataTable().row(_index_DataTable).node()).addClass('patient-called')
    }
})

//看診
$("#btn_start").on('click', function () {
    process_state(1)
})
//未到
$("#btn_late").on('click', function () {
    process_state(6)
})
//檢查
$("#btn_exam").on('click', function () {
    process_state(5)
})
//結束
$("#btn_fin").on('click', function () {
    process_state(7)
})

async function process_state(stateNum) {
    const selectedIndex = _index_DataTable
    let url = `${URL_Put_PatientState}/${CLINICLIST_ID}/${stateNum}`
    const response = await fetch(url, { method: 'POST' })
    const result = await response.json()
    //console.log(result)
    $('#callingTable').DataTable().row(selectedIndex).data(result).draw();
}

let connection2 = new signalR.HubConnectionBuilder()
    .withUrl("/ApptStateHub")
    .withAutomaticReconnect()
    .build();

connection2.on("Set_State", async (jsonMsg) => {
    let jsonObj = JSON.parse(jsonMsg)
    let indexes = $('#callingTable').DataTable().rows().eq(0).filter(function (rowIdx) {
        return $('#callingTable').DataTable().cell(rowIdx, 1).data() == jsonObj.clinicListId ? true : false;
    })[0];
    $('#callingTable').DataTable().row(indexes).data(jsonObj).draw()

    const changedRowNode = $('#callingTable').DataTable().row(indexes).node()
    await DataChanged_ColorAnimate(changedRowNode)
});

connection2.start()
    //.then(function () { })
    .catch(function (err) {
        return console.error(err.toString());
    });