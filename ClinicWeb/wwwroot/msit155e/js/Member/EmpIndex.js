//顯示會員主畫面資料表
function QueryEmpInfo() {
    try {
        $('#empdatatable').dataTable({
            ajax: {
                type: 'GET',
                url: (url_Emp_GetData),
                dataSrc: function (json) { return json; }
            },
            destroy: true,
            columns: [
                // { "data": "id", "visible": false },
                // { "屬性": "值", "visible": false },建議看JSON撈到的來寫欄位 不然會對錯
                { "data": "員工id", "visible": false },
                { "data": "員工編號" },
                { "data": "姓名" },
                { "data": "性別" },
                { "data": "血型" },
                { "data": "身分證字號" },
                { "data": "生日" },
                { "data": "聯絡電話" },
                { "data": "地址" },
                { "data": "員工類別" },
                { "data": "在職" },
                {
                    "data": "修改",
                    "render": function (data, type, row) {
                        return '<button type="button" class="fa fa-edit border:none indexSelector" style="border: none;" data-toggle="modal" data-target="#EmpEdit" onclick="handleButtonClick(' + row.員工id + ')"></button>' +
                            '<button type="button" class="fa fa-file-word-o border:none indexSelector" style="border: none;" data-toggle="modal" data-target="#EmpView" onclick="handleViewButtonClick(' + row.員工id + ')"></button>';
                    }
                }
            ],
            fixedHeader: {
                header: true
            },
            language: {
                url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
            },
            order: [[1, 'asc']]
        });
    } catch (error) {
        console.error('在初始化員工資料表時發生錯誤:', error);
        new PNotify({
            title: '錯誤',
            text: '初始化員工資料表失敗',
            type: 'error',
            styling: 'bootstrap3'
        })
        alert('初始化員工資料表失敗');
        return;
    }
}

QueryEmpInfo();

//在職狀態搜尋
$("#employmentStatus").change(() => { EmpStatus() });

function EmpStatus() {
    if ($("#employmentStatus").find(":selected").text() == '在職狀態') {

        $('#empdatatable').DataTable().column(10).search("").draw();
    } else {
        $('#empdatatable').DataTable().column(10).search($("#employmentStatus").find(":selected").text()).draw();
    }
}
//生日搜尋

//員工類型搜尋
$("#employeeType").change(() => { EmpType() });
function EmpType() {
    if ($("#employeeType").find(":selected").text() == '選擇員工類別') {

        $('#empdatatable').DataTable().column(9).search("").draw();
    } else {
        $('#empdatatable').DataTable().column(9).search($("#employeeType").find(":selected").text()).draw();
    }
}


//-------------------------------------------------
//新增

//選取DOM物件，宣告變數
//選取小窗div
const $EmpCreate = $('#EmpCreate');
//選取表單顯示div
const $divCreateInfo = $('#divCreateInfo')
const _divCreateInfo = document.getElementById('divCreateInfo');
//選取表單
let $form_EmpCreate;
let _form_EmpCreate;

//跳出新增會員小窗
$("#newEmpbtn").on('click', async function () {
    //fetch新表單
    const response = await fetch(url_EmpCreate_Form)
    const data = await response.text();
    $divCreateInfo.html(data);

    //保定
    rebindAll_create()
})

function rebindAll_create() {

    //選取表單
    $form_empCreate = $('#form_EmpCreate')
    _form_empCreate = document.getElementById('form_EmpCreate')

    //表單動態驗證綁定
    rebind_validate($form_empCreate)
    //save事件綁定
    event_AddMemBtn()
    //switchery綁定
    rebind_switchery(document.getElementById('Quit'))
    //關閉綁定
    document.getElementById('closeButton').addEventListener('click', function () {
        deleteCreateForm();
    });
}

