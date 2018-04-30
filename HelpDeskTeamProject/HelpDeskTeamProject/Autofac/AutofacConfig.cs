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
using HelpDeskTeamProject.Classes;

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
            builder.RegisterType<HtmlValidator>().As<IHtmlValidator>().InstancePerRequest();
            builder.RegisterType<TicketTypeService>().As<ITicketTypeManager>().InstancePerRequest();
            builder.RegisterType<TicketLoggerService>().As<ITicketLogger>().InstancePerRequest();
            builder.RegisterType<CommentService>().As<ICommentManager>().InstancePerRequest();
            builder.RegisterType<TicketManagerService>().As<ITicketManager>().InstancePerRequest();
            builder.RegisterType<UserManagerService>().As<IUserManager>().InstancePerRequest();
            builder.RegisterType<DTOConverterService>().As<IDtoConverter>().InstancePerRequest();
            var container = builder.Build();
            System.Web.Mvc.DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}