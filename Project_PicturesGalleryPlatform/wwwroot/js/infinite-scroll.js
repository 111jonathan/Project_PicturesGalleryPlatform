$(document).ready(function () {
    var page = 0;
    var pageSize = 40;
    var isLoading = false;

    function loadItems() {
        if (isLoading) return;
        isLoading = true;

        var urlParams = new URLSearchParams(window.location.search);
        var tag = urlParams.get('tag');
        var keyword = urlParams.get('keyword');
        var filepath = urlParams.get('filePath');
        var requestUrl = '';

        if (keyword) {
            var requestUrl = `/api/Images/Search?keyword=${keyword}`;  // �ھ� tag �I�s�������k
        } else if (tag) {
            var requestUrl = `/api/Images/SearchByTag?tag=${tag}`;  // �ھ� keyword �I�s�������k
        } else if (filepath) {
            var requestUrl = '@Url.Action("GetImagesByFilePath", "Home")';  // �ھ� filepath �I�s�������k
        }

        $.ajax({
            url: requestUrl,
            data: { page: page, pageSize: pageSize },
            type: 'GET',
            success: function (data) {
                if (data.length > 0) {
                    data.forEach(function (item) {
                        $('#imageResultsContainer').append(`
                            <div class="u-align-left u-container-style u-list-item u-repeater-item u-shape-rectangle u-white u-list-item-1"
                                 data-animation-name="customAnimationIn" data-animation-duration="1500" data-animation-direction="X"
                                 data-animation-delay="750">
                                <div class="u-container-layout u-similar-container u-valign-top u-container-layout-1">
                                    <i class="heart-btn heart ${item.isFavorited ? 'fas fa-heart' : 'far fa-heart'}" data-id="${item.id}"></i>
                                    <h4 class="u-align-center u-text u-text-2">
                                        ${item.tag}<br>
                                    </h4>
                                    <img class="u-expanded-width u-image u-image-default u-image-1" alt="${item.tag}" data-image-width="363"
                                         data-image-height="363" src="/images2/${item.id}.webp">
                                    <p class="u-align-center u-text u-text-3">${item.title}</p>
                                    <a href="../SinglePic/SinglePic?id=${item.id}"
                                       class="u-border-1 u-border-active-palette-3-base u-border-black u-border-hover-palette-3-base u-border-no-left u-border-no-right u-border-no-top u-btn u-button-style u-hover-feature u-none u-text-active-black u-text-body-color u-text-hover-black u-btn-1"
                                       data-animation-name="" data-animation-duration="0" data-animation-delay="0" data-animation-direction=""/>
                                        More
                                    </a>
                                </div>
                            </div>
                        `);
                    });
                    page++;
                }
                isLoading = false;
            },
            error: function () {
                isLoading = false;
                alert('Failed to load items');
            }
        });
    }

    loadItems();

    $(window).scroll(function () {
        if ($(window).scrollTop() + $(window).height() >= $(document).height() - 100) {
            loadItems();
        }
    });

    $(document).on('click', '.heart', function (e) {
        e.preventDefault();

        var heart = $(this);
        var imageId = heart.data('id');
        var isLiked = heart.hasClass('fas');

        $.ajax({
            url: '/Page/ToggleImageLikeStatus',
            type: 'POST',
            data: {
                id: imageId,
                category: isLiked ? 'fas' : 'far'
            },
            success: function (response) {
                if (response.success) {
                    if (isLiked) {
                        heart.removeClass('fas fa-heart').addClass('far fa-heart');
                    } else {
                        heart.removeClass('far fa-heart').addClass('fas fa-heart');
                    }
                } else {
                    window.location.href = '/Login/Login';
                }
            },
            error: function () {
                alert('An error occurred, please try again.');
            }
        });
    });
});
