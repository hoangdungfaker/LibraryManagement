document.addEventListener("DOMContentLoaded", function () {
    console.log("LibraryManagement loaded");

    // Confirm logout
    const logoutBtn = document.querySelector(".logout-btn");
    if (logoutBtn) {
        logoutBtn.addEventListener("click", function (e) {
            const ok = confirm("Bạn có chắc muốn đăng xuất không?");
            if (!ok) {
                e.preventDefault();
            }
        });
    }

    // Auto hide alerts
    const alerts = document.querySelectorAll(".alert");
    if (alerts.length > 0) {
        setTimeout(() => {
            alerts.forEach(alert => {
                alert.style.transition = "opacity 0.5s ease";
                alert.style.opacity = "0";
                setTimeout(() => {
                    alert.remove();
                }, 500);
            });
        }, 3500);
    }
});