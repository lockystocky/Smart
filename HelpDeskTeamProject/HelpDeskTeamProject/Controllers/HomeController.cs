using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HelpDeskTeamProject.DataModels;
using HelpDeskTeamProject.Context;
using System.Data.Entity;

namespace HelpDeskTeamProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            AppContext context = new AppContext();
            User test = context.Users.SingleOrDefault(x => x.Id == 1);
            test.Teams.Add(new Team("team1", Guid.NewGuid(), 1));
            test.Teams.Add(new Team("team2", Guid.NewGuid(), 1));
            test.Teams.Add(new Team("team3", Guid.NewGuid(), 1));
            context.SaveChanges();
            User[] users = context.Users.Include(y => y.AppRole).Where(x => x.Id > 0).ToList().ToArray();
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}