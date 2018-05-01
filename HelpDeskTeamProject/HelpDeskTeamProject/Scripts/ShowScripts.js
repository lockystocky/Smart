function addTicket() {
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
        var escapedText = escape(textBox.value);
        uploadTicket(escapedText, typeChoser.value);
        textBox.value = "";
        typeChoser.value = "0";
    }
}

function uploadTicket(text, type) {
    var uploaderUrl = "/Ticket/AddTicket";
    var formData = new FormData();
    formData.append("Description", text);
    formData.append("TypeId", type);
    formData.append("BaseTicketId", curTicketId);
    formData.append("BaseTeamId", curTeamId);
    var xhr = new XMLHttpRequest();
    xhr.open('POST', uploaderUrl, true);
    xhr.onloadend = function () {
        var parsedTicket = JSON.parse(xhr.responseText);
        if (parsedTicket != null) {
            displayNewTicket(parsedTicket);
            getLastLog();
        }
    };
    xhr.send(formData);
}

function commentsOrLogsButtonClick() {
    var curButton = document.getElementById("commentsOrLogsButton");
    var commentsDiv = document.getElementById("mainDisplay");
    var logsDiv = document.getElementById("logsDisplay");
    var inputCommentDiv = document.getElementById("inputCommentDiv");
    if (commentsOrLogs) {//show logs
        inputCommentDiv.style.display = "none";
        commentsDiv.style.display = "none";
        logsDiv.style.display = "";
        commentsOrLogs = false;
        curButton.innerText = "Comments";
    } else {//show comments
        inputCommentDiv.style.display = "";
        commentsDiv.style.display = "";
        logsDiv.style.display = "none";
        commentsOrLogs = true;
        curButton.innerText = "Logs";
    }
}

function editButtonClick(id) {
    window.location.href = "/Ticket/Edit?id=" + id;
}

function showButtonClick(id) {
    window.location.href = "/Ticket/ShowTicket?id=" + id;
}

function clearTicketsDiv() {
    var ticketsDiv = document.getElementById("ticketsDisplay");
    ticketsDiv.innerHTML = "";
}

function clearCommentsDiv() {
    var commentsDiv = document.getElementById("mainDisplay");
    commentsDiv.innerHTML = "";
}

function clearLogsDiv() {
    var logsDiv = document.getElementById("logsDisplay");
    logsDiv.innerHTML = "";
}

function displayNewTicket(ticket) {
    if (emptyTickets == true) {
        clearTicketsDiv();
        emptyTickets = false;
    }

    var ticketsDisplay = document.getElementById("ticketsDisplay");
    var curTicketChildsCount = document.getElementById("ticketChildsCount");
    curTicketChildsCount.innerText = " " + (parseInt(curTicketChildsCount.innerText) + 1);
    var tempId = ticket.Id;

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
    statusDisp.onclick = function () { changeStateClick(tempId); };
    cardDiv.appendChild(statusDisp);

    var deleteButton = document.createElement("h5");
    deleteButton.className = "deleteButton";
    deleteButton.innerText = "Delete";
    deleteButton.addEventListener("click", function () { deleteAndHide(tempId); });
    cardDiv.appendChild(deleteButton);

    var cardText = document.createElement("div");
    cardText.className = "cardText";
    var ticketDescr = document.createElement("p");
    ticketDescr.innerHTML = ticket.Description;
    cardText.appendChild(ticketDescr);
    var themeText = document.createElement("div");
    themeText.className = "themeText";
    themeText.onclick = function () { window.location.href = "/Ticket/Filter"; };
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
    showTicketText.addEventListener("click", function () { showButtonClick(tempId); });
    replyRect.appendChild(showTicketText);
    if (ticket.CanEdit == true) {
        var editTicketText = document.createElement("div");
        editTicketText.className = "replyTextMargin";
        editTicketText.innerText = "Edit";
        editTicketText.addEventListener("click", function () { editButtonClick(tempId); });
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
        getLastLog();
    };
    xhr.send(formData);
}

function addComment() {
    var commentBox = document.getElementById("addCommentText");
    if (commentBox.value == "") {
        commentBox.style.border = "groove";
        commentBox.style.borderColor = "orangered";
        commentBox.style.borderWidth = "1.5px";
    }
    else {
        commentBox.style.border = "";
        commentBox.style.borderColor = "";
        commentBox.style.borderWidth = "";
        var escapedText = escape(commentBox.value);
        uploadComment(escapedText);
        commentBox.value = "";
    }
}

function deleteThisTicket() {
    deleteTicket(curTicketId);
    window.location.href = "/Ticket/Tickets";
}

function deleteAndHide(delId) {
    deleteTicket(delId);
    var curTicketChildsCount = document.getElementById("ticketChildsCount");
    curTicketChildsCount.innerText = " " + (parseInt(curTicketChildsCount.innerText) - 1);
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
            getLastLog();
            console.log("Ticket deletion result - " + parsedResp);
        }
    };
    xhr.send(null);
}

function deleteComAndHide(delId) {
    deleteComment(delId);
    var curTicketCommentsCount = document.getElementById("ticketCommentsCount");
    curTicketCommentsCount.innerText = " " + (parseInt(curTicketCommentsCount.innerText) - 1);
    var delCom = document.getElementById("comment_" + delId);
    delCom.style.display = "none";
}

