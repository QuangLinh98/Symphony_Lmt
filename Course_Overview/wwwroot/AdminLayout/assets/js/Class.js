$(document).ready(function () {
    $("#classTable").DataTable({
        "ajax": {
            "url": '/Admin/Class/GetClass',
            "type": "GET",
            "dataType": "Json"
        },
        "columns": [
            {
                "data": "ClassID",
                "className": "text-center",
                "width": "1%"
            },
            {
                "data": "ClassName",
                "className": "text-center ",
                "width": "20%",
                "render": function (data) {
                    return `<h6>${data}</h6>`;
                }
            },
            {
                "data": "Teacher.FullName",
                "className": "text-center ",
                "width": "20%",
                "render": function (data) {
                    return `<h6>${data}</h6>`;
                }
            },
            {
                "data": "Fee",
                "className": "text-center ",
                "width": "10%",
            },
            {
                "data": "ClassType",
                "className": "text-center ",
                "width": "20%",            },
            {
                "data": 'ClassID',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                                               <a href="/admin/class/update/${data}" class="btn btn-secondary mx-2">
                                                   <i class="bi bi-pencil-square"></i> Edit
                                               </a>
                                               <button class="btn btn-danger" onclick="deleteClasses(${data})">
                                                   <i class="bi bi-trash3"></i> Delete
                                               </button>
                                            </div>`;
                },
                "width": "20%"
            }
        ],
        // Hàm này được gọi sau khi một hàng được tạo và nó được dùng để thêm thuộc tính id vào hàng đó. Mỗi hàng sẽ có một ID duy nhất dưới dạng topicRow_TopicID.
        "createdRow": function (row, data, dataIndex) {
            $(row).attr('id', 'topicRow_' + data.ClassID);
        }
    })
})