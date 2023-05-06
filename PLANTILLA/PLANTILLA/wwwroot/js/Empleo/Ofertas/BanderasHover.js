function doHover(provincia) {
    provincia.style.backgroundImage = "url('../../assets/images/logo_card.gif')";
    //provincia.getElementsByTagName("h5")[0].style.opacity = "1";
}

function undoHover(provincia, img) {
    provincia.style.backgroundImage = "url('" + img + "')";
    //provincia.getElementsByTagName("h5")[0].style.opacity = "0";

}