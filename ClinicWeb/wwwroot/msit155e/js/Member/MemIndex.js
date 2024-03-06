//顯示會員主畫面資料表
function QueryMemberInfo() {
    try {
        $('#memdatatable').dataTable({
            ajax: {
                type: 'GET',
                url: (url_Member_GetData),
                dataSrc: function (json) { return json; }
            },
            destroy: true,
            columns: [
                // { "data": "id", "visible": false },
                // { "屬性": "值", "visible": false },建議看JSON撈到的來寫欄位 不然會對錯
                { "data": "會員id", "visible": false },
                { "data": "會員編號" },
                { "data": "姓名" },
                { "data": "性別" },
                { "data": "血型" },
                { "data": "身分證字號" },
                { "data": "聯絡電話" },
                { "data": "地址" },
                { "data": "緊急聯絡人" },
                { "data": "信箱" },
                { "data": "啟用" },
                {
                    "data": "修改",
                    "render": function (data, type, row) {
                        return '<button type="button" class="btn btn-round btn-info" indexSelector" style="border: none;" data-toggle="modal" data-target="#MemEdit" onclick="handleButtonClick(' + row.會員id + ')">編輯</button>' +
                            '<button type="button" class="btn btn-round btn-warning" indexSelector" style="border: none;" data-toggle="modal" data-target="#MemView" onclick="handleViewButtonClick(' + row.會員id + ')">檢視</button>';
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
        console.error('在初始化會員資料表時發生錯誤:', error);
        new PNotify({
            title: '錯誤',
            text: '初始化會員資料表失敗',
            type: 'error',
            styling: 'bootstrap3'
        })
        alert('初始化會員資料表失敗');
        return;
    }
}

QueryMemberInfo();


//控制對於密碼輸入欄位眼睛的顯示
function hideshow() {
    var password = document.getElementById("MemPassword");
    var slash = document.getElementById("slash");
    var eye = document.getElementById("eye");

    if (password.type === 'password') {
        password.type = "text";
        slash.style.display = "block";
        eye.style.display = "none";
    }
    else {
        password.type = "password";
        slash.style.display = "none";
        eye.style.display = "block";
    }

}

//-------------------------------------------------
//新增

//選取DOM物件，宣告變數
//選取小窗div
const $MemCreate = $('#MemCreate');
//選取表單顯示div
const $divCreateInfo = $('#divCreateInfo')
const _divCreateInfo = document.getElementById('divCreateInfo');
//選取表單
let $form_memberCreate;
let _form_memberCreate;

//跳出新增會員小窗
$("#newMembtn").on('click', async function () {
    //fetch新表單
    const response = await fetch(url_MemberCreate_Form)
    const data = await response.text();
    $divCreateInfo.html(data);

    //綁定
    rebindAll_create()
})

function rebindAll_create() {

    //選取表單
    $form_memberCreate = $('#form_MemberCreate')
    _form_memberCreate = document.getElementById('form_MemberCreate')

    //表單動態驗證綁定
    rebind_validate($form_memberCreate)
    //save事件綁定
    event_AddMemBtn()
    //switchery綁定
    rebind_switchery(document.getElementById('Verification'))
    //事件綁定 關閉新增會員時
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
        if (!$form_memberCreate.valid()) { return; }

        //建立formData物件，轉換成json物件，以及修正資料
        const formData = new FormData(_form_memberCreate);
        const Verification = $("#Verification").is(":checked")
        formData.set('Verification', Verification);

        let jsonobject = {};
        formData.forEach(function (value, key) {
            jsonobject[key] = value;
        });

        if (jsonobject.Gender == '') { jsonobject.Gender = null }
        if (jsonobject.Gender == 'True') { jsonobject.Gender = true }
        if (jsonobject.Gender == 'False') { jsonobject.Gender = false }

        jsonobject.Verification = jsonobject.Verification == 'true' ? true : false;

        //字串化json物件
        const jsonString = JSON.stringify(jsonobject)

        //嘗試fetch
        try {
            const response = await fetch(url_MemberCreate_save, {
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
                $MemCreate.modal('hide');
                new PNotify({
                    title: '新增成功',
                    type: 'success',
                    styling: 'bootstrap3'
                });
                QueryMemberInfo();
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
    $MemCreate.modal('hide') //關掉小窗
    $divCreateInfo.html('') //刪除顯示div
}

//抓到該筆編輯資料的索引
let _index_memdatatable;
$("#memdatatable tbody").on('click', '.indexSelector', function () {

    //要把tr放進去 才能解決換頁索引值出事的問題
    const selectTr = $(this).closest('tr')
    //console.log('新的Row index: ' + $('#memdatatable').DataTable().row(selectTr).index());

    // console.log('Row index: ' + $(this).closest('tr').index());
    _index_memdatatable = $('#memdatatable').DataTable().row(selectTr).index()
    //console.log("抓索引值的事件納編所顯示的會員id" + $("#memdatatable").DataTable().row(_index_memdatatable).data().會員id);
    let memberId = $("#memdatatable").DataTable().row(_index_memdatatable).data().會員id;
    // handleButtonClick(memberId, _index_memdatatable );
});


//-----------------------
//編輯

//抓取小窗div
const $MemEdit = $("#MemEdit")
//選取表單顯示div
let $divEditInfo = $('#divEditInfo')
let _divEditInfo = document.getElementById("divEditInfo")
//選取表單
let $form_memberEdit
let _form_memberEdit

function rebindAll_Edit() {
    //選取表單
    $form_memberEdit = $('#form_MemberEdit')
    _form_memberEdit = document.getElementById('form_MemberEdit')

    //表單動態驗證綁定
    rebind_validate($form_memberEdit)
    //save事件綁定
    buttonEventFunc();
    //switchery綁定
    rebind_switchery(document.getElementById('Verification'))
    //事件綁定 關閉新增會員時
    document.getElementById('editCloseButton').addEventListener('click', function () {
        deleteFormEdit();
    });
}
function deleteFormEdit() {
    $MemEdit.modal('hide') //關掉小窗
    $divEditInfo.html('') //刪除顯示div
}


//顯示編輯小窗
async function handleButtonClick(memberId) {
    try {
        const response = await fetch(`${url_MemberEdit_Form}?memberId=${memberId}`);
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
    _form_memberEdit.addEventListener("submit", async (event) => {
        event.preventDefault(); // 防止表單默認提交行為

        //動態驗證未過，直接停止
        if (!$form_memberEdit.valid()) { return; }

        // memdatatable
        const formData = new FormData(_form_memberEdit);

        let result;
        const Verification = $("#Verification").is(":checked")
        formData.set('Verification', Verification);

        try {
            const response = await fetch(url_MemberEdit_save, {
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

        const currentPage = $('#memdatatable').DataTable().page()
        //只重繪修改的該row
        $('#memdatatable').DataTable().row(_index_memdatatable).data(result);

        $('#memdatatable').DataTable().page(currentPage).draw('page')

    });
}
