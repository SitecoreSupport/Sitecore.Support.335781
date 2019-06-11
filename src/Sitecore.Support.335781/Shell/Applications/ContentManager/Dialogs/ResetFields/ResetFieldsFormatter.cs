using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Applications.ContentEditor;
using Sitecore.Shell.Applications.ContentManager;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.WebControls;
using System.Web.UI.WebControls;

namespace Sitecore.Support.Shell.Applications.ContentManager.Dialogs.ResetFields
{
    public class ResetFieldsFormatter : Sitecore.Support.Shell.Applications.ContentEditor.EditorFormatter
    {
        // Methods
        public override void RenderField(System.Web.UI.Control parent, Editor.Field field, Item fieldType, bool readOnly)
        {
            Assert.ArgumentNotNull(parent, "parent");
            Assert.ArgumentNotNull(field, "field");
            Assert.ArgumentNotNull(fieldType, "fieldType");
            GridPanel control = new GridPanel
            {
                Width = new Unit(100.0, UnitType.Percentage),
                Columns = 5
            };
            Context.ClientPage.AddControl(parent, control);
            Border border = new Border();
            Context.ClientPage.AddControl(control, border);
            control.SetExtensibleProperty(border, "width", "24");
            control.SetExtensibleProperty(border, "valign", "top");
            Sitecore.Web.UI.HtmlControls.Checkbox checkbox = new Sitecore.Web.UI.HtmlControls.Checkbox();
            Context.ClientPage.AddControl(border, checkbox);
            checkbox.ID = "Reset_" + field.ItemField.ID.ToShortID();
            Context.ClientPage.AddControl(control, new Space("8", "1"));
            border = new Border();
            Context.ClientPage.AddControl(control, border);
            control.SetExtensibleProperty(border, "width", "50%");
            control.SetExtensibleProperty(border, "valign", "top");
            base.RenderField(border, field, fieldType, true);
            Context.ClientPage.AddControl(control, new Space("8", "1"));
            border = new Border();
            Context.ClientPage.AddControl(control, border);
            control.SetExtensibleProperty(border, "width", "50%");
            control.SetExtensibleProperty(border, "valign", "top");
            string str = field.ItemField.GetValue(!StandardValuesManager.IsStandardValuesHolder(field.ItemField.Item), true, true, true, false);
            string[] values = new string[] { str };


            base.SupportRenderField(border, field.Clone(Sitecore.Web.UI.HtmlControls.Control.GetUniqueID("RFIELD")), fieldType, true, StringUtil.GetString(values));
        }
    }
}