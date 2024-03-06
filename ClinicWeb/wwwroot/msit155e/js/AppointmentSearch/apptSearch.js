function init_appt_Table() {
    if (!$.fn.DataTable.isDataTable('#apptDataTable')) {
        $('#apptDataTable').dataTable({
            columns: [
                { "data": "id", "visible": false },
                { "data": "日期" },
                { "data": "時段" },
                { "data": "科別" },
                { "data": "醫師名稱" },
                { "data": "診間" },
                { "data": "診號" },
                { "data": "看診狀態" },
                {
                    "data": "修改",
                    "render": function (data, type, row) { return '<button type="button" class="btn btn-danger .indexSelector" data-bs-toggle="modal" data-bs-target="#cancelApptModal" onclick="rowBtnOnClick(' + row.id + ')">退掛</button>' }
                }
            ],
            fixedHeader: {
                header: false
            },
            order: [[1, 'asc']],
            language: {
                url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
            },            //關閉排序和搜尋目標
            aoColumnDefs: [
                { "bSortable": false, "aTargets": [0, 1, 2, 3, 4, 5, 6, 7, 8] },
                { "bSearchable": false, "aTargets": [0, 1, 2, 3, 4, 5, 6, 7, 8] }
            ]//,
            ////搜尋框
            //searching: false,
            ////分頁
            //bPaginate: false,
            ////筆數頁數資訊
            //info: false
        });
    }
}
init_appt_Table()

//撈取登入者資訊
async function get_LoginInfo() {
    const response = await fetch(__checkLoginURL)
    const data = await response.json()
    console.log(data)
}
get_LoginInfo()

//撈取登入者掛號資料 (今天以後)
async function get_ApptList() {
    const jsonData = { today: new Date() }

    const response = await fetch(__getApptListURL,
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                // Content-Type: 'application/x-www-form-urlencoded',
            },
            body: JSON.stringify(jsonData)
        })
    const data = await response.json()
    $("#apptDataTable").DataTable().clear()
    $("#apptDataTable").DataTable().rows.add(data).draw()
}
get_ApptList()

//點擊退掛按鈕
let _idSelected = -1;
function rowBtnOnClick(id) {
    _idSelected = id;
    console.log(_idSelected)
}

//點擊確認取消掛號
async function confirmCancelBtnOnclick() {
    const url = `${__cancelApptURL}/${_idSelected}`
    const response = await fetch(url, { method: 'POST' })
    if (response.ok) {
        console.log("已退掛")
        //重新撈取表單
        get_ApptList()
        new PNotify({
            title: '退掛成功',
            type: 'success',
            styling: 'bootstrap3'
        });
    }
    else {
        console.log("錯誤!")
        new PNotify({
            title: '錯誤!',
            text:'發生錯誤，請洽系統管理員',
            type: 'error',
            styling: 'bootstrap3'
        });
    }
}