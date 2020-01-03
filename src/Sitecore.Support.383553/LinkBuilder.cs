using Sitecore.Links;
using Sitecore.Sites;
using Sitecore.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Support.Links
{
    public class LinkBuilder: Sitecore.Links.LinkProvider.LinkBuilder
    {
        public LinkBuilder(UrlOptions options):base(options)
        {

        }
        internal virtual string GetHostName()
        {
            return WebUtil.GetHostName();
        }

        internal virtual string GetScheme()
        {
            return WebUtil.GetScheme();
        }

        protected override string GetServerUrlElement(SiteInfo siteInfo)
        {
            SiteContext currentSite = Context.Site;

            string currentSiteName = currentSite != null ? currentSite.Name : string.Empty;
            string currentHostName = this.GetHostName();
            string defaultUrl = this.AlwaysIncludeServerUrl ? WebUtil.GetServerUrl() : string.Empty;

            if (siteInfo == null)
            {
                return defaultUrl;
            }

            string hostName = (!string.IsNullOrEmpty(siteInfo.HostName) && !string.IsNullOrEmpty(currentHostName) && siteInfo.Matches(currentHostName))
              ? currentHostName
              : StringUtil.GetString(this.GetTargetHostName(siteInfo), currentHostName);

            string scheme = StringUtil.GetString(siteInfo.Scheme, this.GetScheme());

            int portNumber = MainUtil.GetInt(siteInfo.Port, WebUtil.GetPort());
            int currentPort = WebUtil.GetPort();
            int externalPortNumber = MainUtil.GetInt(siteInfo.ExternalPort, portNumber);

            if (externalPortNumber != portNumber)
            {
                if (this.AlwaysIncludeServerUrl)
                {
                    defaultUrl = externalPortNumber == 80 ? string.Format("{0}://{1}", scheme, this.GetHostName()) : string.Format("{0}://{1}:{2}", scheme, this.GetHostName(), externalPortNumber);
                }

                portNumber = externalPortNumber;
            }

            if (!this.AlwaysIncludeServerUrl && siteInfo.Name.Equals(currentSiteName, StringComparison.OrdinalIgnoreCase) &&
              currentHostName.Equals(hostName, StringComparison.OrdinalIgnoreCase))
            {
                return defaultUrl;
            }

            if (string.IsNullOrEmpty(hostName) || hostName.IndexOf('*') >= 0)
            {
                return defaultUrl;
            }

            string currentScheme = this.GetScheme();
            StringComparison ignoreCase = StringComparison.OrdinalIgnoreCase;

            if (hostName.Equals(currentHostName, ignoreCase) && portNumber == currentPort && scheme.Equals(currentScheme, ignoreCase))
            {
                return defaultUrl;
            }

            string serverUrl = scheme + "://" + hostName;

            if (portNumber > 0 && portNumber != 80)
            {
                serverUrl += ":" + portNumber;
            }

            return serverUrl;
        }
    }
}