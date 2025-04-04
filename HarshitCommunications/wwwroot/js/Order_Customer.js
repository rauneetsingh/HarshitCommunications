var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": '/Customer/home/getall?status='+status
        },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "name", "width": "20%" },
            { "data": "phoneNumber", "width": "10%" },
            { "data": "applicationUser.email", "width": "20%" },
            { "data": "orderStatus", "width": "10%" },
            { "data": "orderTotal", "width": "5%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="action-buttons">
                        <a href="/customer/home/myorder?orderId=${data}" class="btn btn-primary"> <i class="bi bi-pencil-square"></i></a>
                    </div>`
                },
                "width": "5%"
            },
        ]
    });
}