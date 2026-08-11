$("#tblData").DataTable({
    ajax:"/Admin/Product/GetAll",
        columns: [
            { data: "title", "width": "15%"},
            { data: "isbn", "width": "15%"},
            { data: "price", "width": "10%", render: function (data) {
                return '$' + data.toFixed(2);
            }},
            { data: "author", "width": "20%"},
            { data: "category.name", "width": "20%", render: function (data) {
                return '<span class="badge bg-primary">' + data + '</span>';
            }},
            {data:"id","width":"20%", render: function (data) {
                return `<div class="text-end">
                            <a href="/Admin/Product/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>
                            <a onclick=Delete("/Admin/Product/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                <i class="bi bi-trash-fill"></i> Delete
                            </a>
                        </div>`;
            }
        }

        ]
});