$(document).on('click', '.heart-btn', function () {
    var heartBtn = $(this);
    var imageId = heartBtn.data('id');
    var isFavorited = heartBtn.find(".heart").hasClass('fas');

    $.ajax({
        url: '/Page/ToggleImageLikeStatus',
        type: 'POST',
        data: {
            id: imageId,
            isFavorited: isFavorited ? 'fas' : 'far'
        },
        success: function (response) {
            if (response.success) {
                if (isFavorited) {
                    heartBtn.find(".heart").removeClass('fas fa-heart').addClass('far fa-heart');

                } else {
                    heartBtn.find(".heart").removeClass('far fa-heart').addClass('fas fa-heart');

                }
            } else {
                window.location.href = '/Login/Login'; // 未登入的處理
            }
        },
        error: function () {
            alert('An error occurred, please try again.');
        }
    });
});