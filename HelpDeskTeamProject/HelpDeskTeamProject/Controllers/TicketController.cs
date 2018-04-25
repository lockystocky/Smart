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

namespace HelpDeskTeamProject.Controllers
{
    public class TicketController : Controller
    {
        AppContext db = new AppContext();

        public async Task<ActionResult> Edit(int? id)
        {
            if (id != null)
            {
                List<TicketType> ticketTypes = await db.TicketTypes.ToListAsync();
                ViewBag.TicketTypes = ticketTypes;
                Ticket ticket = await db.Tickets.Include(z => z.User).Include(x => x.ChildTickets).Include(y => y.Comments)
                    .SingleOrDefaultAsync(x => x.Id == id);
                if (ticket != null)
                {
                    return View(ticket);
                }
            }
            return RedirectToAction("Tickets", "Ticket");
        }

        [HttpPost]
        public async Task<JsonResult> EditSave(int? id, string description, int? type)
        {
            if (id != null && description != null && type != null && description != "")
            {
                Ticket ticket = await db.Tickets.SingleOrDefaultAsync(x => x.Id == id);
                TicketType newType = await db.TicketTypes.SingleOrDefaultAsync(x => x.Id == type);
                if (ticket != null && newType != null)
                {
                    ticket.Description = description;
                    ticket.Type = newType;
                    await db.SaveChangesAsync();
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> DeleteComment(int? id)
        {
            if (id != null)
            {
                Comment comment = await db.Comments.SingleOrDefaultAsync(x => x.Id == id);
                if (comment != null)
                {
                    db.Comments.Remove(comment);
                    await db.SaveChangesAsync();
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> DeleteTicket(int? id)
        {
            if (id != null)
            {
                Ticket ticket = await db.Tickets.SingleOrDefaultAsync(x => x.Id == id);
                db.Tickets.Remove(ticket);
                await db.SaveChangesAsync();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> NewType()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> NewType(TicketType newType)
        {
            if (ModelState.IsValid)
            {
                db.TicketTypes.Add(newType);
                await db.SaveChangesAsync();
                return RedirectToAction("TypeList", "Ticket");
            }
            return View(newType);
        }

        public async Task<ActionResult> TypeList()
        {
            List<TicketType> ticketTypes = await db.TicketTypes.ToListAsync();
            return View(ticketTypes);
        }

        public async Task<JsonResult> GetTicketsByTeam(int? teamId)
        {
            if (teamId != null)
            {
                Team curTeam = await db.Teams.Include(x => x.Tickets).SingleOrDefaultAsync(y => y.Id == teamId);
                if (curTeam != null)
                {
                    ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext()
                            .GetUserManager<ApplicationUserManager>()
                            .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                    User appUser = await db.Users.Include(z => z.Teams).SingleOrDefaultAsync(x => x.Email.ToLower().Equals(user.Email.ToLower()));
                    TeamRole curTeamUserRole = curTeam.UserPermissions.SingleOrDefault(x => x.UserId == appUser.Id).TeamRole;
                    if (appUser != null && curTeamUserRole != null)
                    {
                        List<Ticket> curTickets = await db.Tickets.Include(x => x.ChildTickets).Include(y => y.Comments).Include(z => z.User).Where(s => s.ParentTicket == null).ToListAsync();
                        List<TicketDTO> curTicketsDto = new List<TicketDTO>();
                        foreach (Ticket value in curTickets)
                        {
                            TicketDTO tempDto = new TicketDTO(value);
                            if (appUser.Id == value.User.Id || appUser.AppRole.Permissions.IsAdmin == true)
                            {
                                tempDto.CanDelete = true;
                                tempDto.CanEdit = true;
                            }
                            else
                            {
                                if (curTeamUserRole.Permissions.CanEditComments == true)
                                {
                                    tempDto.CanEdit = true;
                                }
                                else
                                {
                                    tempDto.CanEdit = false;
                                }
                                if (curTeamUserRole.Permissions.CanDeleteComments == true)
                                {
                                    tempDto.CanDelete = true;
                                }
                                else
                                {
                                    tempDto.CanDelete = false;
                                }
                            }
                            curTicketsDto.Add(tempDto);
                        }
                        return Json(curTicketsDto, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(null);
        }

        public async Task<ActionResult> Tickets()
        {
            List<TicketType> ticketTypes = await db.TicketTypes.ToListAsync();
            ViewBag.TicketTypes = ticketTypes;
            return View();
        }

        public async Task<ActionResult> TicketsList(int? teamId)
        {
            if (teamId != null)
            {
                Team team = await db.Teams.SingleOrDefaultAsync(x => x.Id == teamId);
            }
            return View();
        }

        public async Task<ActionResult> ShowTicket(int? id)
        {
            if (id != null)
            {
                Ticket ticket = await db.Tickets.Include(y => y.User).Include(s => s.ChildTickets)
                    .SingleOrDefaultAsync(x => x.Id == id);
                ticket.ChildTickets = await db.Tickets.Include(z => z.User).Include(y => y.ChildTickets).Include(w => w.Comments)
                    .Where(x => x.ParentTicket.Id == ticket.Id).ToListAsync();
                List<TicketType> ticketTypes = await db.TicketTypes.ToListAsync();
                ViewBag.TicketTypes = ticketTypes;
                return View(ticket);
            }
            return RedirectToAction("Tickets", "Ticket");
        }

        [HttpPost]
        public async Task<JsonResult> AddTicket(TicketBase newTicket)
        {
            if (newTicket.BaseTicketId != null && newTicket.Description != null && newTicket.BaseTicketId > 0 && newTicket.BaseTeamId > 0)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext()
                            .GetUserManager<ApplicationUserManager>()
                            .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                User curUser = await db.Users.SingleOrDefaultAsync(x => x.Email.ToLower().Equals(user.Email.ToLower()));
                TicketType ticketType = await db.TicketTypes.SingleOrDefaultAsync(x => x.Id == newTicket.TypeId);
                Ticket baseTicket = await db.Tickets.SingleOrDefaultAsync(x => x.Id == newTicket.BaseTicketId);
                if (curUser != null && ticketType != null)
                {
                    Ticket ticket = new Ticket(newTicket.BaseTeamId, curUser, newTicket.Description, ticketType, DateTime.Now, TicketState.New, baseTicket);
                    Ticket ticketFromDb = db.Tickets.Add(ticket);
                    await db.SaveChangesAsync();
                    TicketDTO ticketDto = new TicketDTO(ticketFromDb.Id, ticketFromDb.TeamId, ticketFromDb.User, 
                        ticketFromDb.Description, ticketFromDb.Type, ticketFromDb.TimeCreated.ToString(), ticketFromDb.State, 
                        ticketFromDb.ChildTickets.Count, ticketFromDb.Comments.Count);
                    return Json(ticketDto);
                }
            }
            else if (newTicket.BaseTicketId == null && newTicket.Description != null && newTicket.BaseTeamId > 0)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext()
                            .GetUserManager<ApplicationUserManager>()
                            .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                User curUser = await db.Users.SingleOrDefaultAsync(x => x.Email.ToLower().Equals(user.Email.ToLower()));
                TicketType ticketType = await db.TicketTypes.SingleOrDefaultAsync(x => x.Id == newTicket.TypeId);
                if (curUser != null && ticketType != null)
                {
                    Ticket ticket = new Ticket(newTicket.BaseTeamId, curUser, newTicket.Description, ticketType, DateTime.Now, TicketState.New);
                    Ticket ticketFromDb = db.Tickets.Add(ticket);
                    await db.SaveChangesAsync();
                    TicketDTO ticketDto = new TicketDTO(ticketFromDb.Id, ticketFromDb.TeamId, ticketFromDb.User,
                        ticketFromDb.Description, ticketFromDb.Type, ticketFromDb.TimeCreated.ToString(), ticketFromDb.State,
                        ticketFromDb.ChildTickets.Count, ticketFromDb.Comments.Count);
                    return Json(ticketDto);
                }
            }
            return Json(null);
        }

        [HttpPost]
        public async Task<JsonResult> AddComment(int? ticketId, string text)
        {
            if (text != null && ticketId != null && ticketId > 0)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext()
                            .GetUserManager<ApplicationUserManager>()
                            .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                User curUser = await db.Users.SingleOrDefaultAsync(x => x.Email.ToLower().Equals(user.Email.ToLower()));
                Ticket curTicket = await db.Tickets.Include(y => y.Comments).SingleOrDefaultAsync(x => x.Id == ticketId);
                Comment newComment = new Comment(text, curUser, DateTime.Now);
                curTicket.Comments.Add(newComment);
                await db.SaveChangesAsync();
                CommentDTO commentToJs = new CommentDTO(newComment.Id, newComment.Text, newComment.User, newComment.TimeCreated.ToString());
                return Json(commentToJs);
            }
            else
            {
                return Json(null);
            }
        }
    }
}