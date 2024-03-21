const Membersec = document.getElementsByClassName("Membersec");
const Attendsec = document.getElementsByClassName("Attendsec");
const Appointmentsec = document.getElementsByClassName("Appointmentsec");
const Schedulesec = document.getElementsByClassName("Schedulesec");
const Casessec = document.getElementsByClassName("Casessec");
const Drugssec = document.getElementsByClassName("Drugssec");

(async () => {
    const response = await fetch(`/Employee/Main/ProfileforStaff`);
    const data = await response.json();
    var emp = data.EmpType
    document.getElementById('EmpType').innerText = emp;
    document.getElementById('staffname').innerText = data.StaffName;
    document.getElementById('staffnameForLogout').innerText = data.StaffName;
    if ("不能管Membersec Attendsec :護士 醫生 藥師".includes(emp)) {
        Membersec[0].style.display = "none";
        Membersec[1].style.display = "none";
        Attendsec[0].style.display = "none";
        Attendsec[1].style.display = "none";
    }
    if ("不能管Appointmentsec:藥師".includes(emp)) {
        Appointmentsec[0].style.display = "none";
        Appointmentsec[1].style.display = "none";
    }
    if ("不能管Schedulesec: 醫生 藥師".includes(emp)) {
        Schedulesec[0].style.display = "none";
        Schedulesec[1].style.display = "none";
    }
    if ("不能管Casessec: 行政 藥師".includes(emp)) {
        Casessec[0].style.display = "none";
        Casessec[1].style.display = "none";
    }

    if ("不能管Drugssec: 行政 ".includes(emp)) {
        Drugssec[0].style.display = "none";
        Drugssec[1].style.display = "none";
    }

})();

(async () => {

    try {
        const response = await fetch(`/Employee/Main/ProfileForPicture`);
        if (!response.ok) {
            throw new Error(`${response.status}`);
            return;
        }

        const blob = await response.blob();
        if (blob.type != "image/jpeg") {
            return;
        }
        const imgUrl = URL.createObjectURL(blob);
        document.getElementById('profilePic').src = imgUrl;
        document.getElementById('profilePicLogout').src = imgUrl;
    }
    catch (error) {
        console.log(error);
    }

})();

