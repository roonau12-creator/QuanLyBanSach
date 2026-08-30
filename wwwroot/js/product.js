var productDataTable;
$(document).ready(function()
{
    productDataTable();
})
productDataTable=$("#tblData").DataTable({
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

function Delete(url)
{
    Swal.fire({
  title: "Are you sure?",
  text: "You won't be able to revert this!",
  icon: "warning",
  showCancelButton: true,
  confirmButtonColor: "#3085d6",
  cancelButtonColor: "#d33",
  confirmButtonText: "Yes, delete it!"
}).then((result) => {
  if (result.isConfirmed) 
  {
    $.ajax({
        url:url,
        type:'DELETE',
        success:function(data)
        {
            productDataTable.ajax.reload();
            Swal.fire({
            title: "Deleted!",
            text: "Your file has been deleted.",
            icon: "success"
  });
        }
    })
    
    }
});
}