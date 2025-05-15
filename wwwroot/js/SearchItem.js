document.addEventListener("DOMContentLoaded", function () {
    const searchInput = document.getElementById("searchInput");
    const searchResult = document.getElementById("searchResult");

    if (!searchInput || !searchResult) return;

    let items = [];

    fetch('/Book/fetchBookTitle')
        .then(response => response.json())
        .then(data => {
            items = data;
        });

    searchInput.addEventListener("input", () => {
        const query = searchInput.value.toLowerCase();
        searchResult.innerHTML = "";

        if (query.trim() === "") return;

        const filteredItems = items.filter(item => item.toLowerCase().includes(query));
        filteredItems.forEach(item => {
            const li = document.createElement("li");
            li.classList.add("list-group-item");
            li.textContent = item;
            searchResult.appendChild(li);
        });
    });
});
