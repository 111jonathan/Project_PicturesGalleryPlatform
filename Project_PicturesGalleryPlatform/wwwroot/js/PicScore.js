document.addEventListener("DOMContentLoaded", () => {
    const ratingContainer = document.querySelector(".star-rating");
    const stars = document.querySelectorAll(".star-rating i");
    let isLoggedIn = false; // 初始化為 false

    // 檢查使用者是否登入
    fetch('/ SinglePic/IsUserLoggedIn', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
        }
    })
        .then(response => response.json())
        .then(data => {
            isLoggedIn = data.isLoggedIn;
            if (!isLoggedIn) {
                alert("您尚未登入，請先登入後再進行評分！");
            }
        })
        .catch(error => {
            console.error("檢查登入狀態失敗:", error);
        });

    // 綁定點擊事件給每個星星
    stars.forEach(star => {
        star.addEventListener("click", async (e) => {
            if (!isLoggedIn) {
                const userConfirmed = confirm("您尚未登入，是否前往登入頁面？");
                if (userConfirmed) {
                    window.location.href = '/Login/Login'; // 重定向到登入頁面
                }
                return;
            }

            const rating = e.target.getAttribute("data-value"); // 取得評分
            const productId = ratingContainer.getAttribute("data-product-id"); // 取得產品 ID

            if (!rating || !productId) {
                console.error("缺少評分或產品 ID 資訊！");
                return;
            }

            // 防止多次點擊
            stars.forEach(s => s.classList.add("disabled"));

            // 更新星星的視覺效果
            updateStars(rating);

            try {
                // 發送評分請求到後端
                const response = await fetch('/SinglePic/SubmitRating', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'X-CSRF-TOKEN': document.querySelector('input[name="__RequestVerificationToken"]').value
                    },
                    body: JSON.stringify({ productId, rating })
                });

                const data = await response.json();

                if (!response.ok || !data.success) {
                    alert("評分失敗，請稍後再試！");
                } else {
                    console.log("評分成功:", data.message);
                }
            } catch (error) {
                console.error("評分失敗:", error);
                alert("評分過程中發生錯誤，請稍後再試！");
            } finally {
                // 解除多次點擊防護
                stars.forEach(s => s.classList.remove("disabled"));
            }
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
