document.addEventListener("DOMContentLoaded", function () {
    const barChart = document.getElementById("barChart");
    const pieChart = document.getElementById("pieChart");

    if (!barChart || !pieChart || typeof reportData === "undefined") return;

    new Chart(barChart, {
        type: "bar",
        data: {
            labels: ["Đầu sách", "Cuốn sách", "Độc giả", "Nhân viên"],
            datasets: [{
                label: "Số lượng",
                data: [
                    reportData.tongSoSach,
                    reportData.tongSoCuonSach,
                    reportData.tongSoDocGia,
                    reportData.tongSoNhanVien
                ],
                borderWidth: 1,
                borderRadius: 10
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: { display: false }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        precision: 0
                    }
                }
            }
        }
    });

    new Chart(pieChart, {
        type: "pie",
        data: {
            labels: ["Đang mượn", "Đã trả", "Quá hạn"],
            datasets: [{
                data: [
                    reportData.tongPhieuDangMuon,
                    reportData.tongPhieuDaTra,
                    reportData.tongPhieuQuaHan
                ],
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: "bottom"
                }
            }
        }
    });
});