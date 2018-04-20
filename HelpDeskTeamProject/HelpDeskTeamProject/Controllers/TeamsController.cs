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
        public ActionResult InviteUser(int _teamId, string userEmail)
        {
            var teamToInvite = db.Teams.Find(_teamId);
            var newInvitedEmail = new InvitationEmail() { Email = userEmail };
            teamToInvite.InvitedEmails.Add(newInvitedEmail);
            db.SaveChanges();

            return RedirectToAction("TeamInfo", new { teamId = _teamId });
        }


        //create new team
        public ActionResult Create()
        {
            return View();
        }


        //create new team
        [HttpPost]
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
