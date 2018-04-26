function saveTicket() {
    var textBox = document.getElementById("addTicketText");
    var typeChoser = document.getElementById("addTicketType");
    if (textBox.value == "") {
        textBox.style.border = "groove";
        textBox.style.borderColor = "orangered";
        textBox.style.borderWidth = "1.5px";
    }
    if (typeChoser.value == 0) {
        typeChoser.style.border = "groove";
        typeChoser.style.borderColor = "orangered";
        typeChoser.style.borderWidth = "1.5px";
    }
    if (textBox.value != "" && typeChoser.value != 0) {
        textBox.style.border = "";
        textBox.style.borderColor = "";
        textBox.style.borderWidth = "";
        typeChoser.style.border = "";
        typeChoser.style.borderColor = "";
        typeChoser.style.borderWidth = "";
        uploadTicket(textBox.value, typeChoser.value);
    }
}

function uploadTicket(text, type) {
    var uploaderUrl = "/Ticket/EditSave";
    var formData = new FormData();
    formData.append("id", editTicketId);
    formData.append("description", text);
    formData.append("type", type);
    var xhr = new XMLHttpRequest();
    xhr.open('POST', uploaderUrl, true);
    xhr.onloadend = function () {
        var parsedTicket = JSON.parse(xhr.responseText);
        if (parsedTicket != null) {
            location.reload();
        }
    }
    xhr.send(formData);
}