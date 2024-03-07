//初始化表格 欄位如果不變，別再destroy了
function init_ClinicTable() {
    if (!$.fn.DataTable.isDataTable('#clinicDataTable')) {
        $('#clinicDataTable').dataTable({
            columns: [
                { "data": "id", "visible": false },
                { "data": "日期" },
                { "data": "時段" },
                { "data": "科別" },
                { "data": "醫師名稱" },
                { "data": "上限人數" },
                { "data": "預約人數" }
            ],
            fixedHeader: {
                header: true
            },
            language: {
                url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
            },
            order: [[1, 'asc']]
        });
    }
}
function init_ApptTable() {
    if (!$.fn.DataTable.isDataTable('#apptDataTable')) {
        $("#apptDataTable").dataTable({
            columns: [
                { "data": "clinicAppt_id", "visible": false },
                { "data": "member_id", "visible": false },
                { "data": "診號" },
                { "data": "姓名" },
                { "data": "生日" },
                { "data": "性別" },
                { "data": "身分證字號" },
                { "data": "退掛" },
                { "data": "看診狀態" }
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
}
init_ClinicTable()
init_ApptTable()

//左表
$("#Date").change(() => {
    DateOnChange();
    $("#addAppt").prop("disabled", true);
    $("#modAppt").prop("disabled", true);
});

//時段選擇
function DateOnChange() {
    if ($("#Date").val() != '--請選擇--') {
        reset_selectRowState()
        QueryClinicInfo();
    }
    else {
        $('#clinicDataTable').DataTable().clear().draw();
        reset_selectRowState()
    }
}
//依照左點選搜尋右表的資料
async function QueryClinicInfo() {
    //選擇時先清空資料
    $("#clinicDataTable").DataTable().clear();
    //要資料
    const selectedDate = $("#Date").val()
    const response = await fetch(`/Appointment/ApptSys/GET_ClinicInfoList/${selectedDate}`)
    const data = await response.json()
    //填入資料
    $("#clinicDataTable").DataTable().rows.add(data).draw()
}
//下拉選單 選擇科別時
$("#Doctor_Department").change(() => { searchDept() });
function searchDept() {
    if ($("#Doctor_Department").val() == '全部') {
        $('#clinicDataTable').DataTable().column(3).search("").draw();
    }
    else {
        $('#clinicDataTable').DataTable().column(3).search($("#Doctor_Department").val()).draw();
    }
    reset_selectRowState()
}

//下拉選單 選擇時段時
$("#ClinicTime_ClinicShifts").change(() => { searchTime() });
function searchTime() {
    if ($("#ClinicTime_ClinicShifts").val() == '全部') {
        $('#clinicDataTable').DataTable().column(2).search("").draw();
    }
    else {
        $('#clinicDataTable').DataTable().column(2).search($("#ClinicTime_ClinicShifts").val()).draw();
    }
    reset_selectRowState()
}

//重置左表選擇
function reset_selectRowState() {
    $("#apptDataTable").DataTable().clear().draw();
    _index_clinicDataTable == null
    $('#clinicDataTable tr').removeClass('selected');
    $("#addAppt").prop("disabled", true);
}

//全域變數
let _clinicIdSelected;  //選擇的診間id
let _index_clinicDataTable;  //左表index
let _index_apptDataTable;  //右表index

//左表點擊事件
$("#clinicDataTable tbody").on('mousedown', 'tr', function () {
    _index_clinicDataTable = $('#clinicDataTable').DataTable().row(this).index();
    if (_index_clinicDataTable == null) { return; } //忽略無選擇時
    if ($(this).hasClass('selected')) { return; }   //忽略選擇同一row
    //console.log(index)
    _clinicIdSelected = $('#clinicDataTable').DataTable().row(_index_clinicDataTable).data().id;

    $(this).siblings().removeClass('selected');
    $(this).addClass('selected');

    getApptData();
    $("#addAppt").prop("disabled", false);
    $("#modAppt").prop("disabled", true);
});


$("#apptDataTable tbody").on('mousedown', 'tr', function () {
    _index_apptDataTable = $('#apptDataTable').DataTable().row(this).index();
    if (_index_apptDataTable == null) { return; } //忽略無選擇時
    if ($(this).hasClass('selected')) { return; }   //忽略選擇同一row

    $(this).siblings().removeClass('selected');
    $(this).addClass('selected');

    //let memberId = $('#apptDataTable').DataTable().row(_index_apptDataTable).data().member_id;
    //console.log(clinicId + " - " + memberId + " / " + index)

    $("#modAppt").prop("disabled", false);
});

//右表
async function getApptData() {
    //先清空資料
    $("#apptDataTable").DataTable().clear();
    //撈資料
    const response = await fetch(`/Appointment/ApptSys/GET_ApptRecordList/${_clinicIdSelected}`)
    const data = await response.json()
    //填入資料
    await $("#apptDataTable").DataTable().rows.add(data).draw();
}

//下拉選單 搜尋是否退掛
$("#isCancelled").change(() => { searchIsCancelled() });
function searchIsCancelled() {
    if ($("#isCancelled").val() == '全部') {
        $('#apptDataTable').DataTable().column(7).search("").draw();
    }
    else {
        $('#apptDataTable').DataTable().column(7).search($("#isCancelled").val()).draw();
    }
}

//載入完成時啟動
//載入時選取當前月份
$(document).ready(() => {
    let year = new Date().getFullYear().toString()
    let month = (+(new Date().getMonth().toString()) + 1).toString().padStart(2, 0)
    let yyyymm = `${year}/${month}`

    for (let option of document.getElementById('Date').options) {
        if (option.innerText == yyyymm) {
            document.getElementById('Date').selectedIndex = option.index
        }
    }
    DateOnChange()
})

$("#memberNationalIdSearch").on('input', async (e) => {
    //不符合規則則不查詢
    let regexNatId = new RegExp('^[A-Za-z0-9]{1}[0-9]*$')
    if ($("#memberNationalIdSearch").val() == "" || !regexNatId.test($("#memberNationalIdSearch").val())) {
        $("#memberNationalIdSearchData").html("")
        return;
    }
    const url = "ApptSys/GET_MemberDataSnap/" + $("#memberNationalIdSearch").val();
    const response = await fetch(url);
    const data = await response.json();

    //有空將顯字串改成一個個label 小標籤樣式
    const dataHtml = (data.map(x => `<button type="button" class="list-group-item list-group-item-action" onclick="memberNationalIdSearchClick(${x.id},'${x.身分證字號} | ${x.姓名} | ${x.性別} | ${x.生日}')">
    ${x.身分證字號} | ${x.姓名} | ${x.性別} | ${x.生日}</button>`)).join("")

    $("#memberNationalIdSearchData").html(dataHtml)
    $("#memberNationalIdSearchData").show()
})

//點選搜尋欄時 文字全選反白
$("#memberNationalIdSearch").on('focus', () => { $("#memberNationalIdSearch").select() })

//收起搜尋結果事件
$("#addApptForm").on('click', (e) => {
    if ($(e.target).is($("#memberNationalIdSearch"))) {
        $("#memberNationalIdSearchData").show();
    }
    else {
        $("#memberNationalIdSearchData").hide()
    }
})

//填入當前目標與執行搜尋
async function memberNationalIdSearchClick(id, searchResult) {
    $("#memberNationalIdSearch").val(searchResult)
    $("#memberNationalIdSearchData").hide()
    //隱藏重複掛號警告
    $("#addApptMessage").css('visibility', 'hidden')

    const response = await fetch(`/Appointment/ApptSys/GET_MemberData/${id}`, { method: "POST" })
    const data = await response.json()
    $("#AddApptMemberId").val(data.id)
    $("#AddApptMemberNumber").val(data.memberNumber)
    $("#AddApptNationalId").val(data.nationalId)
    $("#AddApptName").val(data.name)
    $("#AddApptGender").val(data.gender)
    $("#AddApptBirthDate").val(data.birthDate)
    $("#AddApptBloodType").val(data.bloodType)
    $("#AddApptContactAddress").val(data.contactAddress)
    $("#AddApptPhone").val(data.phone)
    $("#AddApptMemEmail").val(data.memEmail)
    $("#AddApptIceName").val(data.iceName)
    $("#AddApptIceNumber").val(data.iceNumber)
}
//增加新掛號
async function AddAppt() {
    const memberId = $("#AddApptMemberId").val()
    const isVIP = $("#AddApptIsVIP").is(":checked")
    //const currentPage = $('#apptDataTable').DataTable().page();//紀錄當前page


    if (!_clinicIdSelected || !memberId) {
        $("#addApptMessage").text('未選擇病患')
        $("#addApptMessage").css('visibility', 'visible')
        return
    }
    //loading gif
    $("#addApptLoading").css('visibility', 'visible');
    $("#btnAddAppt").attr('disable', 'disable')
    let result;
    try {
        const response = await fetch(`/Appointment/ApptSys/Add_ApptRecord/${_clinicIdSelected}/${memberId}/${isVIP}`, { method: "POST" })
        result = await response.text()    //result= 'Fail'新增出錯失敗, 'True'重複失敗, 'False'未重複且掛號成功
    } catch (e) {
        $("#addApptMessage").text('新增至資料庫失敗，請洽系統管理員')
        $("#addApptMessage").css('visibility', 'visible')
    }

    await $("#addApptLoading").css('visibility', 'hidden')
    await $("#btnAddAppt").attr('disable', 'none')
    if (result === 'True') {
        //重複掛號
        $("#addApptMessage").text('此病患已重複掛號')
        $("#addApptMessage").css('visibility', 'visible')
    }
    else {
        $("#addApptMessage").css('visibility', 'hidden')
        addApptFormClose()

        new PNotify({
            title: '掛號成功',
            text: $("#AddApptName").val() + " 已掛號",
            type: 'success',
            styling: 'bootstrap3'
        });

        //更新左表掛號數
        await update_PatientNumber()
        $("#modAppt").prop("disabled", "disabled");

        //更新掛號資料表
        await getApptData(_clinicIdSelected)


        //套件jumpToData(資料,欄位編號)
        $('#apptDataTable').DataTable().page.jumpToData(+memberId, 1);  //memberId要轉成int

        //抓取新掛號node
        const changedRowNode = await $('#apptDataTable').DataTable().row(searchFunction({ member_id: +memberId })).node();
        await DataChanged_ColorAnimate(changedRowNode);

    }
}

//搜尋datatable row資料函式
const searchFunction = function (dictSearch) {
    return function (idx, data, node) {
        const keys = Object.keys(dictSearch);
        const n = keys.length;
        let k = 0;
        for (let i = 0; i <= keys.length; i++) {
            //intrinsically checks if the key exists
            if (data[keys[i]] === dictSearch[keys[i]]) {
                k++;
            } else {
                return false;
            }
            if (n === k) {
                return true;
            }
        }
        return false;
    }
}

//更新目前掛號數
async function update_PatientNumber() {
    const currentPage = $('#clinicDataTable').DataTable().page();//紀錄當前page
    const response = await fetch(`ApptSys/GET_ClinicPatientNumber/${_clinicIdSelected}`, { method: "POST" })
    const result = await response.json()
    //$('#clinicDataTable').DataTable().row(_index_clinicDataTable).data(result).draw(); //不需要整列更新
    await $('#clinicDataTable').DataTable().cell(_index_clinicDataTable, 6).data(result.預約人數).draw();
    await $('#clinicDataTable').DataTable().page(currentPage).draw('page') //切換到當前page
    //閃爍動畫
    const changedRowNode = $('#clinicDataTable').DataTable().row(_index_clinicDataTable).node()
    await DataChanged_ColorAnimate(changedRowNode)
}
//關閉#addApptForm視窗
function addApptFormClose() {
    $("#addApptForm").modal('toggle')
}

//點擊修改按鈕
$("#modAppt").on('click', async () => {
    const clinicAppt_id = $('#apptDataTable').DataTable().row(_index_apptDataTable).data().clinicAppt_id;
    ModApptForm_OPEN(clinicAppt_id)
})

//修改的callback
async function modApptCallback() {
    //更新左表總掛號數
    await update_PatientNumber()
}
//刪除的callback
async function delApptCallback() {
    await getApptData(_clinicIdSelected);//還是直接傳回新表??
    await update_PatientNumber()
}