//初始化表格
function init_PatientApptTable() {
    if (!$.fn.DataTable.isDataTable('#patientApptTable')) {
        $('#patientApptTable').dataTable({
            columns: [
                { "data": "clinicAppt_id", "visible": false },
                { "data": "日期" },
                { "data": "時段" },
                { "data": "科別" },
                { "data": "醫師名稱" },
                { "data": "診號" },
                { "data": "退掛" },
                { "data": "看診狀態" },
                { defaultContent: '<button type="button" class="btn btn-info btn_edit" data-toggle="modal" data-target=".modApptModal">編輯</button>'}
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
init_PatientApptTable()


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

    const response = await fetch(`/Appointment/PatientSearch/PatientApptTable/${id}/True`, { method: "POST" })
    const data = await response.json()
    $('#patientApptTable').DataTable().clear().draw
    $("#patientApptTable").DataTable().rows.add(data).draw()
}

//編輯按鈕按下
$("#patientApptTable tbody").on('click', '.btn_edit', function () {
    const selectedTr = $(this).closest('tr')
    const index = $("#patientApptTable").DataTable().row(selectedTr).index()
    const clinicAppt_id = $("#patientApptTable").DataTable().row(index).data().clinicAppt_id;
    console.log(clinicAppt_id);

    ModAppt(clinicAppt_id)
})

function modApptFormClose() {
    $("#modApptForm").modal('toggle')
    $("#modApptMessage").css('visibility', 'hidden')
    isCancelled = ""
}
function delApptFormClose() {
    $("#delApptForm").modal('toggle')
}

//修改選擇的掛號紀錄
async function ModAppt(clinicAppt_id) { 
    const response = await fetch(`/Appointment/PatientSearch/GET_ApptRecordOne/${clinicAppt_id}`, { method: "POST" })
    const data = await response.json();

    //填入視窗
    //$("#ModApptMemberId").val(data.member_id)
    $("#ModApptMemberNumber").val(data.會員號碼)
    $("#ModApptNationalId").val(data.身分證字號)
    $("#ModApptName").val(data.姓名)
    $("#ModApptGender").val(data.性別)
    $("#ModApptBirthDate").val(data.生日)
    $("#ModApptBloodType").val(data.血型)
    $("#ModApptClinicNumber").val(data.診號)
    $("#ModApptState").val(data.看診狀態)
    if (data.退掛 == "是") {
        $("#ModApptIsCancelled").val("True")
        isCancelled = "True"
    } else {
        $("#ModApptIsCancelled").val("False")
        isCancelled = "False"
    }
}