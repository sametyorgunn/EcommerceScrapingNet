function filterFunctionCategory() {
    const input = document.getElementById("myInputCategory");
    const filter = input.value.toUpperCase();
    const select = document.getElementById("CategoryName");
    const options = select.getElementsByTagName("option");

    for (let i = 0; i < options.length; i++) {
        const txtValue = options[i].textContent || options[i].innerText;
        if (txtValue.toUpperCase().indexOf(filter) > -1) {
            options[i].style.display = "";
        } else {
            options[i].style.display = "none";
        }
    }
}
$("#catAdd").click(function () {
	var CategoryName = $("#CategoryNames").val();
	var CategoryID = $("#CategoryName").val();
	var data = {
		CategoryName: CategoryName,
		CategoryId: CategoryID
	}
	$.ajax({
		url: "/Admin/Category/AddCategory",
		type: "POST",
		data: data,
		success: function (response) {
			toastr.success("Kategori Eklendi.")
			setTimeout(function () {
				window.location.reload();
			}, 1000);
		},
		error: function (error) {
			toastr.error("İşlem Başarısız.")
		}
	});
});