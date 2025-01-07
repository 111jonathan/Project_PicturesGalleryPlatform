$(document).ready(function () {
    var page = 0;// ���ͪ�page�s��
    var pageSize = 4;// �@�����J�X�����
    var isLoading = false;

    function loadItems() {
        if (isLoading) return;
        isLoading = true;

        // ���loading�e��
        document.getElementById("loadingOverlay").style.display = "flex";

        $.ajax({
            url: $('#imageResultsContainer').data('url'),
            data: { page: page, pageSize: pageSize },
            type: 'GET',
            success: function (data) {
                if (data.pictures.length > 0) {
                    keyword_AI = data.keyword_AI;
                    console.log("���respone(data): ", data);
                    data.pictures.forEach(function (item) {
                        $('#imageResultsContainer').append(`
                            <div class="u-align-left u-container-style u-list-item u-repeater-item u-shape-rectangle u-white u-list-item-1"
                                 data-animation-name="customAnimationIn" data-animation-duration="1500" data-animation-direction="X"
                                 data-animation-delay="750">
                                <div class="u-container-layout u-similar-container u-valign-top u-container-layout-1">
                                    <h4 class="u-align-center u-text u-text-2">
                                        ${keyword_AI}<br>
                                    </h4>
                                    <img class="u-expanded-width u-image u-image-default u-image-1" alt="${keyword_AI}" data-image-width="363"
                                         data-image-height="363" src="${item.output_url}">
                                    <p class="u-align-center u-text u-text-3">${item.id}</p>
                                    <a href="../SinglePic/SinglePic_AI?id=${item.id}&output_url=${item.output_url}&share_url=${item.share_url}&backend_request_id=${item.backend_request_id}&keyword=${keyword_AI}"
                                       class="u-border-1 u-border-active-palette-3-base u-border-black u-border-hover-palette-3-base u-border-no-left u-border-no-right u-border-no-top u-btn u-button-style u-hover-feature u-none u-text-active-black u-text-body-color u-text-hover-black u-btn-1"
                                       data-animation-name="" data-animation-duration="0" data-animation-delay="0" data-animation-direction="" target="_blank">
                                        More
                                    </a>
                                </div>
                            </div>
                        `);
                    });
                    page++;
                }
                isLoading = false;
                // �ШD����������loading�e��
                document.getElementById("loadingOverlay").style.display = "none";
            },
            error: function () {
                isLoading = false;
                alert('Failed to load items');
            }
        });
    }

    loadItems();

    $('#GenerateMoreButton').click(function () {
        loadItems();
    });
});


