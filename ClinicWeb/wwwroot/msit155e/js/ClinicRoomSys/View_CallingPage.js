"use strict";

let connection = new signalR.HubConnectionBuilder()
    .withUrl("/CallingHub")
    .withAutomaticReconnect()
    .build();

//Disable the send button until connection is established.
//document.getElementById("sendButton").disabled = true;

//新醫生登入事件
connection.on("Listener_ClinicInfo", function (jsonstring) {
    const jsondata = JSON.parse(jsonstring)
    console.log(jsondata)
    //清空
    for (var i = 0; i < 4; i++) {
        $(`#cp-${i}`).find('.cp-dept').text('')
        $(`#cp-${i}`).find('.cp-room').text('')
        $(`#cp-${i}`).find('.cp-doc').text('')
        $(`#cp-${i}`).find('.cp-shift').text('')
        $(`#cp-${i}`).find('.cp-num').text('')
    }
    //填入
    for (var i = 0; i < jsondata.length; i++) {
        $(`#cp-${i}`).find('.cp-dept').text(jsondata[i].department)
        $(`#cp-${i}`).find('.cp-room').text(jsondata[i].room)
        $(`#cp-${i}`).find('.cp-doc').text(jsondata[i].doctorName)
        $(`#cp-${i}`).find('.cp-shift').text(jsondata[i].shift)
        $(`#cp-${i}`).find('.cp-num').text(jsondata[i].number)
    }
});


connection.start().then(function () {
    try {
        connection.invoke("Get_ClinicInfo");
    } catch (err) {
        console.error(err);
    }
}).catch(function (err) {
    return console.error(err.toString());
});