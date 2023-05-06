//Identico a BuscadorDinamicoTabla.js pero para buscar
//en td's con una img y un texto en su interior (es decir bandera+texto) 
function doSearch() {
    const tableReg = document.getElementById('datos');
    const searchText = removeAccents(document.getElementById('searchTerm').value.toLowerCase());
    let total = 0;

    // Recorremos todas las filas con contenido de la tabla
    for (let i = 0; i < tableReg.rows.length; i++) {
        // Si el td tiene la clase "noSearch" no se busca en su cntenido
        if (tableReg.rows[i].classList.contains("noSearch")) {
            continue;
        }

        let found = false;
        const cellsOfRow = tableReg.rows[i].getElementsByTagName('td');
        // Recorremos todas las celdas
        for (let j = 0; j < cellsOfRow.length && !found; j++) {
            const compareWith = removeAccents(cellsOfRow[j].children[1].innerHTML.toLowerCase());
            // Buscamos el texto en el contenido de la celda
            if (searchText.length == 0 || compareWith.indexOf(searchText) > -1) {
                found = true;
                total++;
            }
        }
        if (found) {
            tableReg.rows[i].style.display = '';
        } else {
            // si no ha encontrado ninguna coincidencia, esconde la
            // fila de la tabla
            tableReg.rows[i].style.display = 'none';
        }
    }
}

const removeAccents = (str) => {
    return str.normalize("NFD").replace(/[\u0300-\u036f]/g, "");
}