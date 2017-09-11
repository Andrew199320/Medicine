$(document).ready(function () {
    getCategories();
});

function hidePopup() {
    $('.popup').removeClass("active");
}
function showPopup() {
    $('.popup').addClass("active");
}
$('.createCategory').click(function () {
    $('.popup').addClass("active");
});
$('.create').click(function () {
    createCategory();
});
$('.cancel').click(function () {
    $('.popup').removeClass("active");
});

function getCategories() {
    $.ajax({
        url: '/Category/GetCategories',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            var rows = "";
            $.each(data, function (i, item) {
                rows += "<tr>";
                rows += "<td>" + item.IdCategory + "</td>";
                rows += "<td>" + item.Name + "</td>";
                rows += "<td>" + item.Description + "</td>";
                rows += "<td><button type='button' id='btnEdit' class='btn btn-default' onclick='return getCategoryById(" + item.IdCategory + ")'>Edit</button> <button type='button' id='btnDelete' class='btn btn-danger' onclick='return deleteProductById(" + item.IdCategory + ")'>Delete</button></td>"
                rows += "</tr>";
                $('.categories-list tbody').html(rows);
            });
        },
        error: function (err) {
            alert("Error");
        }
    });
}
function getCategoryById(id) {
    $.ajax({
        url: "/Category/GetCategoryById/" + id,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $('#IdCategory').val(data.IdCategory),
            $('#Name').val(data.Name),
            $('#Description').val(data.Description)
            showPopup();
        },
        error:function(err)
        {
            alert("Something is wrong");
        }
    });
}

function createCategory() {
    var data = {
        IdCategory: $('#IdCategory').val(),
        Name: $('#Name').val(),
        Description: $('#Description').val()
    }
    $.ajax({
        url: '/Category/CreateCategory',
        type: 'POST',
        dataType: 'json',
        data: data,
        success: function (data) {
            getCategories();
            clear();
            hidePopup();
        },
        error: function (err) {
            alert("Category hasn't been added");
        }
    });
}

function clear() {
    $("#IdCategory").val("");
    $("#Name").val("");
    $("#Description").val("");
}





