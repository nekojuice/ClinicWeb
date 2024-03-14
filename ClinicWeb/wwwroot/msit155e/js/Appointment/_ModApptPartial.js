////套用小窗的js主程式碼需要宣告修改和刪除的callback，觸發資料更新完成後的動作(更新表格等)
//async function modApptCallback() {}
//async function delApptCallback() {}

////修改用區域
//修改用全域變數
//isCancelled修改前是否退掛
let isCancelled = '';
let originStateString = '';
//打開修改視窗
async function ModApptForm_OPEN(clinicAppt_id) {
    const response = await fetch(`/Appointment/ApptSys/GET_ApptRecordOne/${clinicAppt_id}`, { method: "POST" });
    const data = await response.json();
    //const dataArray = await Object.values(data)
    //console.log(dataArray)

    //填入視窗
    //$("#ModApptMemberId").val(data.member_id)
    $("#ModApptMemberNumber").val(data.會員號碼)
    $("#ModApptNationalId").val(data.身分證字號)
    $("#ModApptName").val(data.姓名)
    $("#ModApptGender").val(data.性別)
    $("#ModApptBirthDate").val(data.生日)
    $("#ModApptBloodType").val(data.血型)
    $("#ModApptClinicNumber").val(data.診號)
    originStateString = data.看診狀態
    $("#ModApptState option")
        .filter(function () {
            return $(this).text() == data.看診狀態;
        })
        .prop('selected', true);

    isCancelled = data.退掛 == "是" ? "True" : "False"
    $("#ModApptIsCancelled").val(isCancelled)

    $("#ModApptDate").val(data.日期)
    $("#ModApptClinicShifts").val(data.時段)
    $("#ModApptDepartment").val(data.科別)
    $("#ModApptDoctorName").val(data.醫師名稱)
}

//執行修改送出
async function ModAppt() {
    //[區域]putCancell 更變後的值, [全域]isCancelled 原始的值
    const putCancell = $("#ModApptIsCancelled").val()
    const putState = $("#ModApptState option:selected").text()
    const putStateVal = $("#ModApptState").val()
    //未更變則直接退出
    if ((isCancelled == putCancell || isCancelled == '') && (originStateString == putState || originStateString == '')) {
        modApptFormClose()
        return
    }

    //執行更新資料
    const clinicAppt_id = $('#apptDataTable').DataTable().row(_index_apptDataTable).data().clinicAppt_id;
    let result;
    try {
        const response = await fetch(`/Appointment/ApptSys/PUT_ApptRecord_Cancelled/${clinicAppt_id}/${putCancell}/${putStateVal}`, { method: "POST" });
        result = await response.json()
    } catch (e) {
        new PNotify({
            title: '更新資料失敗',
            text: '發生錯誤，請洽系統管理員',
            type: 'error',
            styling: 'bootstrap3'
        });
        return
    }
    modApptFormClose()

    //顯示成功訊息
    const memberName = $('#apptDataTable').DataTable().row(_index_apptDataTable).data().姓名;
    new PNotify({
        title: '修改成功',
        text: `${memberName} 掛號已修改`,
        type: 'success',
        styling: 'bootstrap3'
    });

    //更新右表資料與狀態
    //只重繪修改的該row
    //也許要再修改其他看診狀態，vip狀態等...? 所以整個row重繪
    $('#apptDataTable').DataTable().row(_index_apptDataTable).data(result).draw();
    //跳至當前資料
    $('#apptDataTable').DataTable().page.jumpToData(+clinicAppt_id, 0);
    //閃爍動畫
    const changedRowNode = $('#apptDataTable').DataTable().row(_index_apptDataTable).node()
    await DataChanged_ColorAnimate(changedRowNode)
    //嘗試執行任何callback
    try { await modApptCallback() } catch (e) { }

}

//刪除掛號紀錄(非退掛)
async function DelAppt() {
    const clinicAppt_id = $('#apptDataTable').DataTable().row(_index_apptDataTable).data().clinicAppt_id;
    const memberName = $('#apptDataTable').DataTable().row(_index_apptDataTable).data().姓名;
    const currentPage = $('#apptDataTable').DataTable().page();//紀錄當前page
    try {
        const response = await fetch(`/Appointment/ApptSys/DEL_ApptRecordOne/${clinicAppt_id}`, { method: "POST" });
        if (!response.ok) {
            throw new Error()
        }
        result = await response.text()
    } catch (e) {
        new PNotify({
            title: '刪除失敗',
            text: '發生錯誤，請洽系統管理員',
            type: 'error',
            styling: 'bootstrap3'
        });
        $("#delApptMessage").text('刪除失敗，請洽系統管理員')
        $("#delApptMessage").css('visibility', 'visible')
        return
    }
    if (result == "Success") {
        $("#delApptForm").modal('toggle')
        $("#modApptForm").modal('toggle')
        $("#modAppt").prop("disabled", "disabled");
        new PNotify({
            title: '刪除成功',
            text: `${memberName} 掛號紀錄已刪除`,
            type: 'danger',
            styling: 'bootstrap3'
        });
        //執行刪除的callback
        await delApptCallback() 
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
