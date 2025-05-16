document.addEventListener("DOMContentLoaded", function () {
    const countDisplay = document.getElementById("countDisplay");

    if (countDisplay) {
        fetch("/Cart/GetCartCount")
            .then(response => {
                if (!response.ok) throw new Error("Not authorized");
                return response.json();
            })
            .then(data => {
                if (data > 0) {
                    countDisplay.textContent = data;
                    countDisplay.style.display = "inline-block";
                } else {
                    countDisplay.style.display = "none";
                }
            })
            .catch(() => {
                // User is not logged in or error occurred
                countDisplay.style.display = "none";
            });
    }
});
