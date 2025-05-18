document.addEventListener("DOMContentLoaded", () =>  {
    const searchInput = document.getElementById("searchInput");
    const searchResult = document.getElementById("searchResult");

    if (!searchInput || !searchResult) return;
    
    let books = [];

    fetch('/Book/fetchBookTitle')
        .then(response => response.json())
        .then(data => {
            books = data;
        });

    searchInput.addEventListener("input", () => {
        const query = searchInput.value.toLowerCase();
        searchResult.innerHTML = "";

        if (query.trim() === "") return;

        const filteredItems = books.filter(item => item.bookName.toLowerCase().includes(query));
        filteredItems.forEach(book => {
            const li = document.createElement("li");
            li.classList.add("list-group-item", "p-0");

            const link = document.createElement("a");
            link.href = `/Book/Details/${book.bookId}`;
            link.classList.add("d-flex", "align-items-center", "gap-2", "text-decoration-none", "text-dark", "p-2");

            const img = document.createElement("img");
            img.src = book.imagePath;
            img.alt = book.bookName;
            img.style.width = "50px";
            img.style.height = "75px";
            img.style.objectFit = "cover";
            img.classList.add("rounded");

            const span = document.createElement("span");
            span.textContent = book.bookName;

           
            link.appendChild(img);
            link.appendChild(span);
            li.appendChild(link);
            searchResult.appendChild(li);
        });
    });
});
