(async () => {
    const response = await fetch("/Controllers/ScheduleAPI/WeekScheduleController/Get_WeekSchedule/小兒科")
    const data = await response.json()
    

    //週迴圈
    for (let i = 0; i < 7; i++) {
        if (!data.hasOwnProperty(i)) {
            continue
        }
        tableBuilder(data, i, '早診', 'morning', 'department1')
        tableBuilder(data, i, '午診', 'noon', 'department1')
        tableBuilder(data, i, '晚診', 'night', 'department1')
    }

})();

(async () => {
    const response = await fetch("/Controllers/ScheduleAPI/WeekScheduleController/Get_WeekSchedule/婦產科")
    const data = await response.json()

    //週迴圈
    for (let i = 0; i < 7; i++) {
        if (!data.hasOwnProperty(i)) {
            continue
        }
        tableBuilder(data, i, '早診', 'morning', 'department2')
        tableBuilder(data, i, '午診', 'noon', 'department2')
        tableBuilder(data, i, '晚診', 'night', 'department2')
    }

})();
function cardBuilder(doctor, room) {
    return `<div style="border: dashed;"><h5>${doctor}</h5><h6>${room}</h6></div>`
}
function tableBuilder(data, index, shiftChinese, shift_tr_id, department_tbody_id) {
    if (data[index].hasOwnProperty(shiftChinese)) {
        let dataArray = data[index][shiftChinese]
        let dataCards = dataArray.length > 1 ? dataArray.map(x => { return cardBuilder(x.doctor, x.room) }).join("") : cardBuilder(dataArray[0].doctor, dataArray[0].room)
        $(`tbody#${department_tbody_id} tr#${shift_tr_id} td:nth-child(${index + 2})`).html(dataCards)
    }
}