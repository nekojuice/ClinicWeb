(async () => {
    const response = await fetch("WeekSchedule/Get_evaluation");
    const datas = await response.json();
    console.log(datas);


})();
