function init_callingTable() {
    if (!$.fn.DataTable.isDataTable('#callingTable')) {
        $('#callingTable').dataTable({
            columns: [
                { "data": "member_id", "visible": false },
                { "data": "status_id", "visible": false },
                { "data": "診號" },
                { "data": "姓名" },
                { "data": "性別" },
                { "data": "狀態" }
            ],
            fixedHeader: {
                header: false
            },
            language: {
                url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
            },
            //排序目標
            order: [[1, 'asc']],
            //關閉排序和搜尋目標
            aoColumnDefs: [
                { "bSortable": false, "aTargets": [0, 1, 2, 3, 4, 5] },
                { "bSearchable": false, "aTargets": [0, 1, 2, 3, 4, 5] }
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

    const doctorId = 4

    //const date = new Date() //今天
    const date = new Date("2024/02/01")
    const datedata = `${date.getFullYear()}/${date.getMonth()+1 }`

    const shiftId = 4

    const jsonData = { 'doctorId': doctorId, 'date': datedata, 'shiftId': shiftId }

    //選擇時先清空資料
    //$("#callingTable").DataTable().clear();
    //要資料
    //const selectedDate = $("#Date").val()
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
    //$("#callingTable").DataTable().rows.add(data).draw()
}