function deleteComment(id) {
    var uploaderUrl = "/Ticket/DeleteComment?id=" + id;
    var xhr = new XMLHttpRequest();
    xhr.open('GET', uploaderUrl, true);
    xhr.onloadend = function () {
        console.log(xhr.responseText);
        var parsedResp = JSON.parse(xhr.responseText);
        if (parsedResp != null) {
            getLastLog();
            console.log("Comment deletion result - " + parsedResp);
        }
    };
    xhr.send(null);
}

function uploadComment(text) {
    var uploaderUrl = "/Ticket/AddComment";
    var formData = new FormData();
    formData.append("ticketId", curTicketId);
    formData.append("text", text);
    var xhr = new XMLHttpRequest();
    xhr.open('POST', uploaderUrl, true);
    xhr.onloadend = function () {
        var parsedCom = JSON.parse(xhr.responseText);
        if (parsedCom != null) {
            displayNewComment(parsedCom);
            getLastLog();
        }
    };
    xhr.send(formData);
}

function displayNewComment(comment) {
    if (emptyComments == true) {
        clearCommentsDiv();
        emptyComments = false;
    }
    var mainDisp = document.getElementById("mainDisplay");
    var curTicketCommentsCount = document.getElementById("ticketCommentsCount");
    curTicketCommentsCount.innerText = " " + (parseInt(curTicketCommentsCount.innerText) + 1);

    var cardDiv = document.createElement("div");
    cardDiv.className = "card";
    cardDiv.id = "comment_" + comment.Id;
    mainDisp.appendChild(cardDiv);

    var userNameDisp = document.createElement("h5");
    userNameDisp.className = "headerMargin";
    userNameDisp.innerHTML = "<b>" + comment.UserName + "</b> <b>" + comment.UserSurname + "</b>";
    cardDiv.appendChild(userNameDisp);

    var timeDisp = document.createElement("h5");
    timeDisp.className = "dateTime";
    timeDisp.innerText = comment.TimeCreated;
    cardDiv.appendChild(timeDisp);

    var deleteButton = document.createElement("h5");
    deleteButton.className = "deleteButton";
    deleteButton.innerText = "Delete";
    deleteButton.addEventListener("click", function () { deleteComAndHide(comment.Id); });
    cardDiv.appendChild(deleteButton);

    var comText = document.createElement("div");
    comText.className = "commentText";
    var comPar = document.createElement("p");
    comPar.innerHTML = comment.Text;
    comText.appendChild(comPar);
    cardDiv.appendChild(comText);
}

function setVisibilityInputs(comment, ticket) {
    var commentInput = document.getElementById("inputCommentDiv");
    var ticketInput = document.getElementById("inputTicketDiv");
    if (comment == true) {
        commentInput.style.display = "";
    }
    else {
        commentInput.style.display = "none";
    }
    if (ticket == true) {
        ticketInput.style.display = "";
    }
    else {
        ticketInput.style.display = "none";
    }
}

function getTeamPermissions() {
    var uploaderUrl = "/Role/GetUserTeamPermissions?teamId=" + curTeamId;
    var xhr = new XMLHttpRequest();
    xhr.open('GET', uploaderUrl, true);
    xhr.onloadend = function () {
        var parsedResp = JSON.parse(xhr.responseText);
        if (parsedResp != null) {
            teamPermissions = parsedResp;
            canChangeTicketState = teamPermissions.CanChangeTicketState;
            setVisibilityInputs(teamPermissions.CanCommentTicket, teamPermissions.CanCreateTicket);
        }
    };
    xhr.send(null);
}

function themeTextClick() {
    window.location.href = "/Ticket/Filter";
}

function displayNewLog(log) {
    if (emptyLogs == true) {
        clearLogsDiv();
        emptyLogs = false;
    }
    var mainDisp = document.getElementById("logsDisplay");

    var cardDiv = document.createElement("div");
    cardDiv.className = "card";
    mainDisp.appendChild(cardDiv);

    var userNameDisp = document.createElement("h5");
    userNameDisp.className = "headerMargin";
    userNameDisp.innerHTML = "<b>" + log.UserName + "</b> <b>" + log.UserSurname + "</b>";
    cardDiv.appendChild(userNameDisp);

    var timeDisp = document.createElement("h5");
    timeDisp.className = "dateTime";
    timeDisp.innerText = log.Time;
    cardDiv.appendChild(timeDisp);

    var comText = document.createElement("div");
    comText.className = "commentText";
    var comPar = document.createElement("p");
    comPar.innerText = log.Text;
    comText.appendChild(comPar);
    cardDiv.appendChild(comText);
}

function getLastLog() {
    var uploaderUrl = "/Ticket/GetLastTicketLog?ticketId=" + curTicketId;
    var xhr = new XMLHttpRequest();
    xhr.open('GET', uploaderUrl, true);
    xhr.onloadend = function () {
        var parsedResp = JSON.parse(xhr.responseText);
        if (parsedResp != null && parsedResp !== false) {
            displayNewLog(parsedResp);
        }
    };
    xhr.send(null);
}