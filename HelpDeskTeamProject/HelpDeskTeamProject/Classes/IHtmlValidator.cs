using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskTeamProject.Classes
{
    public interface IHtmlValidator
    {
        List<string> ValidateHtml(string html);
    }
}
