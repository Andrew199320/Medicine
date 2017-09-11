$(document).ready(function () {
    getCategories();
})

function getCategories()
{
    $.ajax({
        url: '/Category/GetCategories',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            var rows = '';
            $.each(data, function (i, item) {
                rows += "<tr>";
                rows += "<td>" + item.IdCategory + "</td>";
                rows += "<td>" + item.Name + "</td>";
                rows += "<td>" + item.Description + "</td>";
                rows += "<td><button type='button' id='btnEdit' class='btn btn-default' onclick='return getProductById(" + item.IdCategory + ")'>Edit</button> <button type='button' id='btnDelete' class='btn red' onclick='return deleteProductById(" + item.Id + ")'>Delete</button></td>"
                rows += "</tr>";
                $("#list-categories tbody").html(rows);
            });
        },
        error: function (err) {
            alert("Error");
        }
    });
}

$(".createNewCategory").click(function () {
    $(".popup").addClass('active');
});

$('.save').click(function ()
{
    createNewCategoty();
});
$('.close').click(function () {
    $(".popup").removeClass('active');
});

function createNewCategoty() {
    var data = {
        IdCategory:$("#IdCategory").val(),
        Name: $('#Name').val(),
        Description: $('#Description').val()
    }
    $.ajax({
        url:'/Category/CreateCategory',
        type: 'POST',
        dataType:'json',
        data:data,
        success:function(data)
        {
            getCategories();
            $(".popup").removeClass('active');
        },
        error: function(err){
            alert("Error");
        }
    })
}




var isUpdateable = false;

function getProductById(id) {
    $("#title").text("Product Detail");
    $.ajax({
        url: '/Category/GetCategoryById/' + id,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $("#Id").val(data.IdCategory);
            $("#Name").val(data.Name);
            $("#Description").val(data.Description);
            isUpdateable = true;
            $("#productModal").modal('show');
        },
        error: function (err) {
            alert("Error: " + err.responseText);
        }
    });
}

$("#btnSave").click(function (e) {

    var data = {
        Id: $("#Id").val(),
        Name: $("#Name").val(),
        Price: $("#Description").val()
    }

    if (!isUpdateable) {
        $.ajax({
            url: '/Category/CreateCategory/',
            type: 'POST',
            dataType: 'json',
            data: data,
            success: function (data) {
                getCategories();
                $("#productModal").modal('hide');
                clear();
            },
            error: function (err) {
                alert("Error: " + err.responseText);
            }
        })
    }
    else {
        $.ajax({
            url: '/Category/EditCategory/',
            type: 'POST',
            dataType: 'json',
            data: data,
            success: function (data) {
                getCategories();
                isUpdateable = false;
                $("#productModal").modal('hide');
                clear();
            },
            error: function (err) {
                alert("Error: " + err.responseText);
            }
        })
    }
});


function deleteProductById(id) {
    $("#confirmModal #title").text("Delete Product");
    $("#confirmModal").modal('show');
    $("#confirmModal #btnOk").click(function (e) {
        $.ajax({
            url: "/Product/Delete/" + id,
            type: "POST",
            dataType: 'json',
            success: function (data) {
                getProducts();
                $("#confirmModal").modal('hide');
            },
            error: function (err) {
                alert("Error: " + err.responseText);
            }
        });

        e.preventDefault();
    });
}

// Set title for create new
$("#btnCreate").click(function () {
    $("#title").text("Create New");
})

// Close modal
$("#btnClose").click(function () {
    clear();
});

// Clear all items
function clear() {
    $("#Id").val("");
    $("#Name").val("");
    $("#Price").val("");
}