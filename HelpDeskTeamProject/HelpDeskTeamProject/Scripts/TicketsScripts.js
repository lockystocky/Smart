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
        sysMessageDiv.innerText = "This team does not have any child tickets, try adding more by clicking add button.";
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
    statusDisp.onclick = function () { changeStateClick(tempId); };
    cardDiv.appendChild(statusDisp);

    if (ticket.CanDelete == true) {
        var deleteButton = document.createElement("h5");
        deleteButton.className = "deleteButton";
        deleteButton.innerText = "Delete";
        deleteButton.addEventListener("click", function () { deleteAndHide(tempId); });
        cardDiv.appendChild(deleteButton);
    }

    var cardText = document.createElement("div");
    cardText.className = "cardText";
    var ticketDescr = document.createElement("p");
    ticketDescr.innerText = ticket.Description;
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
    };
    xhr.send(formData);
}

function editButtonClick(id) {
    window.location.href = "/Ticket/Edit?id=" + id;
}

function showButtonClick(id) {
    window.location.href = "/Ticket/ShowTicket?id=" + id;
}

function getTickets(teamId) {
    var uploaderUrl = "/Ticket/GetTicketsByTeam?teamId=" + teamId;
    var xhr = new XMLHttpRequest();
    xhr.open('GET', uploaderUrl, true);
    xhr.onloadend = function () {
        var parsedTickets = JSON.parse(xhr.responseText);
        if (parsedTickets != null) {
            displayNewTicket(parsedTickets);
        }
    };
    xhr.send(null);
}

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
        //var escapedValue = window.escape(textBox.value);
        uploadTicket(textBox.value, typeChoser.value, teamId);
        textBox.value = "";
        typeChoser.value = "0";
    }
}

function uploadTicket(text, type, teamId) {
    var uploaderUrl = "/Ticket/AddTicket";
    var formData = new FormData();
    formData.append("Description", text);
    formData.append("TypeId", type);
    formData.append("BaseTicketId", null);
    formData.append("BaseTeamId", teamId);
    var xhr = new XMLHttpRequest();
    xhr.open('POST', uploaderUrl, true);
    xhr.onloadend = function () {
        var parsedTicket = JSON.parse(xhr.responseText);
        if (parsedTicket != null) {
            displayOneTicket(parsedTicket);
        }
    };
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
    };
    xhr.send(null);
}

function setVisibilityInputTicket(visible) {
    var ticketInput = document.getElementById("newTicketInput");
    if (visible == true) {
        ticketInput.style.display = "";
    }
    else {
        ticketInput.style.display = "none";
    }
}

function clearTicketsDiv() {
    var ticketsDiv = document.getElementById("ticketsDiv");
    ticketsDiv.innerHTML = "";
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
            setVisibilityInputTicket(teamPermissions.CanCreateTicket);
        }
    };
    xhr.send(null);
}

function showByTeam(id) {
    teamId = id;
    clearTicketsDiv();
    getTickets(teamId);
    getTeamPermissions(teamId);
}

function createTeamsMenu(teamsList) {
        $.each(teamsList, function () {
            var team = this;
            var menuItem = $('<div class="team-name list-group-item list-group-item-action flex-column align-items-start teamHover" style="margin-right:0px">');
            menuItem.attr('id', team.TeamId);
            var teamIcon = $('<img class="group-icon">');
            teamIcon.attr('src', '/Content/Icons/group_icon.png');
            var teamHeader = $('<p>');
            teamHeader.append(teamIcon);
            if (team.TeamName.length > 15)
                team.TeamName = team.TeamName.slice(0, 12) + "...";
            teamHeader.append(team.TeamName);
            menuItem.append(teamHeader);
            
            $('#teamsmenu').append(menuItem);
            menuItem.click(function () {
                var shaded = $('.shade:last').removeClass('shade');
                $(this).addClass('shade');
                var team_id = $(this).attr('id');
                
                 //here must be call of Andrew function to show tickets of team
                showByTeam(team_id);
                getManageTeamLink(team_id);

            });
        });
    };

    function getTeamsListAndCreateTeamsMenu() {
        $.getJSON("/Teams/GetCurrentUserTeamsList/")
            .done(function (teamsList) {
                if (teamsList.length > 0) {
                    $('#noteamsmsg').text('');
                    createTeamsMenu(teamsList);
                }
                else {
                    $('#noteamsmsg').text('There are no teams associated with your account, create one or ask somebody to invite you');
                }
            });
    };

    function getManageTeamLink(teamId) {        
        $.get("/teams/GetTeamManagementLink?_teamId=" + teamId, function (data) {
            if (data) {
                $('#manageteamdiv').removeClass('hidden');
                $('#managetext').text('Manage team');
                $('#manageteamdiv').click(function () {
                    window.location.href = data;
                });
            }
            else {
                $('#manageteamdiv').addClass('hidden');
                $('#manageteamdiv').click(function () { });
            }
           
        });

    }

    $(function () {
        getTeamsListAndCreateTeamsMenu();
    });
