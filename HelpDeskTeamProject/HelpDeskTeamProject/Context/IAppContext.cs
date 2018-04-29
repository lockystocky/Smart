using HelpDeskTeamProject.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskTeamProject.Context
{
    public interface IAppContext
    {
        DbSet<User> Users { get; set; }

        DbSet<Team> Teams { get; set; }

        DbSet<TeamRole> TeamRoles { get; set; }

        DbSet<ApplicationRole> AppRoles { get; set; }

        DbSet<AdminLog> AdminLogs { get; set; }

        DbSet<UserPermission> Permissions { get; set; }

        DbSet<Ticket> Tickets { get; set; }

        DbSet<TicketType> TicketTypes { get; set; }

        DbSet<Comment> Comments { get; set; }

        DbSet<TicketLog> TicketLogs { get; set; }

        int SaveChanges();

        Task<int> SaveChangesAsync();

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
