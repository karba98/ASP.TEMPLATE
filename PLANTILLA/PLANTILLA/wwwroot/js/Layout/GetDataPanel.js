var base_url = window.location.origin;
$("#panel_vc").load(base_url+"/Home/PanelDatos", function (response) {
    $("#spin_panel").remove();
});