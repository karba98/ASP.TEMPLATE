function doSearch() {
    const tableReg = document.getElementById('datos_card');
    const searchText = removeAccents(document.getElementById('searchTerm').value.toLowerCase());
    let card_titles = tableReg.getElementsByClassName("card-title");
    let cards = tableReg.getElementsByClassName("card");

    // Recorremos todas las filas con contenido de la tabla
    for (let i = 0; i < card_titles.length; i++) {

        let found = false;

        // Si el td tiene la clase "noSearch" no se busca en su cntenido
        if (card_titles[i].classList.contains("noSearch")) {
            continue;
        }
        const compareWith = removeAccents(card_titles[i].innerHTML.toLowerCase());
        // Buscamos el texto en el contenido de la celda
        if (compareWith.indexOf(searchText) > -1 || searchText.length==0) {
            found = true;
        }

        if (found) {
            cards[i].style.display = '';
        } else {
            // si no ha encontrado ninguna coincidencia, esconde la
            // fila de la tabla
            cards[i].style.display = 'none';
        }
    }
}
const removeAccents = (str) => {
    return str.normalize("NFD").replace(/[\u0300-\u036f]/g, "");
}