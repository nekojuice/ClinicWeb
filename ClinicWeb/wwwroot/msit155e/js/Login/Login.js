const Membersec = document.getElementById("Membersec");
const Attendsec = document.getElementById("Attendsec");
const Appointmentsec = document.getElementById("Appointmentsec");
const Schedulesec = document.getElementById("Schedulesec");
const Casessec = document.getElementById("Casessec");
const Drugssec = document.getElementById("Drugssec");

(async () => {
    const response = await fetch(`/Employee/Main/ProfileforStaff`);
    const data = await response.json();
    var emp = data.EmpType
    document.getElementById('EmpType').innerText = emp;
    document.getElementById('staffname').innerText = data.StaffName;
    document.getElementById('staffnameForLogout').innerText = data.StaffName;
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
    if ("不能管Casessec: 行政 藥師".includes(emp)) {
        Casessec.style.display = "none";
    }

    if ("不能管Drugssec: 行政 ".includes(emp)) {
        Drugssec.style.display = "none";
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


