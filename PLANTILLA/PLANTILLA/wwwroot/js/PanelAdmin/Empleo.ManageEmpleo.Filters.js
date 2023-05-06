
//Filters
function doSearch() {
    setAllDisplayed();
    searchTitulo();
    searchFecha();
    searchFuente();
    searcCategoria();
}

function searchTitulo() {
    const tableReg = document.getElementById('datos');
    const searchText = document.getElementById('searchTerm').value.toLowerCase();

    // Recorremos todas las filas con contenido de la tabla
    for (let i = 1; i < tableReg.rows.length; i++) {
        // Si el td tiene la clase "noSearch" no se busca en su cntenido
        if (tableReg.rows[i].classList.contains("noSearch")) {
            continue;
        }

        let found = false;

        var row = tableReg.rows[i];
        // Buscamos el texto en el contenido de la celda con classname td_titulo
        const compareWith = removeAccents(row.getElementsByClassName("td_titulo_a")[0].innerHTML.toLowerCase());
        if (searchText.length == 0 || compareWith.indexOf(searchText) > -1) {
            found = true;
        }
        if (found) {
            tableReg.rows[i].style.display = '';
        } else {
            // si no ha encontrado ninguna coincidencia, esconde la
            // fila de la tabla
            tableReg.rows[i].style.display = 'none';
            tableReg.rows[i].classList.add("noSearch");
        }
    }
}
function searchFecha() {
    var datepicker = document.getElementById("date");
    if (datepicker.value == "") {
        return;
    }
    var selected_date = new Date(datepicker.value);
    const tableReg = document.getElementById('datos');

    for (let i = 1; i < tableReg.rows.length; i++) {

        // Si el td tiene la clase "noSearch" no se busca en su cntenido
        if (tableReg.rows[i].classList.contains("noSearch")) {
            continue;
        }

        let found = false;
        var row = tableReg.rows[i];
        var fecha = row.children[8].innerHTML.trim();

        var offer_date = null;

        if (fecha.includes("AM") || fecha.includes("PM")) {
            fecha = fecha.slice(0, -3);
        }

        offer_date = getDate(fecha);

        var dif = dateDifference(selected_date, offer_date);

        if (dif == 0) {
            found = true;
        }
        if (found) {
            tableReg.rows[i].style.display = '';
        } else {
            // si no ha encontrado ninguna coincidencia, esconde la
            // fila de la tabla
            tableReg.rows[i].style.display = 'none';
            tableReg.rows[i].classList.add("noSearch");
        }

    }
}
function searchFuente() {

    var select = document.getElementById("Filtros");
    var value = select.value;

    let searchText = ""
    const tableReg = document.getElementById('datos');
    if (value == "Borradores") {
        searchText = "B"
    } else if (value == "Públicas") {
        searchText = "P"
    } else { searchText = "" }


    let total = 0;

    // Recorremos todas las filas con contenido de la tabla
    for (let i = 1; i < tableReg.rows.length; i++) {
        if (searchText == "") {
            break;
        }
        // Si el td tiene la clase "noSearch" no se busca en su cntenido
        if (tableReg.rows[i].classList.contains("noSearch")) {
            continue;
        }

        let found = false;
        var row = tableReg.rows[i];

        const compareWith = removeAccents(row.getElementsByClassName("td_modo")[0].innerHTML);
        if (searchText.length == 0 || compareWith.indexOf(searchText) > -1) {
            found = true;
            total++;
        }
        if (found) {
            tableReg.rows[i].style.display = '';
        } else {
            // si no ha encontrado ninguna coincidencia, esconde la
            // fila de la tabla
            tableReg.rows[i].style.display = 'none';
            tableReg.rows[i].classList.add("noSearch");
        }

    }
}

function searcCategoria() {
    var select = document.getElementById("Categorias");
    var value = select.value;

    let searchText = ""
    const tableReg = document.getElementById('datos');
    if (value == "Todas") {
        return;
    } else { searchText = value; }


    let total = 0;

    // Recorremos todas las filas con contenido de la tabla
    for (let i = 1; i < tableReg.rows.length; i++) {
        if (searchText == "") {
            break;
        }
        // Si el td tiene la clase "noSearch" no se busca en su cntenido
        if (tableReg.rows[i].classList.contains("noSearch")) {
            continue;
        }

        let found = false;
        var row = tableReg.rows[i];

        const compareWith = removeAccents(row.getElementsByClassName("td_categoria")[0].innerHTML);
        if (searchText.length == 0 || compareWith.indexOf(searchText) > -1) {
            found = true;
            total++;
        }
        if (found) {
            tableReg.rows[i].style.display = '';
        } else {
            // si no ha encontrado ninguna coincidencia, esconde la
            // fila de la tabla
            tableReg.rows[i].style.display = 'none';
            tableReg.rows[i].classList.add("noSearch");
        }

    }
}

/*Varios*/
const removeAccents = (str) => {
    return str.normalize("NFD").replace(/[\u0300-\u036f]/g, "");
}


function setAllDisplayed() {
    const tableReg = document.getElementById('datos');
    for (let j = 0; j < tableReg.rows.length; j++) {
        tableReg.rows[j].style.display = 'table-row';
        tableReg.rows[j].classList.remove("noSearch");
    }
}
var today = new Date();
let timeoutDebounce = null

function resetFilters() {
    setAllDisplayed();
    var datepicker = document.getElementById("date");
    datepicker.value = "";
    var select = document.getElementById("Filtros");
    select.value = "Todas"
    var searcher = document.getElementById("searchTerm");
    searcher.value = "";

}


function getDate(fecha) {
    var d = fecha;
    var d1 = d.split(" ");
    var date = d1[0].split("/");
    var time = d1[1].split(":");
    var dd = date[0];
    var mm = date[1];
    var yy = date[2];
    var hh = time[0];
    var min = time[1];
    var ss = time[2];
    var fech = new Date(yy, mm - 1, dd, hh, min, ss);

    return fech;
}
function dateDifference(date2, date1) {
    // Discard the time and time-zone information.
    const diffTime = Math.abs(removeTime(date2) - removeTime(date1));
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
    return diffDays;
}

function removeTime(date = new Date()) {
    return new Date(
        date.getFullYear(),
        date.getMonth(),
        date.getDate()
    );
}