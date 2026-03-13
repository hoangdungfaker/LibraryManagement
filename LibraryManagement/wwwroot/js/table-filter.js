document.addEventListener("DOMContentLoaded", function () {
    const searchInputs = document.querySelectorAll("[data-table-search]");

    searchInputs.forEach(input => {
        const targetSelector = input.getAttribute("data-table-search");
        const rows = document.querySelectorAll(`${targetSelector} tbody tr`);

        input.addEventListener("input", function () {
            const keyword = this.value.toLowerCase().trim();

            rows.forEach(row => {
                const rowText = row.innerText.toLowerCase();
                row.style.display = rowText.includes(keyword) ? "" : "none";
            });
        });
    });
});