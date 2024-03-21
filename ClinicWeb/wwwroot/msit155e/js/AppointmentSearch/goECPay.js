$("#checkoutBtn").on('click', async (e) => {
    //e.preventDefault(); //因為送出就跳轉到綠界，這個可以停住確認自己的console.log的內容
    let formData = $("#form").serializeArray();
    var json = {};
    $.each(formData, function () {
        json[this.name] = this.value || "";
    });
    console.log(json);
    const jsonString = JSON.stringify(json)
    //step3 : 新增訂單到資料庫
    //$.ajax({
    //    type: 'POST',
    //    url: 'https://localhost:7071/Ecpay/AddOrders',
    //    contentType: 'application/json; charset=utf-8',
    //    data: JSON.stringify(json),
    //    success: function (res) {
    //        console.log(res);
    //    },
    //    error: function (err) { console.log(err); },
    //});
    const response = await fetch('https://localhost:7071/Ecpay/AddOrders',
        {
            'method': 'POST',
            'headers': {
                'Content-Type': 'application/json;charset=UTF-8'
            },
            'body': jsonString
        });
    const msg = await response.text()
    console.log(msg)

});