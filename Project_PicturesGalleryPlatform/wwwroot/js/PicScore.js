document.addEventListener("DOMContentLoaded", () => {
    const ratingContainer = document.querySelector(".star-rating");
    const stars = document.querySelectorAll(".star-rating i");
    const submitButton = document.getElementById("submit-rating");

    let isLoggedIn = false;
    let user = getQueryParam("user");  // 假設有一個 user 參數

    // 確認用戶是否登入
    function getQueryParam(param) {
        const urlParams = new URLSearchParams(window.location.search);
        return urlParams.get(param);
    }

    // 點擊提交按鈕
    submitButton.addEventListener("click", async () => {
        if (!isLoggedIn) {
            const userConfirmed = confirm("您尚未登入，是否前往登入頁面？");
            if (userConfirmed) {
                window.location.href = '/Login/Login'; // 重定向到登入頁面
            }
            return;
        }

        // 確保選擇了評分
        const selectedStar = Array.from(stars).find(star => star.classList.contains("fas"));
        const rating = selectedStar ? selectedStar.getAttribute("data-value") : null;
        const productId = ratingContainer.getAttribute("data-product-id");

        if (!rating || !productId) {
            alert("請選擇評分！");
            return;
        }

        const csrfToken = document.querySelector('input[name="__RequestVerificationToken"]').value;

        try {
            // 發送評分請求到後端
            const response = await fetch('/SinglePic/SubmitRating', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'X-CSRF-TOKEN': csrfToken
                },
                body: JSON.stringify({
                    user: user,
                    productId: productId,
                    rating: rating
                })
            });

            const data = await response.json();

            if (!response.ok || !data.success) {
                alert("評分失敗，請稍後再試！");
            } else {
                console.log("評分成功:", data.message);
                alert("評分成功！");
            }
        } catch (error) {
            console.error("評分過程中發生錯誤:", error);
            alert("評分過程中發生錯誤，請稍後再試！");
        }
    });

    // 綁定點擊事件給每顆星星
    stars.forEach(star => {
        star.addEventListener("click", () => {
            if (!isLoggedIn) {
                const userConfirmed = confirm("您尚未登入，是否前往登入頁面？");
                if (userConfirmed) {
                    window.location.href = '/Login/Login'; // 重定向到登入頁面
                }
                return;
            }

            // 更新星星的視覺效果
            updateStars(star.getAttribute("data-value"));
        });
    });

    /**
     * 更新星星顯示狀態
     * @param {string} rating - 使用者選擇的評分值
     */
    function updateStars(rating) {
        stars.forEach((star, index) => {
            if (index < rating) {
                star.classList.remove("far");
                star.classList.add("fas");
            } else {
                star.classList.remove("fas");
                star.classList.add("far");
            }
        });
    }
});
