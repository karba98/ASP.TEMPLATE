document.onload = GetWords();

function GetWords() {
    $.get("/Empleo/GetWords", function (data) {
        // Iterar sobre las palabras obtenidas de la API
        $.each(data, function (index, palabra) {
            // Crear una nueva fila para cada palabra
            let row = $("<tr></tr>");
            // Crear las celdas para la palabra y el botón de eliminar
            let wordCell = $("<td>" + palabra + "</td>");
            let buttonCell = $("<td></td>");
            // Crear el botón de eliminar y asignarlo a la celda de acciones
            let button = $("<a class=\"bi bi-trash2-fill delete btnaction\" >" +
                "<img src = \"/assets/webfonts/trash3-fill.svg\" alt = \"Bootstrap\" width = \"23\" height = \"23\" >" +
                "</a>");
            button.on("click", function () { eliminarPalabra(palabra); });
            buttonCell.append(button);
            // Agregar las celdas a la fila
            row.append(buttonCell);
            row.append(wordCell);
            // Agregar la fila a la tabla
            $("#tablapalabras").append(row);
        });
    });
}
function eliminarPalabra(word) {
    $.get("/Empleo/DeleteWord", { "word": word }, function (data) {
        cleanTable();
        // Iterar sobre las palabras obtenidas de la API
        $.each(data, function (index, palabra) {
            // Crear una nueva fila para cada palabra
            let row = $("<tr></tr>");
            // Crear las celdas para la palabra y el botón de eliminar
            let wordCell = $("<td>" + palabra + "</td>");
            let buttonCell = $("<td></td>");
            // Crear el botón de eliminar y asignarlo a la celda de acciones
            let button = $("<a class=\"bi bi-trash2-fill delete btnaction\" >" +
                "<img src = \"/assets/webfonts/trash3-fill.svg\" alt = \"Bootstrap\" width = \"23\" height = \"23\" >" +
                "</a>");
            button.on("click", function () { eliminarPalabra(palabra); });
            buttonCell.append(button);
            // Agregar las celdas a la fila
            row.append(buttonCell);
            row.append(wordCell);
            // Agregar la fila a la tabla
            $("#tablapalabras").append(row);
        });
    });
}
function insertWord() {
    let inputText = document.getElementById("inputText").value;
    if (inputText != null && inputText != "" && inputText != " ") {
        $.get("/Empleo/InsertWord", { "word": inputText }, function (data) {
            cleanTable();
            // Iterar sobre las palabras obtenidas de la API
            $.each(data, function (index, palabra) {
                // Crear una nueva fila para cada palabra
                let row = $("<tr></tr>");
                // Crear las celdas para la palabra y el botón de eliminar
                let wordCell = $("<td>" + palabra + "</td>");
                let buttonCell = $("<td></td>");
                // Crear el botón de eliminar y asignarlo a la celda de acciones
                let button = $("<a class=\"bi bi-trash2-fill delete btnaction\" >" +
                    "<img src = \"/assets/webfonts/trash3-fill.svg\" alt = \"Bootstrap\" width = \"23\" height = \"23\" >" +
                    "</a>");
                button.on("click", function () { eliminarPalabra(palabra); });
                buttonCell.append(button);
                // Agregar las celdas a la fila
                row.append(buttonCell);
                row.append(wordCell);
                // Agregar la fila a la tabla
                $("#tablapalabras").append(row);
            });
        });
    } else {
        console.log("errorrr");
    }
}
function cleanTable() {
    $("#tablapalabras tr").remove();
}
