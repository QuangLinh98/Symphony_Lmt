$(document).ready(function () {
    $("#teacherTable").DataTable({
        "ajax": {
            "url": '/Admin/Teacher/GetTeacher',
            "type": "GET",
            "dataType": "Json"
        },
        "columns": [
            {
                "data": "TeacherID",
                "className": "text-center",
                "width": "1%"
            },
            {
                "data": "FullName",
                "className": "text-center ",
                "width": "20%",
                "render": function (data) {
                    return `<h6>${data}</h6>`;
                }
            },
            { "data": "TeacherCode" },
            {
                "data": "Email",
                "className": "text-center",
                "width": "15%",
            },
            {
                "data": "Phone",
                "className": "text-center",
                "width": "15%",
            },
            {
                "data": "ImagePath",
                "className": "text-center",
                "width": "25%",
                "render": function (data) {
                    return `<img src="/${data}" style="width: 150px; height: 150px;" class="img-thumbnail " />`;
                }
            },
            {
                "data": 'TeacherID',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                                               <a href="/admin/teacher/update/${data}" class="btn btn-secondary mx-2">
                                                   <i class="bi bi-pencil-square"></i>
                                               </a>
                                               <button class="btn btn-danger" onclick="deleteTeacher(${data})">
                                                   <i class="bi bi-trash3"></i>
                                               </button>
                                            </div>`;
                },
                "width": "20%"
            }
        ],
        // Hàm này được gọi sau khi một hàng được tạo và nó được dùng để thêm thuộc tính id vào hàng đó. Mỗi hàng sẽ có một ID duy nhất dưới dạng topicRow_TopicID.
        "createdRow": function (row, data, dataIndex) {
            $(row).attr('id', 'topicRow_' + data.TeacherID);
        }

    })
})