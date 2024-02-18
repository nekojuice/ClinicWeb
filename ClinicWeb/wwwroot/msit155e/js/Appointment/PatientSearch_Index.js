//初始化表格
function init_PatientApptTable() {
    if (!$.fn.DataTable.isDataTable('#apptDataTable')) {
        $('#apptDataTable').dataTable({
            columns: [
                { "data": "clinicAppt_id", "visible": false },
                { "data": "姓名", "visible": false },
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

//全域變數: 當前選取的會員id
let selectedMemberId = '';
//填入當前目標
async function memberNationalIdSearchClick(id, searchResult) {
    selectedMemberId = id
    $("#memberNationalIdSearch").val(searchResult)
    $("#memberNationalIdSearchData").hide()
    //搜尋
    getApptData()
}

//執行搜尋
async function getApptData() {
    //是否包含歷史紀錄(當月以前)
    const isContainHistory = $("#isContainHistory").is(":checked")

    const response = await fetch(`/Appointment/PatientSearch/PatientApptTable/${selectedMemberId}/${isContainHistory}`, { method: "POST" })
    const data = await response.json()
    $('#apptDataTable').DataTable().clear().draw
    $("#apptDataTable").DataTable().rows.add(data).draw()
}


let _index_apptDataTable = '';
//編輯按鈕按下
$("#apptDataTable tbody").on('click', '.btn_edit', function () {
    const selectedTr = $(this).closest('tr')
    _index_apptDataTable = $("#apptDataTable").DataTable().row(selectedTr).index()
    const clinicAppt_id = $("#apptDataTable").DataTable().row(_index_apptDataTable).data().clinicAppt_id;

    ModApptForm_OPEN(clinicAppt_id)
})

//修改的callback
async function modApptCallback() {
    //目前啥都沒有
}
//刪除的callback
async function delApptCallback() {
    getApptData()
}

//切換 是否包含歷史資料開關時
$("#isContainHistory").on('click', function () {
    getApptData()
})