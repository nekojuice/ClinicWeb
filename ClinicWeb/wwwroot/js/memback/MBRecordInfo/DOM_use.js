var myModal = new bootstrap.Modal(document.getElementById('exampleModal'), { //找到modal
    keyboard: false
})
var close = document.getElementById('close'); //關閉按鈕
close.addEventListener('click', function () {
    myModal.hide();
    $('#prescriptionListDataTable').DataTable().clear().draw();
});