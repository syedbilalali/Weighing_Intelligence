$(document).ready(function () {
    $("#BtnLogin").click(function () {
        var check = true;
        if ($("#UserName").val().trim() === "") {
            check = false;
            $("#UserName").addClass("error");
        }
        if ($("#Password").val().trim() === "") {
            check = false;
            $("#Password").addClass("error");
        }
        if ($("#WBID").val().trim() === "") {
            check = false;
            $("#WBID").addClass("error");
        }
        if ($("#PlantID").val().trim() === "") {
            check = false;
            $("#PlantID").addClass("error");
        }

        return check;

    });
});