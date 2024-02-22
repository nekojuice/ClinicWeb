const Membersec = document.getElementById("Membersec");
const Attendsec = document.getElementById("Attendsec");
const Appointmentsec = document.getElementById("Appointmentsec");
const Schedulesec = document.getElementById("Schedulesec");
const Casessec = document.getElementById("Casessec");
const Drugssec = document.getElementById("Drugssec");

(async () => {
    const response = await fetch(`/Employee/Main/Profile`);
    const data = await response.json();
    var emp = data.EmpType
    document.getElementById('EmpType').innerText = emp;
    document.getElementById('staffname').innerText = data.StaffName;
    if ("不能管Membersec Attendsec :護士 醫生 藥師".includes(emp)) {
        Membersec.style.display = "none";
        Attendsec.style.display = "none";
    }
    if ("不能管Appointmentsec:藥師".includes(emp)) {
        Appointmentsec.style.display = "none";
    }
    if ("不能管Schedulesec: 醫生 藥師".includes(emp)) {
        Schedulesec.style.display = "none";
    }
    if ("不能管Schedulesec: 行政 藥師".includes(emp)) {
        Casessec.style.display = "none";
    }

    if ("不能管Schedulesec: 行政 ".includes(emp)) {
        Drugssec.style.display = "none";
    }

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

