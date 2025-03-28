﻿$("#delete-product").click(function () {
	const selectedCheckboxes = Array.from(document.querySelectorAll('input[class="procheck form-check-input"]:checked'));
	var ids = [];
	selectedCheckboxes.forEach(checkbox => {
		ids.push(parseInt(checkbox.value));
	});

	$.ajax({
		url: '/Admin/Product/DeleteCheckedProduct',
		type: 'POST',
		data: {ids:ids},
		success: function (response) {
			toastr.success("ürünler silindi")
			window.location.reload()
		},
		error: function (error) {
			alert('Error: ' + JSON.stringify(error));
			window.location.reload();
		}
	});
});

$("#scrapeProd").click(function () {
	var CategoryId = $("#selectArea .dropdown-content:last").val();
	var ProductName = $("#ProductName").val();
	var data = {
				CategoryId: CategoryId,
				ProductName: ProductName
			   }
	toastr.options = {
			timeOut: 0, // Mesaj kaybolmasın
			extendedTimeOut: 0, // Fare ile üzerine gelinse de kaybolmasın
			closeButton: false, // Kapatma butonu göstermeyin
			tapToDismiss: false // Tıklayınca kapanmasın
		};

	var loadingMessage = toastr.info("Ürünler çekiliyor, lütfen bekleyiniz...");

	$.ajax({
		url: "/Admin/ProductComment/ScrapeProduct",
		type: "POST",
		data: data,
		success: function (response) {
			toastr.clear(loadingMessage);
			toastr.success("ürünler Başarıyla çekildi.")
			window.location.reload();
		},
		error: function (error) {
			toastr.clear(loadingMessage);
			console.log(error);
			toastr.error("ürünler çekilemedi." + error)
				}
			});
});


function GetProductDetail(id) {
	$("#catAdd").css({ display: "none" });
	$("#catUpdate").css({ display: "inline-block" });
	$.ajax({
		url: "/Admin/Category/GetCategoryById",
		type: "POST",
		data: { id: id },
		success: function (response) {
			$("#kt_modal_add_user").modal("show");
			$("#CategoryNames").val(response.name);
			$("#CategoryName").val(response.parentId);
			$("#catID").val(response.id);
		},
		error: function (error) {
			toastr.error("İşlem Başarısız.")
		}
	});
}
function editProduct(param, event) {
	debugger
	event.preventDefault();
	$("#kt_modal_add_user").modal("show");
}

