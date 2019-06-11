using System;
using Sitecore;
using Sitecore.Diagnostics;
using Sitecore.Shell.Applications.ContentEditor;
using Sitecore.StringExtensions;
using Sitecore.Text;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.Support.Shell.Applications.ContentEditor
{
    public class Security : Frame, IContentField
    {
        /// <summary>
        /// Gets or sets the field ID.
        /// </summary>
        /// <value>The field ID.</value>
        /// <contract>
        ///   <requires name="value" condition="not null" />
        ///   <ensures condition="not null" />
        /// </contract>
        public string FieldID
        {
            get
            {
                return base.GetViewStateString("FieldID");
            }
            set
            {
                Assert.ArgumentNotNull(value, "value");
                base.SetViewStateString("FieldID", value);
            }
        }

        /// <summary>
        /// Gets or sets the item ID.
        /// </summary>
        /// <value>The item ID.</value>
        /// <contract>
        ///   <requires name="value" condition="not empty" />
        ///   <ensures condition="not null" />
        /// </contract>
        public string ItemID
        {
            get
            {
                return base.GetViewStateString("ItemID");
            }
            set
            {
                Assert.ArgumentNotNullOrEmpty(value, "value");
                base.SetViewStateString("ItemID", value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="T:Sitecore.Shell.Applications.ContentEditor.IFrame" /> tracks the modified flag.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the <see cref="T:Sitecore.Shell.Applications.ContentEditor.IFrame" /> tracks the modified flag; otherwise, <c>false</c>.
        /// </value>
        public bool TrackModified
        {
            get
            {
                return base.GetViewStateBool("TrackModified", true);
            }
            set
            {
                base.SetViewStateBool("TrackModified", value, true);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sitecore.Shell.Applications.ContentEditor.Security" /> class.
        /// </summary>
        public Security()
        {
            this.Class = "scContentControlSecurity";
            base.Scrolling = "yes";
            base.Activation = true;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns>The value of the field.</returns>
        public string GetValue()
        {
            return "__#!$No value$!#__";
        }

        /// <summary>
        /// Handles the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public override void HandleMessage(Message message)
        {
            Assert.ArgumentNotNull(message, "message");
            base.HandleMessage(message);
            if (message.Name != "contentsecurity:assign")
            {
                return;
            }
            SheerResponse.Eval("scForm.invoke('security:openitemsecurityeditor(accountname=,accounttype=,fieldid={0})')".FormatWith(new object[] { this.FieldID }));
        }

        /// <summary>
        /// Loads the post data.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <contract>
        ///   <requires name="value" condition="not null" />
        /// </contract>
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

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"></see> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        /// <contract>
        ///   <requires name="e" condition="not null" />
        /// </contract>
        protected override void OnLoad(EventArgs e)
        {
            Assert.ArgumentNotNull(e, "e");
            base.OnLoad(e);
            if (Sitecore.Context.ClientPage.IsEvent)
            {
                return;
            }
            UrlString urlString = new UrlString("/sitecore/shell/-/xaml/Sitecore.Shell.Applications.Security.SecurityDetails.aspx");

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

        /// <summary>
        /// Sets the Modified flag.
        /// </summary>
        protected virtual void SetModified()
        {
            if (this.TrackModified)
            {
                Sitecore.Context.ClientPage.Modified = true;
            }
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="value">The value of the field.</param>
        public void SetValue(string value)
        {
            Assert.ArgumentNotNull(value, "value");
            this.Value = value;
        }
    }
}