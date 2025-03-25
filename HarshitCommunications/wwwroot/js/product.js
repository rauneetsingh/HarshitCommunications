
var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Product/getall",
        },
        "columns": [
            { "data": "name", "width": "10%" },
            { "data": "shortDesc", "width": "25%" },
            {
                "data": "description",
                "render": function (data) {
                    return data ? `<div>${data}</div>` : "No Description";
                },
                "width": "30%"
            },
            { "data": "category.name", "width": "5%" },
            { "data": "price", "width": "5%" },
            {
                "data": "imageUrl",
                "render": function (data) {
                    return data ? `<img src="${data}" width="100" height="100" />` : "No Image";
                },
                "width": "5%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="action-buttons">
                        <a href="/admin/product/upsert?id=${data}" class="btn btn-primary"> <i class="bi bi-pencil-square"></i> Edit </a>
                        <a onClick = Delete('/admin/product/delete/${data}') class="btn btn-danger"> <i class="bi bi-trash-fill"></i> Delete </a>
                    </div>`
                },
                "width": "15%"
            },
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message)
                }
            })
        }
    });
}
