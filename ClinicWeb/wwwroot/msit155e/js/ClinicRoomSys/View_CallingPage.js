"use strict";

const callingSound = document.getElementById('callingSound');

let connection = new signalR.HubConnectionBuilder()
    .withUrl("/CallingHub")
    .withAutomaticReconnect()
    .build();

//Disable the send button until connection is established.
//document.getElementById("sendButton").disabled = true;

//新醫生登入事件
let isCallintArray = [];
let lastCallingIndex = -1;
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
    isCallintArray = [] //清空陣列
    for (var i = 0; i < jsondata.length; i++) {
        $(`#cp-${i}`).find('.cp-dept').text(jsondata[i].department)
        $(`#cp-${i}`).find('.cp-room').text(jsondata[i].room)
        $(`#cp-${i}`).find('.cp-doc').text(jsondata[i].doctorName)
        $(`#cp-${i}`).find('.cp-shift').text(jsondata[i].shift)
        $(`#cp-${i}`).find('.cp-num').text(jsondata[i].number)
        //撈出叫號者
        isCallintArray.push(jsondata[i].isCalling)
    }
    //尋找最大值(正在叫號的index)
    let callingIndex = isCallintArray.indexOf(Math.max(...isCallintArray));
    console.log(callingIndex)
    if (callingIndex < 0 && lastCallingIndex == callingIndex) { return; }
    callingSound.play()
    lastCallingIndex = callingIndex
    $(`#cp-${callingIndex}`).find('.cp-num').fadeOut(500).fadeIn(500).fadeOut(500).fadeIn(500);
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