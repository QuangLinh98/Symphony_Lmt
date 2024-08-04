
    var joinNowModal = document.getElementById('joinNowModal');
    joinNowModal.addEventListener('show.bs.modal', function (event) {
        // Button that triggered the modal
        var button = event.relatedTarget;
    // Extract info from data-* attributes
    var topicName = button.getAttribute('data-topic-name');
    // Update the modal's content.
    var modalTitle = joinNowModal.querySelector('.modal-title');
    var modalBodyInput = joinNowModal.querySelector('.modal-body input#topicName');

    modalTitle.textContent = 'Register - ' + topicName;
    modalBodyInput.value = topicName;
    });
