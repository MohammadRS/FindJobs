    document.querySelector(".zmenu__btn").addEventListener("click", function () {
        document.querySelector(".zmenu").classList.toggle("zmenu--active");
        this.classList.toggle("zmenu__btn--active");
        document.querySelector(".zmenu__nav").classList.toggle("zmenu__nav--active");
    });
