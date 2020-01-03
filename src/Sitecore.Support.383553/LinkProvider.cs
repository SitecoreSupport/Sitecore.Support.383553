using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Diagnostics;
using Sitecore.Links;

namespace Sitecore.Support.Links
{
    public class LinkProvider: Sitecore.Links.LinkProvider
    {
        protected override LinkBuilder CreateLinkBuilder(UrlOptions options)
        {
            Debug.ArgumentNotNull(options, "options");
            return new Sitecore.Support.Links.LinkBuilder(options);
        }
    }
}