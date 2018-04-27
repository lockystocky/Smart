function selectStateChanged() {
    var selectTeam = document.getElementById("selectTeam");
    var selectType = document.getElementById("selectType");
    if (selectTeam.value != 0 && selectType.value != 0) {
        showAllTickets(selectTeam.value, selectType.value);
    }
}

function displayNewTicket(tickets) {
    if (emptyTeamTickets == true) {
        clearTicketsDiv();
        emptyTeamTickets = false;
    }
    var ticketsDisplay = document.getElementById("ticketsDiv");

    if (tickets.length > 0) {
        for (var counter = 0; counter < tickets.length; counter++) {
            displayOneTicket(tickets[counter]);
        }
    }
    else {
        emptyTeamTickets = true;
        var cardDiv = document.createElement("div");
        cardDiv.className = "card";
        ticketsDisplay.appendChild(cardDiv);

        var sysMessageDiv = document.createElement("div");
        sysMessageDiv.className = "systemMessageText";
        sysMessageDiv.innerText = "This team does not have any child tickets with that type, select different one.";
        cardDiv.appendChild(sysMessageDiv);
    }
}

function displayOneTicket(ticket) {
    if (emptyTeamTickets == true) {
        clearTicketsDiv();
        emptyTeamTickets = false;
    }
    var tempId = ticket.Id;
    var ticketsDisplay = document.getElementById("ticketsDiv");

    var cardDiv = document.createElement("div");
    cardDiv.className = "card";
    cardDiv.id = "ticket_" + ticket.Id;
    ticketsDisplay.appendChild(cardDiv);

    var statusRectDiv = document.createElement("div");
    statusRectDiv.className = "statusRect";
    statusRectDiv.id = "strect_" + ticket.Id;
    statusRectDiv.style.backgroundColor = statusColorsJs[ticket.State];
    cardDiv.appendChild(statusRectDiv);

    var userNameDisp = document.createElement("h5");
    userNameDisp.className = "headerMargin";
    userNameDisp.innerHTML = "<b>" + ticket.UserName + "</b> <b>" + ticket.UserSurname + "</b>";
    cardDiv.appendChild(userNameDisp);

    var timeDisp = document.createElement("h5");
    timeDisp.className = "dateTime";
    timeDisp.innerText = ticket.TimeCreated;
    cardDiv.appendChild(timeDisp);

    var statusDisp = document.createElement("h5");
    statusDisp.className = "ticketStatus";
    statusDisp.id = "stdisp_" + ticket.Id;
    statusDisp.style.color = statusColorsJs[ticket.State];
    statusDisp.innerHTML = "<b>" + statusNames[ticket.State] + "</b>";
    statusDisp.onclick = function () { changeStateClick(tempId) };
    cardDiv.appendChild(statusDisp);

    if (ticket.CanDelete == true) {
        var deleteButton = document.createElement("h5");
        deleteButton.className = "deleteButton";
        deleteButton.innerText = "Delete";
        deleteButton.addEventListener("click", function () { deleteAndHide(tempId) });
        cardDiv.appendChild(deleteButton);
    }

    var cardText = document.createElement("div");
    cardText.className = "cardText";
    var ticketDescr = document.createElement("p");
    ticketDescr.innerText = ticket.Description;
    cardText.appendChild(ticketDescr);
    var themeText = document.createElement("div");
    themeText.className = "themeText";
    themeText.innerText = ticket.Type.Name;
    cardText.appendChild(themeText);
    cardDiv.appendChild(cardText);

    var divideLine = document.createElement("div");
    divideLine.className = "divideLine";
    cardDiv.appendChild(divideLine);

    var replyRect = document.createElement("div");
    replyRect.className = "replyRectangle";
    var showTicketText = document.createElement("div");
    showTicketText.className = "replyTextMargin";
    showTicketText.innerText = "Show";
    showTicketText.addEventListener("click", function () { showButtonClick(tempId) });
    replyRect.appendChild(showTicketText);
    if (ticket.CanEdit == true) {
        var editTicketText = document.createElement("div");
        editTicketText.className = "replyTextMargin";
        editTicketText.innerText = "Edit";
        editTicketText.addEventListener("click", function () { editButtonClick(tempId) });
        replyRect.appendChild(editTicketText);
    }
    var commentsTicketDisp = document.createElement("div");
    commentsTicketDisp.className = "replyTextMargin";
    commentsTicketDisp.innerHTML = "<img src=\"/Content/comments.png\" style=\"padding-bottom:1px;\" /><b> " + ticket.CommentsCount + "</b>";
    replyRect.appendChild(commentsTicketDisp);
    var childTicketDisp = document.createElement("div");
    childTicketDisp.className = "replyTextMargin";
    childTicketDisp.innerHTML = "<img src=\"/Content/ticket.png\" style=\"padding-bottom:1px;\" /><b> " + ticket.ChildTicketsCount + "</b>";
    replyRect.appendChild(childTicketDisp);
    cardDiv.appendChild(replyRect);
}

