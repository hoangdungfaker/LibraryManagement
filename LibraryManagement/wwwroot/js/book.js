document.addEventListener("DOMContentLoaded", function () {
    const searchInput = document.getElementById("bookSearchInput");
    const categoryInputs = document.querySelectorAll("input[name='bookCategory']");
    const bookCards = document.querySelectorAll(".book-filter-item");
    const resetBtn = document.getElementById("resetBookFilter");

    if (!searchInput || bookCards.length === 0) return;

    function normalizeText(text) {
        return (text || "").toLowerCase().trim();
    }

    function getSelectedCategory() {
        const selected = document.querySelector("input[name='bookCategory']:checked");
        return selected ? selected.value : "";
    }

    function filterBooks() {
        const keyword = normalizeText(searchInput.value);
        const category = normalizeText(getSelectedCategory());

        bookCards.forEach(card => {
            const title = normalizeText(card.dataset.title);
            const bookCategory = normalizeText(card.dataset.category);

            const matchTitle = title.includes(keyword);
            const matchCategory = category === "" || bookCategory === category;

            card.style.display = (matchTitle && matchCategory) ? "" : "none";
        });
    }

    searchInput.addEventListener("input", filterBooks);

    categoryInputs.forEach(input => {
        input.addEventListener("change", filterBooks);
    });

    if (resetBtn) {
        resetBtn.addEventListener("click", function () {
            searchInput.value = "";

            const firstRadio = document.querySelector("input[name='bookCategory'][value='']");
            if (firstRadio) firstRadio.checked = true;

            filterBooks();
        });
    }
});