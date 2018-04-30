using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace HelpDeskTeamProject.Classes
{
    public class HtmlValidator : IHtmlValidator
    {
        public string ValidateHtml(string html)
        {
            var sanitizer = new Ganss.XSS.HtmlSanitizer();

            sanitizer.AllowedAttributes.Remove("style");

            var result = sanitizer.Sanitize(html);

            return result;
        }
    }
}