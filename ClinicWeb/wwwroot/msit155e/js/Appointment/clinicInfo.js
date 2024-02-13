//初始化表格 欄位如果不變，別再destroy了
function init_ClinicTable() {
    if (!$.fn.DataTable.isDataTable('#apptDataTable')) {
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
                { "data": "clinic_id", "visible": false },
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

    getApptData(_clinicIdSelected);
    $("#addAppt").prop("disabled", false);
    $("#modAppt").prop("disabled", true);
});

$("#apptDataTable tbody").on('mousedown', 'tr', function () {
    _index_apptDataTable = $('#apptDataTable').DataTable().row(this).index();
    if (_index_apptDataTable == null) { return; } //忽略無選擇時
    if ($(this).hasClass('selected')) { return; }   //忽略選擇同一row

    $(this).siblings().removeClass('selected');
    $(this).addClass('selected');

    let clinicId = $('#apptDataTable').DataTable().row(_index_apptDataTable).data().clinic_id;
    let memberId = $('#apptDataTable').DataTable().row(_index_apptDataTable).data().member_id;
    //console.log(clinicId + " - " + memberId + " / " + index)

    $("#modAppt").prop("disabled", false);
});

//右表
async function getApptData(clinicID) {
    //選擇時先清空資料
    $("#apptDataTable").DataTable().clear();
    //要資料
    const response = await fetch(`/Appointment/ApptSys/GET_ApptRecordList/${clinicID}`)
    const data = await response.json()
    //填入資料
    $("#apptDataTable").DataTable().rows.add(data).draw()
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
    const currentPage = $('#apptDataTable').DataTable().page();//紀錄當前page

    if (!_clinicIdSelected || !memberId) {
        alert("未選擇病患或門診")
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
        await getApptData(_clinicIdSelected)
        $("#modAppt").prop("disabled", "disabled");

        await update_PatientNumber()
        //更新重撈後更換page
        await $('#apptDataTable').DataTable().page(currentPage).draw('page') //切換到當前page
    }
}
//更新目前掛號數
async function update_PatientNumber() {
    const response = await fetch(`ApptSys/GET_ClinicPatientNumber/${_clinicIdSelected}`, { method: "POST" })
    const result = await response.json()
    //$('#clinicDataTable').DataTable().row(_index_clinicDataTable).data(result).draw(); //不需要整列更新
    $('#clinicDataTable').DataTable().cell(_index_clinicDataTable, 6).data(result.預約人數).draw();
}
//關閉#addApptForm視窗
function addApptFormClose() {
    $("#addApptForm").modal('toggle')
}
let isCancelled = "";
//修改選擇的掛號紀錄
$("#modAppt").on('click', async (e) => {
    let memberId = $('#apptDataTable').DataTable().row(_index_apptDataTable).data().member_id;

    const response = await fetch(`/Appointment/ApptSys/GET_ApptRecordOne/${_clinicIdSelected}/${memberId}`, { method: "POST" });
    const data = await response.json();
    //const dataArray = await Object.values(data)
    //console.log(dataArray)

    //填入視窗
    $("#ModApptMemberNumber").val(data.clinic_id)
    $("#ModApptNationalId").val(data.member_id)
    $("#ModApptName").val(data.姓名)
    $("#ModApptGender").val(data.性別)
    $("#ModApptBirthDate").val(data.生日)
    $("#ModApptBloodType").val(data.看診狀態)
    $("#ModApptClinicNumber").val(data.診號)
    $("#ModApptState").val(data.身分證字號)
    if (data.退掛 == "是") {
        $("#ModApptIsCancelled").val("True")
        isCancelled = "True"
    } else {
        $("#ModApptIsCancelled").val("False")
        isCancelled = "False"
    }
})
async function ModAppt() {
    let putCancell = $("#ModApptIsCancelled").val() //[區域]putCancell愈更變的值, [全域]isCancelled原始的值
    if (isCancelled == putCancell || isCancelled == "") {
        modApptFormClose()
        return
    }

    let memberId = $('#apptDataTable').DataTable().row(_index_apptDataTable).data().member_id;

    let result;
    try {
        const response = await fetch(`/Appointment/ApptSys/PUT_ApptRecord_Cancelled/${_clinicIdSelected}/${memberId}/${putCancell}`, { method: "POST" });
        result = await response.json()
    } catch (e) {
        new PNotify({
            title: '更新資料失敗',
            text: '發生錯誤，請洽系統管理員',
            type: 'danger',
            styling: 'bootstrap3'
        });
        //$("#modApptMessage").text('更新資料失敗，請洽系統管理員')
        //$("#modApptMessage").css('visibility', 'visible')
        return
    }

    modApptFormClose()
    new PNotify({
        title: '修改成功',
        type: 'success',
        styling: 'bootstrap3'
    });

    //更新右表資料與狀態
    //只重繪修改的該row
    //也許要新增修改看診狀態? 所以整個row重繪
    $('#apptDataTable').DataTable().row(_index_apptDataTable).data(result).draw();
    //更新左表總掛號數
    await update_PatientNumber()
}
//刪除掛號紀錄(非退掛)
async function DelAppt() {
    let memberId = $('#apptDataTable').DataTable().row(_index_apptDataTable).data().member_id;
    const memberName = $('#apptDataTable').DataTable().row(_index_apptDataTable).data().姓名;
    const currentPage = $('#apptDataTable').DataTable().page();//紀錄當前page
    try {
        const response = await fetch(`/Appointment/ApptSys/DEL_ApptRecordOne/${_clinicIdSelected}/${memberId}`, { method: "POST" });
        if (!response.ok) {
            throw new Error()
        }
        result = await response.text()
    } catch (e) {
        new PNotify({
            title: '刪除失敗',
            text: '發生錯誤，請洽系統管理員',
            type: 'danger',
            styling: 'bootstrap3'
        });
        $("#delApptMessage").text('刪除失敗，請洽系統管理員')
        $("#delApptMessage").css('visibility', 'visible')
        return
    }
    if (result == "Success") {
        $("#delApptForm").modal('toggle')
        $("#modApptForm").modal('toggle')
        getApptData(_clinicIdSelected);//還是直接傳回新表??
        $("#modAppt").prop("disabled", "disabled");
        new PNotify({
            title: '刪除成功',
            text: `${memberName} 掛號紀錄已刪除`,
            type: 'success',
            styling: 'bootstrap3'
        });
        await update_PatientNumber()
        //刪除重撈後切換
        //最大頁數不可比當前頁數小，避免page沒有row
        if ($('#apptDataTable').DataTable().page.info().pages - 1 >= currentPage) {
            await $('#apptDataTable').DataTable().page(currentPage).draw('page') //切換到當前page
        }
    }
}
function delApptFormClose() {
    $("#delApptForm").modal('toggle')
}
//關閉#modApptForm視窗
function modApptFormClose() {
    $("#modApptForm").modal('toggle')
    $("#modApptMessage").css('visibility', 'hidden')
    isCancelled = ""
}