function changeStateClick(id) {
    if (canChangeTicketState) {
        var statusRect = document.getElementById("strect_" + id);
        var statusDisp = document.getElementById("stdisp_" + id);

        var indexOfCurrent = statusColorsJs.indexOf(statusRect.style.backgroundColor);
        if (indexOfCurrent >= 3) {
            indexOfCurrent = 0;
        }
        else {
            indexOfCurrent++;
        }

        statusRect.style.backgroundColor = statusColorsJs[indexOfCurrent];
        statusDisp.style.color = statusColorsJs[indexOfCurrent];
        statusDisp.innerHTML = "<b>" + statusNames[indexOfCurrent] + "</b>";
        sendTicketState(id, indexOfCurrent);
    }
}

function sendTicketState(id, state) {
    var uploaderUrl = "/Ticket/ChangeTicketState";
    var formData = new FormData();
    formData.append("ticketId", id);
    formData.append("state", state);
    var xhr = new XMLHttpRequest();
    xhr.open('POST', uploaderUrl, true);
    xhr.onloadend = function () {
        var parsedTicket = JSON.parse(xhr.responseText);
        if (parsedTicket != null) {
            console.log(parsedTicket);
        }
    }
    xhr.send(formData);
}

function editButtonClick(id) {
    window.location.href = "/Ticket/Edit?id=" + id;
}

function showButtonClick(id) {
    window.location.href = "/Ticket/ShowTicket?id=" + id;
}

function getTickets(teamId, typeId) {
    var uploaderUrl = "/Ticket/GetTicketsByTeamAndType";
    var formData = new FormData();
    formData.append("teamId", teamId);
    formData.append("typeId", typeId);
    var xhr = new XMLHttpRequest();
    xhr.open('POST', uploaderUrl, true);
    xhr.onloadend = function () {
        var parsedTicket = JSON.parse(xhr.responseText);
        if (parsedTicket != null) {
            displayNewTicket(parsedTicket);
        }
    }
    xhr.send(formData);
}

function deleteAndHide(delId) {
    deleteTicket(delId);
    var delTicket = document.getElementById("ticket_" + delId);
    delTicket.style.display = "none";
}

function deleteTicket(id) {
    var uploaderUrl = "/Ticket/DeleteTicket?id=" + id;
    var xhr = new XMLHttpRequest();
    xhr.open('GET', uploaderUrl, true);
    xhr.onloadend = function () {
        console.log(xhr.responseText);
        var parsedResp = JSON.parse(xhr.responseText);
        if (parsedResp != null) {
            console.log("Ticket deletion result - " + parsedResp);
        }
    }
    xhr.send(null);
}

function getTeamPermissions(id) {
    var uploaderUrl = "/Role/GetUserTeamPermissions?teamId=" + id;
    var xhr = new XMLHttpRequest();
    xhr.open('GET', uploaderUrl, true);
    xhr.onloadend = function () {
        var parsedResp = JSON.parse(xhr.responseText);
        if (parsedResp != null) {
            teamPermissions = parsedResp;
            canChangeTicketState = teamPermissions.CanChangeTicketState;
        }
    }
    xhr.send(null);
}

function clearTicketsDiv() {
    var ticketsDiv = document.getElementById("ticketsDiv");
    ticketsDiv.innerHTML = "";
}

function showAllTickets(id, type) {
    teamId = id;
    clearTicketsDiv();
    getTickets(teamId, type);
    getTeamPermissions(teamId);
}