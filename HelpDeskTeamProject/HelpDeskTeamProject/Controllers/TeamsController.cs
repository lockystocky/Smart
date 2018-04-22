using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HelpDeskTeamProject.Context;
using HelpDeskTeamProject.DataModels;
using HelpDeskTeamProject.Models;
using Microsoft.AspNet.Identity;

namespace HelpDeskTeamProject.Controllers
{
    public class TeamsController : Controller
    {
        private AppContext db = new AppContext();

        //view list of all existing teams
        public ActionResult AllTeams()
        {
            return View(db.Teams.ToList());
        }

        //invite user to team
        [Authorize]
        public ActionResult InviteUser(int? teamId)
        {
            var teamToInvite = db.Teams.Find(teamId);

            InviteUserToTeamViewModel viewModel = new InviteUserToTeamViewModel()
            {
                TeamToInvite = teamToInvite
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public ActionResult InviteUser(int _teamId, string userEmail)
        {
            var teamToInvite = db.Teams.Find(_teamId);
            var newInvitedEmail = new InvitationEmail() { Email = userEmail };
            teamToInvite.InvitedEmails.Add(newInvitedEmail);
            db.SaveChanges();

            return RedirectToAction("TeamInfo", new { teamId = _teamId });
        }

        [Authorize]
        public ActionResult Teams()
        {
            return View();
        }

        //used in view to create teams menu for curent user 
        [Authorize]
        public ActionResult GetCurrentUserTeamsList()
        {
            var context = new ApplicationDbContext();
            var currentUserId = User.Identity.GetUserId();

            db.Configuration.ProxyCreationEnabled = false;

            var currentUser = db.Users
                .Where(user => user.AppId == currentUserId)
                .FirstOrDefault();

            var currentUserTeamsList = db.Teams
                .Include(t => t.Tickets)
                .Where(t => t.Users.Select(u => u.Id).Contains(currentUser.Id))
                .ToList();            

            List<TeamWithLastChangesViewModel> data = new List<TeamWithLastChangesViewModel>();
            foreach (var team in currentUserTeamsList)
            {
                Ticket lastTicketInTeam = team.Tickets
                    .OrderByDescending(ticket => ticket.TimeCreated)
                    .FirstOrDefault();

                if (lastTicketInTeam == null)
                    lastTicketInTeam = new Ticket()
                    {
                        Description = "This is default last ticket text to test",
                        TimeCreated = DateTime.Now
                    };


                TeamWithLastChangesViewModel teamViewModel = new TeamWithLastChangesViewModel()
                {
                    Team = team,
                    LastTicket = lastTicketInTeam
                };

                data.Add(teamViewModel);
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        

        //create new team
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        
        //create new team
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] Team team)
        {
            if (ModelState.IsValid)
            {
                var context = new ApplicationDbContext();
                var currentUserId = User.Identity.GetUserId();

                var helpDeskUser = db.Users
                    .Where(user => user.AppId == currentUserId)
                    .FirstOrDefault();

                Team createdTeam = CreateTeam(team.Name, helpDeskUser.Id);
                createdTeam.Users.Add(helpDeskUser);
                db.Teams.Add(createdTeam);
                db.SaveChanges();

                return RedirectToAction("AllTeams");
            }

            return View(team);
        }

        private Team CreateTeam(string teamName, int ownerId)
        {
            Guid teamGuid = Guid.NewGuid();

            Team team = new Team()
            {
                Name = teamName,
                OwnerId = ownerId,
                TeamGuid = teamGuid,
                InvitationLink = "/team/" + teamGuid,
                InvitedEmails = new List<InvitationEmail>(),
                UserPermissions = new List<UserPermission>(),
                Tickets = new List<Ticket>(),
                Users = new List<User>()
            };

            return team;
        }
        
        public ActionResult TeamInfo(int? teamId)
        {
            var team = db.Teams
                .Where(t => t.Id == teamId).FirstOrDefault();

            if (team == null)
                return HttpNotFound();

            return View(team);
        }

        //Team Administrator must be able to manage Team  members and their roles

        public ActionResult ManageTeam(int? teamId)
        {
            var context = new ApplicationDbContext();
            var currentUserId = User.Identity.GetUserId();

            var currentUser = db.Users
                .Where(user => user.AppId == currentUserId)
                .FirstOrDefault();
            

            var team = db.Teams
                .Where(t => t.Id == teamId)
                .FirstOrDefault();

            if (team == null)
                return HttpNotFound();

            //team.

            if (currentUser.Id != team.OwnerId)
                return HttpNotFound();

            ManageTeamViewModel viewModel = new ManageTeamViewModel()
            {
                TeamId = team.Id,
                TeamUsers = team.Users,
                UserPermissions = team.UserPermissions
            };

            return View(viewModel);
        }

        /*

        // GET: Teams/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = await db.Teams.FindAsync(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,TeamGuid,Name,OwnerId,InvitationLink")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Entry(team).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(team);
        }

        // GET: Teams/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = await db.Teams.FindAsync(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Team team = await db.Teams.FindAsync(id);
            db.Teams.Remove(team);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }*/

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
