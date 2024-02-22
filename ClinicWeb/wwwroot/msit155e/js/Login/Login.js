(async () => {
    const response = await fetch(`/Employee/Main/Profile`);
    const data = await response.json();

    document.getElementById('EmpType').innerText = data.EmpType;
    document.getElementById('staffname').innerText = data.StaffName;
    console.log(document.cookie);

})();

(async () => {
    const response = await fetch(`/Employee/Main/ProfileForPicture`);
    if (response.ok) {
        const blob = await response.blob();
        const imgUrl = URL.createObjectURL(blob);
        document.getElementById('profilePic').src = imgUrl;
    } else {
        console.error('抓大頭照的api怪怪的');
    }
})();