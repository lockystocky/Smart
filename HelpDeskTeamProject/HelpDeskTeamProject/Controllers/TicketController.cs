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

        public async Task<JsonResult> GetTicketsByTeam(int? teamId)
        {
            if (teamId != null)
            {
                Team curTeam = await db.Teams.Include(x => x.Tickets).SingleOrDefaultAsync(y => y.Id == teamId);
                List<Ticket> curTickets = await db.Tickets.Include(x => x.ChildTickets).Include(y => y.Comments).Include(z => z.User).Where(s => s.ChildTickets)).ToListAsync();
                List<TicketDTO> curTicketsDto = new List<TicketDTO>();
                foreach (Ticket value in curTickets)
                {
                    curTicketsDto.Add(new TicketDTO(value));
                }
                return Json(curTicketsDto, JsonRequestBehavior.AllowGet);
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
            id = 1;
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
            return View();
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
                    TicketDTO ticketDto = new TicketDTO(ticketFromDb.Id, ticketFromDb.TeamId, ticketFromDb.User, 
                        ticketFromDb.Description, ticketFromDb.Type, ticketFromDb.TimeCreated.ToString(), ticketFromDb.State, 
                        ticketFromDb.ChildTickets.Count, ticketFromDb.Comments.Count);
                    await db.SaveChangesAsync();
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
                    TicketDTO ticketDto = new TicketDTO(ticketFromDb.Id, ticketFromDb.TeamId, ticketFromDb.User,
                        ticketFromDb.Description, ticketFromDb.Type, ticketFromDb.TimeCreated.ToString(), ticketFromDb.State,
                        ticketFromDb.ChildTickets.Count, ticketFromDb.Comments.Count);
                    await db.SaveChangesAsync();
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
                CommentDTO commentToJs = new CommentDTO(newComment.Text, newComment.User, newComment.TimeCreated.ToString());
                await db.SaveChangesAsync();
                return Json(commentToJs);
            }
            else
            {
                return Json(null);
            }
        }
    }
}