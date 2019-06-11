using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Security.Accounts;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Pages;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.XmlControls;
using System;
using System.Collections;

namespace Sitecore.Support.Shell.Applications.ContentManager.Dialogs.ResetFields
{
    public class ResetFieldsForm : DialogForm
    {
        // Fields
        protected Border Fields;
        protected XmlControl Dialog;

        // Methods
        protected override void OnLoad(EventArgs e)
        {
            Assert.ArgumentNotNull(e, "e");
            base.OnLoad(e);
            if (!Context.ClientPage.IsEvent)
            {
                base.OK.ToolTip = Translate.Text("Reset the field values on this page.");
                Item itemFromQueryString = UIUtil.GetItemFromQueryString(Context.ContentDatabase);
                RenderEditor(itemFromQueryString, itemFromQueryString, this.Fields);
            }
        }

        protected override void OnOK(object sender, EventArgs args)
        {
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(args, "args");
            Item itemFromQueryString = UIUtil.GetItemFromQueryString(Context.ContentDatabase);
            Assert.IsNotNull(itemFromQueryString, "Item not found");
            User user = Context.User;
            itemFromQueryString.Editing.BeginEdit();
            foreach (string str in Context.ClientPage.ClientRequest.Form.Keys)
            {
                if (string.IsNullOrEmpty(str))
                {
                    continue;
                }
                if (str.StartsWith("Reset_", StringComparison.InvariantCulture))
                {
                    ID id = ShortID.DecodeID(StringUtil.Mid(str, 6));
                    Field field = itemFromQueryString.Fields[id];
                    if (field.CanUserWrite(user))
                    {
                        field.Reset();
                    }
                    string[] parameters = new string[] { AuditFormatter.FormatItem(itemFromQueryString), AuditFormatter.FormatField(field) };
                    Log.Audit(this, "Reset field: {0}, field: {1}", parameters);
                }
            }
            itemFromQueryString.Editing.EndEdit();
            SheerResponse.SetDialogValue("yes");
            base.OnOK(sender, args);
        }

        private static void RenderEditor(Item item, Item root, Control parent)
        {
            Assert.ArgumentNotNull(item, "item");
            Assert.ArgumentNotNull(root, "root");
            Assert.ArgumentNotNull(parent, "parent");
            Hashtable fieldInfo = new Hashtable();
            ResetFieldsPreview preview1 = new ResetFieldsPreview();
            preview1.RenderHeader = false;
            preview1.RenderTabsAndBars = false;
            preview1.Render(item, root, fieldInfo, parent, false);
        }
    }
}