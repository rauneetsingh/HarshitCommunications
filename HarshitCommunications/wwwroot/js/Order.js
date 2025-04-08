var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    let status = "all"; // Default to "all"
    if (url.includes("inprocess")) status = "inprocess";
    else if (url.includes("completed")) status = "completed";
    else if (url.includes("pending")) status = "pending";
    else if (url.includes("approved")) status = "approved";
    else if (url.includes("refunded")) status = "refunded";

    console.log("Loading DataTable with status: " + status);
    loadDataTable(status);
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": '/Admin/order/getall?status='+status
        },
        "order": [[3, "desc"]],
        "columns": [
            { "data": "name", "width": "25%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "applicationUser.email", "width": "25%" },
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
            { "data": "orderTotal", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="action-buttons">
                        <a href="/admin/order/details?orderId=${data}" class="btn btn-primary"> <i class="bi bi-pencil-square"></i></a>
                    </div>`
                },
                "width": "10%"
            },
        ]
    });
}