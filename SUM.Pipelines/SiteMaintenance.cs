using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore;
using Sitecore.Pipelines.HttpRequest;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using System.Web;
using Sitecore.Diagnostics;
using Sitecore.Data;
using Sitecore.Links;
using Sitecore.Globalization;
namespace SUM.Pipelines
{
    public class SiteMaintenance : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {

            Item StartItem, itmSiteSettings;
            LinkField lnkfldMaintenaceLink;
            List<Item> lstSiteSettings;
            try
            {
                string strstartItem = Sitecore.Context.Site.StartPath;
                if (string.IsNullOrEmpty(strstartItem) || Sitecore.Context.Site.Name.ToLower().Equals("shell")
                    || Sitecore.Context.Site.Name.ToLower().Equals("login")
                    || Sitecore.Context.Site.Name.ToLower().Equals("admin")
                    || Sitecore.Context.Site.Name.ToLower().Equals("service")
                    || Sitecore.Context.Site.Name.ToLower().Equals("modules_shell")
                    || Sitecore.Context.Site.Name.ToLower().Equals("modules_website")
                    )
                {
                    return;
                }
                StartItem = Sitecore.Context.Database.GetItem(strstartItem);

                if (StartItem != null)
                {
                    Log.Info("Executing SUM Pipeline", HttpContext.Current.Request);
                    lstSiteSettings = (from ss in StartItem.Parent.GetChildren()
                                       where ss.TemplateID.ToString().Equals(TemplateIDs.SiteSettings.ToString())
                                       select ss).ToList<Item>();
                    if (lstSiteSettings.Count > 0)
                    {
                        itmSiteSettings = lstSiteSettings.First();

                        if (itmSiteSettings != null && itmSiteSettings["SwitchSUM"] != null)
                        {
                            if (itmSiteSettings.Fields["SwitchSUM"].Value == "1")
                            {
                                if (itmSiteSettings["MaintenanceLink"] != null && !string.IsNullOrEmpty(itmSiteSettings.Fields["MaintenanceLink"].Value))
                                {
                                    lnkfldMaintenaceLink = itmSiteSettings.Fields["MaintenanceLink"];
                                    if (Context.Item.ID.ToString() != (Context.Database.GetItem(lnkfldMaintenaceLink.TargetID)).ID.ToString())
                                    {
                                        HttpContext.Current.Response.Redirect(LinkManager.GetItemUrl(Context.Database.GetItem(lnkfldMaintenaceLink.TargetID)), true);
                                    }

                                }
                            }
                        }
                    }

                }
                Log.Info("Executed SUM Pipeline", HttpContext.Current.Request);

            }
            catch (Exception e)
            {

                Log.Error("Error - SUM pipeline :", e);
            }
        }
    }
}