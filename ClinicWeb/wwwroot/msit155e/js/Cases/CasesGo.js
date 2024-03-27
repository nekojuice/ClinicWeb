var mytable = $('#clinicDataTable').DataTable({
	ajax: {
		type: 'GET',
		url: "/Cases/Home/maindata",
		dataSrc: function (json) { return json; }
	},
	columns: [
		{ "data": "id", "visible": false },
		{ "data": "姓名" },
		{ "data": "身分證字號" },
		{ "data": "初診日期" },
		{
			data: null, title: "操作功能",  // 這邊是欄位
			render: function (data, type, row) {
				return '<button id="Regist" type="button" class="btn btn-warning btn-sm"><i class="bi bi-pencil-square"></i></button> ' +
					'<button id="Delete" type="button" class="btn btn-danger btn-sm"><i class="bi bi-trash"></i></button>'
			}
		},
	],
	fixedHeader: {
		header: true
	},
	language: {
		url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
	}
});
$('#clinicDataTable tbody').on('click', 'tr', function () {
	var data = mytable.row(this).data();
	console.log(data);
	alert('You clicked on ' + data["姓名"] + "'s row");
});