//宣告事件綁定為一方法，給裡頭遞迴使用
function event_AddMemBtn() {
    //新增會員的按下儲存後
    $("#AddMemBtn").on('click', async function (event) {
        //停止預設行為
        event.preventDefault();

        //動態驗證未過，直接停止
        if (!$form_empCreate.valid()) { return; }

        //建立formData物件，轉換成json物件，以及修正資料
        const formData = new FormData(_form_empCreate);
        const Quit = $("#Quit").is(":checked")
        formData.set('Quit', Quit);

        let jsonobject = {};
        formData.forEach(function (value, key) {
            jsonobject[key] = value;
        });

        if (jsonobject.Gender == '') { jsonobject.Gender = null }
        if (jsonobject.Gender == 'True') { jsonobject.Gender = true }
        if (jsonobject.Gender == 'False') { jsonobject.Gender = false }

        jsonobject.Quit = jsonobject.Quit == 'true' ? true : false;

        //字串化json物件
        const jsonString = JSON.stringify(jsonobject)

        //嘗試fetch
        try {
            const response = await fetch(url_EmpCreate_save, {
                method: "POST",
                headers: {
                    'Content-Type': 'application/json;charset=UTF-8',
                    'RequestVerificationToken': jsonobject.__RequestVerificationToken,  //RequestVerificationToken要加在header裡面
                    'Accept': 'text/plain'
                },
                body: jsonString
            });
            const data = await response.text();

            //處理回傳的結果，成功 => "success"，失敗 => partial HTML文件
            if (data === "success") {
                $EmpCreate.modal('hide');
                new PNotify({
                    title: '新增成功',
                    type: 'success',
                    styling: 'bootstrap3'
                });
                QueryEmpInfo();
                deleteCreateForm();
            }
            else {
                //把剛剛所有formdata包含錯誤訊息塞給divCreateInfo 也就是createpartial的id
                _divCreateInfo.innerHTML = data;

                //綁定
                rebindAll_create()
            }

        } catch {
            new PNotify({
                title: '錯誤',
                text: '新增資料失敗',
                type: 'error',
                styling: 'bootstrap3'
            })
            alert('新增資料失敗，請洽系統管理員');
            return;
        }
    })
}


function deleteCreateForm() {
    $EmpCreate.modal('hide') //關掉小窗
    $divCreateInfo.html('') //刪除顯示div
}

//抓到該筆編輯資料的索引
let _index_empdatatable;
$("#empdatatable tbody").on('click', '.indexSelector', function () {
    const selectTr = $(this).closest('tr')
    _index_empdatatable = $('#empdatatable').DataTable().row(selectTr).index()
    let memberId = $("#empdatatable").DataTable().row(_index_empdatatable).data().員工id;
});

//-----------------------
//編輯

//抓取小窗div
const $EmpEdit = $("#EmpEdit")
//選取表單顯示div
let $divEditInfo = $('#divEditInfo')
let _divEditInfo = document.getElementById("divEditInfo")
//選取表單
let $form_empEdit
let _form_empEdit

function rebindAll_Edit() {
    //選取表單
    $form_empEdit = $('#form_EmpEdit')
    _form_empEdit = document.getElementById('form_EmpEdit')

    //表單動態驗證綁定
    rebind_validate($form_empEdit)
    //save事件綁定
    buttonEventFunc();
    //switchery綁定
    rebind_switchery(document.getElementById('Quit'))
    //事件綁定 關閉新增會員時
    document.getElementById('editCloseButton').addEventListener('click', function () {
        deleteFormEdit();
    });
}
function deleteFormEdit() {
    $EmpEdit.modal('hide') //關掉小窗
    $divEditInfo.html('') //刪除顯示div
}


//顯示編輯小窗
async function handleButtonClick(empId) {
    try {
        const response = await fetch(`${url_EmpEdit_Form}?empId=${empId}`);
        if (!response.ok) {
            throw new Error('在取得會員資料並打開編輯畫面出現錯誤，請洽系統管理員');
        }
        const data = await response.text();

        _divEditInfo.innerHTML = data;

        //重新綁定
        rebindAll_Edit()

    } catch (error) {
        console.error('在取得會員資料並打開編輯畫面出現錯誤，請洽系統管理員:', error);

        // 顯示給使用者
        new PNotify({
            title: '錯誤',
            text: '在取得會員資料並打開編輯畫面出現錯誤，請洽系統管理員',
            type: 'error',
            styling: 'bootstrap3'
        });

    }
}



//編輯儲存
async function buttonEventFunc() {
    _form_empEdit.addEventListener("submit", async (event) => {
        event.preventDefault(); // 防止表單默認提交行為

        //動態驗證未過，直接停止
        if (!$form_empEdit.valid()) { return; }

        // empdatatable
        const formData = new FormData(_form_empEdit);

        let result;
        const Quit = $("#Quit").is(":checked")
        formData.set('Quit', Quit);

        try {
            const response = await fetch(url_EmpEdit_save, {
                "method": "POST",
                "body": formData
            });
            result = await response.json();
        }
        catch {
            result = await response.text()
            if (result == "驗證未通過") {
                return;
            }

            new PNotify({
                title: '錯誤',
                text: '修改資料失敗',
                type: 'error',
                styling: 'bootstrap3'
            })

            alert('修改資料失敗，請洽系統管理員');
            return;
        }

        deleteFormEdit()
        new PNotify({
            title: '修改成功',
            type: 'success',
            styling: 'bootstrap3'
        });

        const currentPage = $('#empdatatable').DataTable().page()
        //只重繪修改的該row
        $('#empdatatable').DataTable().row(_index_empdatatable).data(result);

        $('#empdatatable').DataTable().page(currentPage).draw('page')

    });
}

