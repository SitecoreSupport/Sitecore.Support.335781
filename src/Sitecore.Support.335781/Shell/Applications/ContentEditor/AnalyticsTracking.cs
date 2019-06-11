using Sitecore;
using Sitecore.Analytics.Pipelines.GetItemPersonalizationVisibility;
using Sitecore.Diagnostics;
using Sitecore.Pipelines;
using Sitecore.Shell.Applications.ContentEditor;
using Sitecore.Text;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Sheer;
using System;
using System.Web.UI.WebControls;

namespace Sitecore.Support.Shell.Applications.ContentEditor
{
    [UsedImplicitly]
    public class AnalyticsTracking : Frame, IContentField
    {
        // Methods
        public AnalyticsTracking()
        {
            this.Class = "scContentControlSecurity";
            this.Height = new Unit(150.0, UnitType.Pixel);
            base.Scrolling = "yes";
            base.Activation = true;
        }

        public string GetValue() =>
            "__#!$No value$!#__";

        public override void HandleMessage(Message message)
        {
            Assert.ArgumentNotNull(message, "message");
            base.HandleMessage(message);
            if (message["id"] == this.ID)
            {
                string str = string.Empty;
                string name = message.Name;
                if (name == "contentanalytics:openprofiles")
                {
                    GetItemPersonalizationVisibilityArgs args1 = new GetItemPersonalizationVisibilityArgs(true, Client.GetItemNotNull(this.ItemID));
                    args1.Visible = true;
                    GetItemPersonalizationVisibilityArgs args = args1;
                    CorePipeline.Run("getItemPersonalizationVisibility", args, false);
                    if (args.Visible)
                    {
                        str = "scForm.invoke('item:personalize')";
                    }
                }
                else if (name == "contentanalytics:opengoals")
                {
                    str = "scForm.invoke('analytics:opengoals')";
                }
                else if (name == "contentanalytics:openattributes")
                {
                    str = "scForm.invoke('analytics:opentrackingfield')";
                }
                if (!string.IsNullOrEmpty(str))
                {
                    SheerResponse.Eval(str);
                }
            }
        }

        protected override bool LoadPostData(string value)
        {
            Assert.ArgumentNotNull(value, "value");
            if (value == this.Value)
            {
                return false;
            }
            this.SetModified();
            this.Value = value;
            return true;
        }

        protected override void OnLoad(EventArgs e)
        {
            Assert.ArgumentNotNull(e, "e");
            base.OnLoad(e);
            if (!Sitecore.Context.ClientPage.IsEvent)
            {
                UrlString urlString = new UrlString("/sitecore/shell/~/xaml/Sitecore.Shell.Applications.Analytics.TrackingFieldDetails.aspx");

                #region Check whether it is standard values and generate appropriate query string
                var item = Client.GetItemNotNull(this.ItemID);
                var standardValues = item.Template.StandardValues;

                if (Value != "standard_values" || standardValues == null)
                {
                    item.Uri.AddToUrlString(urlString);
                }
                else
                {
                    standardValues.Uri.AddToUrlString(urlString);
                }
                #endregion

                if (this.Disabled)
                {
                    urlString["di"] = "1";
                }
                urlString["fld"] = this.FieldID;
                UIUtil.AddContentDatabaseParameter(urlString);
                base.SourceUri = urlString.ToString();
            }
        }

        protected virtual void SetModified()
        {
            if (this.TrackModified)
            {
                Sitecore.Context.ClientPage.Modified = true;
            }
        }

        public void SetValue(string value)
        {
            Assert.ArgumentNotNull(value, "value");
            this.Value = value;
        }

        // Properties
        public string FieldID
        {
            get =>
                base.GetViewStateString("FieldID");
            set
            {
                Assert.ArgumentNotNull(value, "value");
                base.SetViewStateString("FieldID", value);
            }
        }

        public string ItemID
        {
            get =>
                base.GetViewStateString("ItemID");
            set
            {
                Assert.ArgumentNotNullOrEmpty(value, "value");
                base.SetViewStateString("ItemID", value);
            }
        }

        public bool TrackModified
        {
            get =>
                base.GetViewStateBool("TrackModified", true);
            set =>
                base.SetViewStateBool("TrackModified", value, true);
        }
    }
}