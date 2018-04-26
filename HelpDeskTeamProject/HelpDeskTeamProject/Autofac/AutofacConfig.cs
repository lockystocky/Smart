using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HelpDeskTeamProject.DataModels;
using HelpDeskTeamProject;
using HelpDeskTeamProject.Context;
using HelpDeskTeamProject.Services;

namespace HelpDeskTeamProject.Autofac
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterType<AppContext>().As<IAppContext>().InstancePerRequest();
            builder.RegisterType<TeamService>().As<ITeamService>().InstancePerRequest();
            var container = builder.Build();
            System.Web.Mvc.DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}