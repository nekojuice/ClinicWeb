//宣告重新綁定switchery方法, _selector = input, checkbox
function rebind_switchery(_selector) {
    let switcheryOptions = {
        color: '#64bd63', // 當checked時的背景顏色
        secondaryColor: '#dfdfdf', // unchecked時的背景顏色
        jackColor: '#fff', // checked時的按鈕顏色
        jackSecondaryColor: null, // uncheck時的按鈕顏色
        className: 'switchery', // class名稱
        disabled: false, // 是否設為disabled
        disabledOpacity: 0.5, // 當設為disabled時的透明度
        speed: '0.1s', // 開關切換的速度
        size: 'default' // 開關大小(small, default, large)
    };
    new Switchery(_selector, switcheryOptions);
}


//重新綁定create表單的驗證, $selector = Form
function rebind_validate($selector) {
    //重新綁定要驗證的表單 為了把編輯的表單跟validation連起來 (因為資料室ajax 過來 validation在資料來之前就在了)
    $selector.removeData('validator');
    $selector.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse($selector);
}

//清除表單內容, _selector = form
// 關閉新增會員的表單後要清上面的資料 一定要放在所有新增相關動作的後面
function clearForm(_selector) {
    //清除輸入的東西
    _selector.reset();
    //清除出現的警告訊息
    $('.text-danger').text('');
}

// 觸發圖片上傳
function triggerImageUpload(cardSelector, uploadInputId) {
    document.querySelector(cardSelector).addEventListener('click', function () {
        document.getElementById(uploadInputId).click();
    });
}

// 處理圖片檔案選擇和顯示預覽
function handleImageUpload(uploadInputId, previewImageId) {
    var inputElement = document.getElementById(uploadInputId);
    var previewImage = document.getElementById(previewImageId);

    inputElement.addEventListener('change', function () {
        // 一般圖片上傳
        if (this.files && this.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                previewImage.src = e.target.result;
                previewImage.style.display = 'block'; 
            };
            reader.readAsDataURL(this.files[0]);
        }
    });

    // Blob
    this.previewBlob = function (blob) {
        var reader = new FileReader();
        reader.onload = function (e) {
            previewImage.src = e.target.result;
            previewImage.style.display = 'block'; 
        };
        reader.readAsDataURL(blob);
    };
}