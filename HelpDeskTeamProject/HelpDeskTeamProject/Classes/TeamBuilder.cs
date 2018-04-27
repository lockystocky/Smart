using HelpDeskTeamProject.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.Classes
{
   
    class TeamDirector
    {
        TeamBuilder builder;
        public TeamDirector(TeamBuilder builder)
        {
            this.builder = builder;
        }
        public void Construct()
        {
            builder.BuildName();
            builder.BuildGuid();
            builder.BuildInvitationLink();
            builder.BuildInvitedUsers();
            builder.BuildUsersPermission();
            builder.BuildUsers();
            builder.BuildTickets();
        }
    }

    abstract class TeamBuilder
    {
        public abstract void BuildName();
        public abstract void BuildGuid();
        public abstract void BuildInvitationLink();
        public abstract void BuildInvitedUsers();
        public abstract void BuildTickets();
        public abstract void BuildUsersPermission();
        public abstract void BuildUsers();
        public abstract TeamProduct GetResult();
    }

    class TeamProduct
    {
        Team team = new Team();

        public void AddName(string  name)
        {
            team.Name = name;
        }

        public void AddOwner(int ownerId)
        {
            team.OwnerId = ownerId;
        }

        public void AddGuid(Guid guid)
        {
            team.TeamGuid = guid;
        }

        public void AddInvitationLink(string link)
        {
            team.InvitationLink = link;
        }

        public void AddInvitedUsers()
        {
            team.InvitedUsers = new List<InvitedUser>();
        }
        

        public void AddTickets()
        {
            team.Tickets = new List<Ticket>();
        }

        public void AddUsers()
        {
            team.Users = new List<User>();
        }

        public void AddUsersPermission()
        {
            team.UserPermissions = new List<UserPermission>();
        }

        public Team GetCreatedTeam()
        {
            return team;
        }
    }

    class DefaultTeamBuilder : TeamBuilder
    {
        Guid teamGuid;
        string name;
        int ownerId;
        string invitationLink;

        public DefaultTeamBuilder(Guid teamGuid, string name, int ownerId, string invitationLink)
        {
            this.teamGuid = teamGuid;
            this.name = name;
            this.ownerId = ownerId;
            this.invitationLink = invitationLink;
        }

        TeamProduct product = new TeamProduct();

        public override void BuildUsers()
        {
            product.AddUsers();
        }

        public override void BuildName()
        {
            product.AddName(name);
        }

        public override void BuildGuid()
        {
            product.AddGuid(teamGuid);
        }

        public override void BuildInvitedUsers()
        {
            product.AddInvitedUsers();
        }

        public override void BuildInvitationLink()
        {
            product.AddInvitationLink(invitationLink);
        }

        public override void BuildTickets()
        {
            product.AddTickets();
        }

        public override void BuildUsersPermission()
        {
            product.AddUsersPermission();
        }

        public override TeamProduct GetResult()
        {
            return product;
        }
    }
}