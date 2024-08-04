function deleteTopic(topicID) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Admin/Topic/Delete/' + topicID, // Replace with your actual URL
                type: 'POST',
                success: function (response) {
                    // Xóa dòng trên table khi xóa thành công
                    $('#topicRow_' + topicID).remove();

                    // Hiển thị thông báo thành công
                    Swal.fire({
                        icon: "success",
                        title: "Delete item successfully!",
                        timer: 1500
                    });
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error("Error deleting item:", textStatus, errorThrown);
                    Swal.fire({
                        icon: "error",
                        title: "Delete item failed",
                        text: "Please try again.",
                        timer: 1500
                    });
                }
            });
        }
    });
}


function deleteTeacher(teacherID) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Admin/Teacher/Delete/' + teacherID, // Replace with your actual URL
                type: 'POST',
                success: function (response) {
                    // Xóa dòng trên table khi xóa thành công
                    $('#topicRow_' + teacherID).remove();

                    // Hiển thị thông báo thành công
                    Swal.fire({
                        icon: "success",
                        title: "Delete item successfully!",
                        timer: 1500
                    });
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error("Error deleting item:", textStatus, errorThrown);
                    Swal.fire({
                        icon: "error",
                        title: "Delete item failed",
                        text: "Please try again.",
                        timer: 1500
                    });
                }
            });
        }
    });
}

function deleteClasses(classID) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Admin/Class/Delete/' + classID, // Replace with your actual URL
                type: 'POST',
                success: function (response) {
                    // Xóa dòng trên table khi xóa thành công
                    $('#topicRow_' + classID).remove();

                    // Hiển thị thông báo thành công
                    Swal.fire({
                        icon: "success",
                        title: "Delete item successfully!",
                        timer: 1500
                    });
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error("Error deleting item:", textStatus, errorThrown);
                    Swal.fire({
                        icon: "error",
                        title: "Delete item failed",
                        text: "Please try again.",
                        timer: 1500
                    });
                }
            });
        }
    });
}

function deleteSlider(sliderID) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Admin/Slider/Delete/' + sliderID, // Replace with your actual URL
                type: 'POST',
                success: function (response) {
                    // Xóa dòng trên table khi xóa thành công
                    $('#topicRow_' + sliderID).remove();

                    // Hiển thị thông báo thành công
                    Swal.fire({
                        icon: "success",
                        title: "Delete item successfully!",
                        timer: 1500
                    });
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error("Error deleting item:", textStatus, errorThrown);
                    Swal.fire({
                        icon: "error",
                        title: "Delete item failed",
                        text: "Please try again.",
                        timer: 1500
                    });
                }
            });
        }
    });
}

function deleteService(serviceID) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Admin/Slider/Delete/' + serviceID, // Replace with your actual URL
                type: 'POST',
                success: function (response) {
                    // Xóa dòng trên table khi xóa thành công
                    $('#topicRow_' + serviceID).remove();

                    // Hiển thị thông báo thành công
                    Swal.fire({
                        icon: "success",
                        title: "Delete item successfully!",
                        timer: 1500
                    });
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error("Error deleting item:", textStatus, errorThrown);
                    Swal.fire({
                        icon: "error",
                        title: "Delete item failed",
                        text: "Please try again.",
                        timer: 1500
                    });
                }
            });
        }
    });
}

function deleteStudent(studentID) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Admin/Student/Delete/' + studentID, // Replace with your actual URL
                type: 'POST',
                success: function (response) {
                    // Xóa dòng trên table khi xóa thành công
                    $('#topicRow_' + studentID).remove();

                    // Hiển thị thông báo thành công
                    Swal.fire({
                        icon: "success",
                        title: "Delete item successfully!",
                        timer: 1500
                    });
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error("Error deleting item:", textStatus, errorThrown);
                    Swal.fire({
                        icon: "error",
                        title: "Delete item failed",
                        text: "Please try again.",
                        timer: 1500
                    });
                }
            });
        }
    });
}

function deleteFAQ(fAQID) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Admin/FAQ/Delete/' + fAQID, // Replace with your actual URL
                type: 'POST',
                success: function (response) {
                    // Xóa dòng trên table khi xóa thành công
                    $('#topicRow_' + fAQID).remove();

                    // Hiển thị thông báo thành công
                    Swal.fire({
                        icon: "success",
                        title: "Delete item successfully!",
                        timer: 1500
                    });
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error("Error deleting item:", textStatus, errorThrown);
                    Swal.fire({
                        icon: "error",
                        title: "Delete item failed",
                        text: "Please try again.",
                        timer: 1500
                    });
                }
            });
        }
    });
}

function deleteContact(contactID) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Admin/Contact/Delete/' + contactID, // Replace with your actual URL
                type: 'POST',
                success: function (response) {
                    // Xóa dòng trên table khi xóa thành công
                    $('#topicRow_' + contactID).remove();

                    // Hiển thị thông báo thành công
                    Swal.fire({
                        icon: "success",
                        title: "Delete item successfully!",
                        timer: 1500
                    });
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error("Error deleting item:", textStatus, errorThrown);
                    Swal.fire({
                        icon: "error",
                        title: "Delete item failed",
                        text: "Please try again.",
                        timer: 1500
                    });
                }
            });
        }
    });
}

function deleteAbout(aboutID) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Admin/About/Delete/' + aboutID, // Replace with your actual URL
                type: 'POST',
                success: function (response) {
                    // Xóa dòng trên table khi xóa thành công
                    $('#topicRow_' + aboutID).remove();

                    // Hiển thị thông báo thành công
                    Swal.fire({
                        icon: "success",
                        title: "Delete item successfully!",
                        timer: 1500
                    });
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error("Error deleting item:", textStatus, errorThrown);
                    Swal.fire({
                        icon: "error",
                        title: "Delete item failed",
                        text: "Please try again.",
                        timer: 1500
                    });
                }
            });
        }
    });
}

function deleteUser(email) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Admin/Auth/Delete/', // Replace with your actual URL
                data: { email: email },
                type: 'POST',
                success: function (response) {
                    // Xóa dòng trên table khi xóa thành công
                    $('#topicRow_' + email).remove();

                    // Hiển thị thông báo thành công
                    Swal.fire({
                        icon: "success",
                        title: "Delete item successfully!",
                        timer: 1500
                    });
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error("Error deleting item:", textStatus, errorThrown);
                    Swal.fire({
                        icon: "error",
                        title: "Delete item failed",
                        text: "Please try again.",
                        timer: 1500
                    });
                }
            });
        }
    });
}