﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.Classes
{
    public class HtmlValidator
    {
        public List<string> Validate(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            int scriptTagCount = 0;
            var scripts = doc.DocumentNode.Descendants("script");
            foreach (var script in scripts)
            {
                scriptTagCount++;
            }

            var errors = new List<string>();
            if (scriptTagCount > 0)
                errors.Add("You cannot enter script tag in your text.");

            int styleTagCount = 0;
            var styles = doc.DocumentNode.Descendants("style");
            foreach (var script in scripts)
            {
                styleTagCount++;
            }

            if (styleTagCount > 0)
                errors.Add("You cannot enter style tag in your text.");

            var otherErrors = doc.ParseErrors;

            foreach (var error in otherErrors)
            {
                errors.Add(error.Reason);
            }

            return errors;
        }
    }
}