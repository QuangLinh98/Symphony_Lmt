$(document).ready(function () {
    $("#fAQTable").DataTable({
        "ajax": {
            "url": '/Admin/FAQ/GetFAQ',
            "type": "GET",
            "dataType": "Json"
        },
        "columns": [
            { "data": "FAQID"},
            { "data": "Question"},
            {
                "data": "Answer",
                "render": function (data, type, row) {
                    var truncatedAnswer = data.length > 1000 ? data.substring(0, 1000) + "..." : data;
                    var fullAnswer = data;
                    var faqId = row.FAQID;
                    return `
                        <div class="description-container" id="answer_${faqId}">
                            ${truncatedAnswer}
                        </div>
                        <button class="btn btn-link p-0" onclick="showFullAnswer(${faqId}, '${fullAnswer}')">Read more</button>
                    `;
                } },
            {
                "data": 'FAQID',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                                               <a href="/admin/FAQ/update/${data}" class="btn btn-secondary mx-2">
                                                   <i class="bi bi-pencil-square"></i> Edit
                                               </a>
                                               <button class="btn btn-danger" onclick="deleteFAQ(${data})">
                                                   <i class="bi bi-trash3"></i> Delete
                                               </button>
                                            </div>`;
                },
                "width": "20%"
            }
        ],
        // Hàm này được gọi sau khi một hàng được tạo và nó được dùng để thêm thuộc tính id vào hàng đó. Mỗi hàng sẽ có một ID duy nhất dưới dạng topicRow_TopicID.
        "createdRow": function (row, data, dataIndex) {
            $(row).attr('id', 'topicRow_' + data.FAQID);
        }
    })
})