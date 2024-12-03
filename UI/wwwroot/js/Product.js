$("#delete-product").click(function () {
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
			toastr("ürünler silindi");
			window.location.reload();
		},
		error: function (error) {
			alert('Error: ' + JSON.stringify(error));
			window.location.reload();
		}
	});
});

$("#scrapeProd").click(function () {
	var CategoryId = $("#selectArea .dropdown-content:last").val();

	//var CategoryId = $("#CategoryName").val();
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
			// if (response.status == "True") {
			toastr.clear(loadingMessage);
		toastr.success("ürünler Başarıyla çekildi.")
		window.location.reload();
			//            } else {
			// toastr.clear(loadingMessage);
			// toastr.error("ürünler çekilemedi.")
			//            }
		},
		error: function (error) {
			toastr.clear(loadingMessage);
			console.log(error);
			toastr.error("ürünler çekilemedi.")
				}
			});
});

$(document).ready(function () {
	$("#trendBtn").prop("disabled", true);
	$("#amazonBtn").prop("disabled", true);
});

$("#n11Btn").on("click", function (event) {
	event.preventDefault();
	$("#n11Btn").prop("disabled", true);
	$("#trendBtn").prop("disabled", false);
});
$("#trendBtn").on("click", function (event) {
	event.preventDefault();
	$("#trendBtn").prop("disabled", true);
	$("#amazonBtn").prop("disabled", false);
});