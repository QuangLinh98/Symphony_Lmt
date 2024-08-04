$(document).ready(function () {
    $("#courseTable").DataTable({
        "ajax": {
            "url": "/Admin/Course/GetCourses",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "CourseID", "className": "text-center", "width": "1%" },
            {
                "data": "CourseName",
                "className": "text-center ",
                "width": "10%",
                "render": function (data) {
                    return `<h6>${data}</h6>`;
                }
            },
            {
                "data": "Duration",
                "className": "text-center ",
                "width": "5%",
                "render": function (data) {
                    return `<h6>${data}</h6>`;
                }
            },
            {
                "data": "Level",
                "className": "text-center ",
                "width": "5%",
                "render": function (data) {
                    return `<h6>${data}</h6>`;
                }
            },
            {
                "data": "Fee",
                "className": "text-center ",
                "width": "5%",
                "render": function (data) {
                    return `<h6>${data}</h6>`;
                }
            },
            {
                "data": "EntryRequirements",
                "render": function (data, type, row) {
                    const courseId = row.CourseID;
                    return `
                    <div class="description-container" id="entry_${courseId}">
                       ${data}
                    </div>
                    <button class="btn btn-link p-0" onclick="showFullContent('${courseId}', 'entry')">Read more</button>
                    `;
                }
            },
            {
                "data": "StudyMaterials",
                "render": function (data, type, row) {
                    const courseId = row.CourseID;
                    return `
                    <div class="description-container" id="study_${courseId}">
                       ${data}
                    </div>
                    <button class="btn btn-link p-0" onclick="showFullContent('${courseId}', 'study')">Read more</button>
                    `;
                }
            },
            {
                "data": "LabFacilities",
                "render": function (data, type, row) {
                    const courseId = row.CourseID;
                    return `
                    <div class="description-container" id="lab_${courseId}">
                       ${data}
                    </div>
                    <button class="btn btn-link p-0" onclick="showFullContent('${courseId}', 'lab')">Read more</button>
                    `;
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
                "data": 'CourseID',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                                               <a href="/admin/course/update/${data}" class="btn btn-secondary mx-2">
                                                   <i class="bi bi-pencil-square"></i> Update
                                               </a>   
                                               <a href="/admin/course/Detail/${data}" class="btn btn-primary mx-2" >
                                                   <i class="bi bi-plus-square"></i> Detail
                                               </a>                                             
                                            </div>`;
                },
                "width": "20%"
            }
        ]
    });
});

