document.addEventListener("DOMContentLoaded", function () {
    // ===== Confirm logout =====
    const logoutBtn = document.querySelector(".logout-btn");
    if (logoutBtn) {
        logoutBtn.addEventListener("click", function (e) {
            e.preventDefault();

            Swal.fire({
                title: "Đăng xuất?",
                text: "Bạn có chắc muốn đăng xuất khỏi hệ thống không?",
                icon: "question",
                showCancelButton: true,
                confirmButtonText: "Đăng xuất",
                cancelButtonText: "Huỷ"
            }).then((result) => {
                if (result.isConfirmed) {
                    window.location.href = logoutBtn.href;
                }
            });
        });
    }

    // ===== Auto hide alerts =====
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

    // ===== Active menu =====
    document.querySelectorAll(".menu-link").forEach(link => {
        const href = link.getAttribute("href");
        if (href && window.location.pathname.toLowerCase() === href.toLowerCase()) {
            link.classList.add("active");
        }
    });

    // ===== Confirm delete buttons =====
    document.querySelectorAll(".btn-delete-confirm").forEach(button => {
        button.addEventListener("click", function (e) {
            e.preventDefault();

            const url = this.getAttribute("href");

            Swal.fire({
                title: "Xoá dữ liệu?",
                text: "Thao tác này có thể không hoàn tác được.",
                icon: "warning",
                showCancelButton: true,
                confirmButtonText: "Xoá",
                cancelButtonText: "Huỷ",
                confirmButtonColor: "#d93636"
            }).then((result) => {
                if (result.isConfirmed && url) {
                    window.location.href = url;
                }
            });
        });
    });

    // ===== Loading when submit form =====
    document.querySelectorAll("form").forEach(form => {
        form.addEventListener("submit", function () {
            const submitBtn = form.querySelector("button[type='submit']");
            if (submitBtn) {
                submitBtn.disabled = true;
                const originalText = submitBtn.innerHTML;
                submitBtn.dataset.originalText = originalText;
                submitBtn.innerHTML = "Đang xử lý...";
            }
        });
    });

    // ===== Re-enable submit if page restored from cache =====
    window.addEventListener("pageshow", function () {
        document.querySelectorAll("form button[type='submit']").forEach(btn => {
            btn.disabled = false;
            if (btn.dataset.originalText) {
                btn.innerHTML = btn.dataset.originalText;
            }
        });
    });
});