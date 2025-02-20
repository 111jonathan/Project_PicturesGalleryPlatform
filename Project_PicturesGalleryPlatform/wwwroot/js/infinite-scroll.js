$(document).ready(function () {
    var page = 0;
    var pageSize = 40;
    var isLoading = false;

    var urlParams = new URLSearchParams(window.location.search);
    var tag = urlParams.get('tag');
    var keyword = urlParams.get('keyword');
    var ids = urlParams.get('ids');

    if (keyword) {
        $("h2.u-align-center").text(keyword + '\u5716\u7247');
    } else if (tag) {
        $("h2.u-align-center").text(tag + '\u5716\u7247');
    }

    function loadItems() {
        if (isLoading) return;
        isLoading = true;

        var requestUrl = '';

        if (keyword) {
            var requestUrl = `/api/Images/Search?keyword=${keyword}`;  // �ھ� tag �I�s�������k
        } else if (tag) {
            var requestUrl = `/api/Images/SearchByTag?tag=${tag}`;  // �ھ� keyword �I�s�������k
        } else if (ids) {
            var requestUrl = `/api/Images/SearchByids?ids=${ids}`;;  // �ھ� filepath �I�s�������k
        }if (!requestUrl) {
            console.error("No valid parameters to generate request URL.");
            isLoading = false;
            return;
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
                                    <a href="javascript:void(0);" class="heart-btn" data-id="${item.id}">
                                      <i class="heart ${item.isFavorited ? 'fas fa-heart' : 'far fa-heart'}" style="color: red;"></i>
                                    </a>

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
});
