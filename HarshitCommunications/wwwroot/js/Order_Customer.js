var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": '/Customer/home/getall?status='+status
        },
        "order": [[3, "desc"]],
        "columns": [
            { "data": "name", "width": "20%" },
            { "data": "phoneNumber", "width": "10%" },
            { "data": "applicationUser.email", "width": "20%" },
            {
                "data": "orderDate",
                "render": function (data) {
                    // Convert raw date to desired format (YYYY-MM-DD)
                    const date = new Date(data);
                    const formattedDate = date.toLocaleDateString('hi-IN');
                    return formattedDate;
                },
                "width": "15%"
            },
            { "data": "orderStatus", "width": "10%" },
            { "data": "orderTotal", "width": "5%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="action-buttons">
                        <a href="/customer/home/myorder?orderId=${data}" class="btn btn-primary"><i class="bi bi-info-circle"></i></i></a>
                    </div>`
                },
                "width": "5%"
            },
        ],
        "responsive": true
    });
}