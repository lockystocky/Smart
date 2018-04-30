function changeUserRoleInTeam(userId, teamId, roleId) {
    $.ajax({
        type: "POST",
        url: "/teams/changeUserRoleInTeam/",
        data: "{'userId':'" + userId + "', teamId:'" + teamId + "', roleId:'" + roleId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    });
}

function deleteUserFromTeam(userId, teamId) {
    $.ajax({
        type: "POST",
        url: "/teams/DeleteUserFromTeam/",
        data: "{'userId':'" + userId + "', teamId:'" + teamId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    }).done(function () {
        alert('Ok');
    });
}


function reinviteUserToTeam(userId, teamId){
    $.ajax({
        type: "POST",
        url: "/teams/reinviteUserToTeam/",
        data: "{'invUserId':'" + userId + "', teamId:'" + teamId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    }).done(function (data) {
        if (!data) {
            var errorMsg = $('#inverror').clone();
            $(errorMsg).removeClass('hidden');
            $('#invmsg').text('');
            $('#invmsg').append(errorMsg);
        }
        else {
            var successMsg = $('#invsuc').clone();
            $(successMsg).removeClass('hidden');
            $('#invmsg').text('');
            $('#invmsg').append(successMsg);
        }
    });
}

$(function () {

    $('.savebtn').click(function () {
        var userId = $(this).parents('.userinfo').attr('id');
        var teamId = $(this).parents('.team').attr('id');
        var roleId = $(this).parent().siblings('.role').children('select').find(':selected').val();
        changeUserRoleInTeam(userId, teamId, roleId);
    });

    $('.delbtn').click(function () {
        var userId = $(this).parents('.userinfo').attr('id');
        var teamId = $(this).parents('.team').attr('id');
        var isReadyToDeleteUser = confirm("Are you sure want to delete user from team?");
        if (isReadyToDeleteUser) {
            deleteUserFromTeam(userId, teamId);
            $(this).parents('.userinfo').hide();
        }
    });

    $('.invbtn').click(function () {
        var userId = $(this).parents('.inviteduser').attr('id');
        var teamId = $(this).parents('.invitedusers').attr('id');
        reinviteUserToTeam(userId, teamId);
    });

});