function revealY() {
    var revealYs = document.querySelectorAll(".revealY");

    for (var i = 0; i < revealYs.length; i++) {
        var windowHeight = window.innerHeight;
        var elementTop = revealYs[i].getBoundingClientRect().top;
        var elementVisible = 150;

        if (elementTop < windowHeight - elementVisible) {
            revealYs[i].classList.add("active");
        } else {
            revealYs[i].classList.remove("active");
        }
    }
}

window.addEventListener("scroll", revealY);
window.addEventListener("DOMContentLoaded", revealY);
