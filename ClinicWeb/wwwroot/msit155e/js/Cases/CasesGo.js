$(document).ready(function () {
	$('#clinicDataTable').dataTable({
		ajax: {
			type: 'GET',
			url: "/Cases/Home/maindata",
			dataSrc: function (json) { return json; }
		},
		columns: [
			{ "data": "id", "visible": false },
			{ "data": "姓名" },
			{ "data": "身分證字號" },
			{ "data": "性別" },
		],
		fixedHeader: {
			header: true
		},
		language: {
			url: "https://cdn.datatables.net/plug-ins/1.13.7/i18n/zh-HANT.json"
		}
	});
});