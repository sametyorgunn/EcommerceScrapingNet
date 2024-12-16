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
	var CategoryID = $("#selectArea .dropdown-content:last").val();
	if (CategoryID == null) {
		CategoryID = $("#CategoryName").val();
	}
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

function GetCategoryDetail(id) {
	$("#catAdd").css({ display: "none" });
	$("#catUpdate").css({ display: "inline-block" });
	$.ajax({
		url: "/Admin/Category/GetCategoryById",
		type: "POST",
		data: {id:id},
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

$("#catUpdate").click(function () {
	var CategoryId = $("#catID").val();
	var CategoryName = $("#CategoryNames").val();
	var CategoryID = $("#CategoryName").val();
	var data = {
		Name: CategoryName,
		ParentId: CategoryID,
		Id: CategoryId,
	}
	$.ajax({
		url: "/Admin/Category/UpdateCategory",
		type: "POST",
		data: data,
		success: function (response) {
			toastr.success("Kategori Güncellendi.")
			setTimeout(function () {
				window.location.reload();
			}, 1000);
		},
		error: function (error) {
			toastr.error("İşlem Başarısız.")
		}
	});
});

$("#categoryUpdate").click(function () {
	$("#kt_modal_add_user").modal("show");
	$("#catID").val(0);
	$("#CategoryNames").val("");
	$("#CategoryName").val(0);
});

$(document).ready(function () {
	$(document).on("change", ".dropdown-content", function () {
		var selectedCategoryId = $(this).val();
		$.ajax({
			url: "/Admin/Category/GetSubCategories",
			type: "GET",
			data: { categoryId: selectedCategoryId },
			success: function (response) {
				if (response && response.length > 0) {
					//var subCategorySelect = $("<select>", {
					//	id: "CategoryName_" + selectedCategoryId,
					//	name: "CategoryId",
					//	class: "dropdown-content form-control",
					//});
					var select = `
                    <select name="CategoryId" class="dropdown-content form-control" id="CategoryName_${selectedCategoryId}">
                        <option value="" disabled selected>Bir seçenek seçin</option>
                        ${response.map(subCategory => `<option value="${subCategory.id}">${subCategory.name}</option>`).join('')}
                    </select></br>
                `;


					//subCategorySelect.append($("<option>", {
					//	value: "",
					//	text: "Bir seçenek seçin",
					//	disabled: true,
					//	selected: true
					//}));
					//response.forEach(function (subCategory) {
					//	subCategorySelect.append($("<option>", {
					//		value: subCategory.id,
					//		text: subCategory.name
					//	}));
					//});

					//$("#selectArea").append(subCategorySelect);
					$("#selectArea").append(select);

				} else {
					toastr.warning("Alt kategori bulunamadı.");
				//	$("#selectArea").empty();
				}
			},
			error: function () {
				toastr.error("İşlem başarısız oldu.");
			}
		});
	});
});

$("#n11cat").click(function () {
	$.ajax({
		url: "/Admin/Category/N11CategoryUpdate",
		type: "GET",
		success: function (response) {
			toastr.success("güncelleniyor");
		},
		error: function (error) {
			toastr.error("İşlem Başarısız.");
		}
	});
})

$("#trendcat").click(function () {
	$.ajax({
		url: "/Admin/Category/TrendyolCategoryUpdate",
		type: "GET",
		success: function (response) {
			toastr.success("güncelleniyor");
		},
		error: function (error) {
			toastr.error("İşlem Başarısız.");
		}
	});
})

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

