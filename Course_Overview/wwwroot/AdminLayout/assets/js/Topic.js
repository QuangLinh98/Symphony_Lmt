$(document).ready(function () {
    $("#topicTable").DataTable({
        "ajax": {
            "url": '/Admin/Topic/GetTopic',
            "type": "GET",
            "dataType": "Json"
        },
        "columns": [
            { "data": "TopicID" },
            {
                "data": "TopicName",
                "className": "text-center ",
                "width": "25%",
                "render": function (data) {
                    return `<h6>${data}</h6>`;
                }
            },
            {
                "data": "Course.CourseName",
                "className": "text-center ",
                "width": "25%",
                "render": function (data) {
                    return `<h6>${data}</h6>`;
                }
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
                "data": 'TopicID',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                                               <a href="/admin/topic/update/${data}" class="btn btn-secondary mx-2">
                                                   <i class="bi bi-pencil-square"></i> Edit
                                               </a>
                                               <button class="btn btn-danger" onclick="deleteTopic(${data})">
                                                   <i class="bi bi-trash3"></i> Delete
                                               </button>
                                            </div>`;
                },
                "width": "20%"
            }
        ],

        // Hàm này được gọi sau khi một hàng được tạo và nó được dùng để thêm thuộc tính id vào hàng đó. Mỗi hàng sẽ có một ID duy nhất dưới dạng topicRow_TopicID.
        "createdRow": function (row, data, dataIndex) {
            $(row).attr('id', 'topicRow_' + data.TopicID);
        }
    })
})