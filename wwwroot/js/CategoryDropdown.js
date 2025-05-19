document.addEventListener("DOMContentLoaded", () => {
    const dropdownMenu = document.getElementById("categoryDropdown");

    fetch("/Category/getAllCategoryName")
        .then(response => response.json())
        .then(data => {
            data.forEach(category => {
                const li = document.createElement("li");
                const a = document.createElement("a");
                li.classList.add("list-group-item", "p-0");
                a.classList.add("dropdown-item");
                a.href = `/Book/FindByCategory/${category.categoryId}`;
                a.textContent = category.categoryName;
                li.appendChild(a);
                dropdownMenu.appendChild(li); 
            });
        });
});
