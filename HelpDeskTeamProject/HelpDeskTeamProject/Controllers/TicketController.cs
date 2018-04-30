using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelpDeskTeamProject.DataModels;
using HelpDeskTeamProject.Context;
using System.Threading.Tasks;
using System.Data.Entity;
using HelpDeskTeamProject.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using HelpDeskTeamProject.Services;
using HelpDeskTeamProject.Classes;

namespace HelpDeskTeamProject.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        IAppContext db;
        IUserManager userManager;
        ITicketLogger ticketLogger;
        ITicketTypeManager typeManager;
        ICommentManager commentManager;
        ITicketManager ticketManager;
        IDtoConverter dtoConverter;
        IHtmlValidator htmlValidator;

        public TicketController(IAppContext context, ITicketTypeManager typeMan, ITicketLogger tickLog, ICommentManager comManage, ITicketManager tikMan, IUserManager userMan, IDtoConverter dtoConver, IHtmlValidator htmlValid)
        {
            db = context;
            typeManager = typeMan;
            ticketLogger = tickLog;
            commentManager = comManage;
            ticketManager = tikMan;
            userManager = userMan;
            dtoConverter = dtoConver;
            htmlValidator = htmlValid;
        }

        public ActionResult NoPermissionError()
        {
            return View();
        }

        public async Task<ActionResult> Filter()
        {
            User curUser = await userManager.GetCurrentUser();
            if (curUser != null)
            {
                await FillTypeList();
                List<Team> teams = curUser.Teams;
                return View(teams);
            }
            return RedirectToAction("Tickets", "Ticket");
        }

        [HttpPost]
        public async Task<JsonResult> ChangeTicketState(int? ticketId, int? state)
        {
            if (ticketId != null && state != null && ticketId > 0 && state >= 0 && state <= 3)
            {
                User curUser = await userManager.GetCurrentUser();
                Ticket ticket = await ticketManager.GetTicketNoInclude(ticketId);
                if (curUser != null && ticket != null)
                {
                    TeamPermissions teamPerms = await GetCurrentTeamPermissions(ticket.TeamId, curUser.Id);
                    if (curUser.AppRole.Permissions.IsAdmin || teamPerms.CanChangeTicketState)
                    {
                        await ticketManager.ChangeState(ticketId, state, curUser);
                        return Json(true);
                    }
                }
            }
            return Json(false);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id != null)
            {
                await FillTypeList();
                Ticket ticket = await db.Tickets.Include(z => z.User).Include(x => x.ChildTickets).Include(y => y.Comments)
                    .SingleOrDefaultAsync(x => x.Id == id);
                User curUser = await userManager.GetCurrentUser();
                TeamPermissions teamPerms = await GetCurrentTeamPermissions(ticket.TeamId, curUser.Id);
                if (ticket != null && curUser != null && teamPerms != null)
                {
                    if (ticket.User.Id == curUser.Id || curUser.AppRole.Permissions.IsAdmin || teamPerms.CanEditTickets)
                    {
                        return View(ticket);
                    }
                    else
                    {
                        return RedirectToAction("NoPermissionError", "Ticket");
                    }
                }
            }
            return RedirectToAction("Tickets", "Ticket");
        }

        [HttpPost]
        public async Task<JsonResult> EditSave(int? id, string description, int? type)
        {
            if (id != null && description != null && type != null && description != "")
            {
                Ticket ticket = await ticketManager.GetTicketNoInclude(id);
                User curUser = await userManager.GetCurrentUser();
                TeamPermissions teamPerms = await GetCurrentTeamPermissions(ticket.TeamId, curUser.Id);
                if (ticket.User.Id == curUser.Id || curUser.AppRole.Permissions.IsAdmin || teamPerms.CanEditTickets)
                {
                    TicketType newType = await db.TicketTypes.SingleOrDefaultAsync(x => x.Id == type);
                    if (ticket != null && newType != null)
                    {
                        await ticketManager.Edit(id, description, type, curUser);
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> DeleteComment(int? id)
        {
            if (id != null)
            {
                Comment comment = await db.Comments.SingleOrDefaultAsync(x => x.Id == id);
                Ticket curTicket = await ticketManager.GetTicketNoInclude(comment.BaseTicketId);
                User curUser = await userManager.GetCurrentUser();
                TeamPermissions teamPerms = await GetCurrentTeamPermissions(comment.TeamId, curUser.Id);
                if (comment != null && teamPerms != null)
                {
                    if (comment.User.Id == curUser.Id || curUser.AppRole.Permissions.IsAdmin || teamPerms.CanDeleteComments)
                    {
                        bool result = await commentManager.Delete(id, curUser, curTicket);
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> DeleteTicket(int? id)
        {
            if (id != null)
            {
                Ticket ticket = await db.Tickets.Include(z => z.ParentTicket).SingleOrDefaultAsync(x => x.Id == id);
                User curUser = await userManager.GetCurrentUser();
                TeamPermissions teamPerms = await GetCurrentTeamPermissions(ticket.TeamId, curUser.Id);
                if (ticket != null && teamPerms != null)
                {
                    if (ticket.User.Id == curUser.Id || curUser.AppRole.Permissions.IsAdmin || teamPerms.CanDeleteTickets)
                    {
                        await ticketManager.Delete(id, curUser);
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> NewType()
        {
            User curUser = await userManager.GetCurrentUser();
            if (curUser != null)
            {
                if (curUser.AppRole.Permissions.CanManageTicketTypes || curUser.AppRole.Permissions.IsAdmin)
                {
                    return View();
                }
            }
            return RedirectToAction("NoPermissionError", "Ticket");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> NewType(TicketType newType)
        {
            if (ModelState.IsValid)
            {
                User curUser = await userManager.GetCurrentUser();
                if (curUser != null)
                {
                    if (curUser.AppRole.Permissions.CanManageTicketTypes || curUser.AppRole.Permissions.IsAdmin)
                    {
                        if (await typeManager.CreateNew(newType))
                        {
                            return RedirectToAction("TypeList", "Ticket");
                        }
                        else
                        {
                            ModelState.AddModelError("TicketNameExists", "Ticket type with that name already exists.");
                            return View(newType);
                        }
                    }
                }
                return RedirectToAction("NoPermissionError", "Ticket");
            }
            return View(newType);
        }

        public async Task<ActionResult> TypeList()
        {
            User curUser = await userManager.GetCurrentUser();
            if (curUser.AppRole.Permissions.CanManageTicketTypes || curUser.AppRole.Permissions.IsAdmin)
            {
                List<TicketType> ticketTypes = await typeManager.GetAllTypes();
                return View(ticketTypes);
            }
            return RedirectToAction("NoPermissionError", "Ticket");
        }

        public async Task<JsonResult> GetTicketsByTeam(int? teamId)
        {
            if (teamId != null)
            {
                Team curTeam = await db.Teams.Include(x => x.Tickets).SingleOrDefaultAsync(y => y.Id == teamId);
                if (curTeam != null)
                {
                    User curUser = await userManager.GetCurrentUser();
                    if (curTeam.Users.Find(x => x.Id == curUser.Id) != null || curUser.AppRole.Permissions.IsAdmin)
                    {
                        UserPermission curUserPerms = curTeam.UserPermissions.SingleOrDefault(x => x.User.Id == curUser.Id);
                        if ((curUser != null && curUserPerms != null) || (curUser != null && curUser.AppRole.Permissions.IsAdmin))
                        {
                            TeamRole curTeamUserRole = null;
                            if (curUserPerms != null)
                            {
                                curTeamUserRole = curUserPerms.TeamRole;
                            }
                            List<Ticket> curTickets = await ticketManager.GetTicketsByTeam(teamId);
                            List<TicketDTO> curTicketsDto = dtoConverter.ConvertTicketList(curTickets, curUser, curTeamUserRole.Permissions);
                            return Json(curTicketsDto, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            return Json(null);
        }

        public async Task<ActionResult> Tickets()
        {
            await FillTypeList();
            return View();
        }

        public async Task<ActionResult> ShowTicket(int? id)
        {
            if (id != null)
            {
                User curUser = await userManager.GetCurrentUser();
                Ticket ticket = await db.Tickets.Include(y => y.User).Include(s => s.ChildTickets)
                    .SingleOrDefaultAsync(x => x.Id == id);
                ticket.ChildTickets = await db.Tickets.Include(z => z.User).Include(e => e.Type).Include(y => y.ChildTickets).Include(w => w.Comments)
                    .Where(x => x.ParentTicket.Id == ticket.Id).ToListAsync();
                Team team = await db.Teams.Include(x => x.UserPermissions).SingleOrDefaultAsync(y => y.Id == ticket.TeamId);
                UserPermission teamUserPerms = team.UserPermissions.SingleOrDefault(x => x.User.Id == curUser.Id);
                TeamPermissions teamPerms = null;
                if (teamUserPerms != null)
                {
                    teamPerms = teamUserPerms.TeamRole.Permissions;
                }

                if (team.Users.Find(x => x.Id == curUser.Id) != null || curUser.AppRole.Permissions.IsAdmin)
                {
                    TicketDTO ticketDto = dtoConverter.ConvertTicket(ticket, curUser, teamPerms);
                    ticketDto.ChildTickets = dtoConverter.ConvertTicketList(ticket.ChildTickets, curUser, teamPerms);
                    ticketDto.Comments = dtoConverter.ConvertCommentList(ticket.Comments, curUser, teamPerms);
                    ticketDto.Logs = dtoConverter.ConvertTicketLogList(ticket.Logs);
                    await FillTypeList();
                    return View(ticketDto);
                }
            }
            return RedirectToAction("Tickets", "Ticket");
        }

        [HttpPost]
        public async Task<JsonResult> AddTicket(TicketBase newTicket)
        {
            User curUser = await userManager.GetCurrentUser();
            string unescapedText = HttpUtility.UrlDecode(newTicket.Description);
            //unescapedText = htmlValidator.ValidateHtml(unescapedText);
            TeamPermissions userPerms = await GetCurrentTeamPermissions(newTicket.BaseTeamId, curUser.Id);
            if (userPerms.CanCreateTicket == true || curUser.AppRole.Permissions.IsAdmin == true)
            {
                Ticket createdTicket = await ticketManager.Add(newTicket, curUser);
                TicketDTO ticketDto = dtoConverter.ConvertTicket(createdTicket, curUser, userPerms);
                return Json(ticketDto);
            }
            return Json(null);
        }

        [HttpPost]
        public async Task<JsonResult> AddComment(int? ticketId, string text)
        {
            if (text != null && ticketId != null && ticketId > 0)
            {
                User curUser = await userManager.GetCurrentUser();
                Ticket curTicket = await db.Tickets.Include(y => y.Comments).SingleOrDefaultAsync(x => x.Id == ticketId);
                TeamPermissions userPerms = await GetCurrentTeamPermissions(curTicket.TeamId, curUser.Id);
                if (userPerms.CanCommentTicket || curUser.AppRole.Permissions.IsAdmin)
                {
                    Comment newComment = await commentManager.Add(curUser, curTicket, text);
                    CommentDTO commentToJs = dtoConverter.ConvertComment(newComment, curUser, userPerms);
                    return Json(commentToJs);
                }
            }
            return Json(null);
        }

        [HttpPost]
        public async Task<JsonResult> GetTicketsByTeamAndType(int? teamId, int? typeId)
        {
            if (teamId != null && typeId != null && teamId > 0 && typeId > 0)
            {
                User curUser = await userManager.GetCurrentUser();
                TicketType curType = await db.TicketTypes.SingleOrDefaultAsync(x => x.Id == typeId);
                Team curTeam = await db.Teams.SingleOrDefaultAsync(x => x.Id == teamId);
                if (curTeam != null && curType != null && curUser != null)
                {
                    if (curTeam.Users.Find(x => x.Id == curUser.Id) != null)
                    {
                        TeamPermissions teamPerms = curTeam.UserPermissions.SingleOrDefault(x => x.User.Id == curUser.Id).TeamRole.Permissions;
                        List<Ticket> ticketsList = await ticketManager.GetTicketsByTeamAndType(teamId, typeId);
                        List<TicketDTO> dtoTicketsList = dtoConverter.ConvertTicketList(ticketsList, curUser, teamPerms);
                        return Json(dtoTicketsList);
                    }
                }
            }
            return Json(false);
        }

        public async Task<JsonResult> GetLastTicketLog(int? ticketId)
        {
            if (ticketId != null && ticketId > 0)
            {
                User curUser = await userManager.GetCurrentUser();
                List<TicketLog> curTicketLogs = await db.TicketLogs.Where(x => x.TicketId == ticketId && x.User.Id == curUser.Id).ToListAsync();
                if (curTicketLogs != null)
                {
                    TicketLog lastLog = curTicketLogs.SingleOrDefault(x => x.Time.Ticks == curTicketLogs.Max(z => z.Time.Ticks));
                    if (lastLog != null)
                    {
                        TicketLogDTO tempDto = dtoConverter.ConvertTicketLog(lastLog);
                        return Json(tempDto, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        private async Task FillTypeList()
        {
            List<TicketType> ticketTypes = await typeManager.GetAllTypes();
            ViewBag.TicketTypes = ticketTypes;
        }

        private async Task<TeamPermissions> GetCurrentTeamPermissions(int ticketTeamId, int curUserId)
        {
            Team team = await db.Teams.SingleOrDefaultAsync(x => x.Id == ticketTeamId);
            UserPermission userPerms = team.UserPermissions.SingleOrDefault(x => x.User.Id == curUserId);
            if (userPerms != null)
            {
                TeamPermissions teamPerms = userPerms.TeamRole.Permissions;
                return teamPerms;
            }
            return new TeamPermissions();
        }
    }
}