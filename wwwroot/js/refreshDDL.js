//Reusable refresh routine for SelectLists
//Parameters: ddl_ID - ID of the Select to refresh
//  uri - controller/action to call to get a new SelcectList as JSON
//  addDefault - Boolean indicating if you want to add a default/prompt 
//      option at the top of the list.
//  defaultText - string value to display as the default/prompt value
function refreshDDL(ddl_ID, uri, addDefault, defaultText, fadeOutIn) {
    var theDDL = $("#" + ddl_ID);
    var selectedOption = theDDL.val();
    var URL = "/" + uri + "/" + selectedOption;
    $(function () {
        $.getJSON(URL, function (data) {
            if (data !== null && !jQuery.isEmptyObject(data)) {
                theDDL.empty();
                if (addDefault) {
                    theDDL.append($('<option/>', {
                        value: "",
                        text: defaultText
                    }));
                }
                $.each(data, function (index, item) {
                    theDDL.append($('<option/>', {
                        value: item.value,
                        text: item.text,
                        selected: item.selected
                    }));
                });
                theDDL.trigger("chosen:updated");
            }
        });
    });
    if (fadeOutIn) {
        theDDL.fadeToggle(400, function () {
            theDDL.fadeToggle(400);
        });
    }
    return;
}