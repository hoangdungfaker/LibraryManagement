document.addEventListener("DOMContentLoaded", function () {
    const logoutBtn = document.querySelector(".logout-btn");
    if (logoutBtn) {
        logoutBtn.addEventListener("click", function (e) {
            const ok = confirm("Bạn có chắc muốn đăng xuất không?");
            if (!ok) {
                e.preventDefault();
            }
        });
    }

    const alerts = document.querySelectorAll(".alert");
    if (alerts.length > 0) {
        setTimeout(() => {
            alerts.forEach(alert => {
                alert.style.transition = "opacity 0.5s ease";
                alert.style.opacity = "0";
                setTimeout(() => alert.remove(), 500);
            });
        }, 3500);
    }

    document.querySelectorAll(".menu-link").forEach(link => {
        if (window.location.pathname.toLowerCase() === link.getAttribute("href")?.toLowerCase()) {
            link.classList.add("active");
        }
    });
});