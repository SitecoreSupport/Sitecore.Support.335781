using System.Web.UI.WebControls;
using Sitecore;
using Sitecore.Collections;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Reflection;
using Sitecore.Resources;
using Sitecore.Shell;
using Sitecore.Shell.Applications.ContentEditor;
using Sitecore.Shell.Applications.ContentManager;
using Sitecore.Web.UI.HtmlControls;


namespace Sitecore.Support.Shell.Applications.ContentEditor
{
    public class EditorFormatter : Sitecore.Shell.Applications.ContentEditor.EditorFormatter
    {
        public void SupportRenderField(System.Web.UI.Control parent, Editor.Field field, Item fieldType, bool readOnly, string value)
        {
            Assert.ArgumentNotNull(parent, "parent");
            Assert.ArgumentNotNull(field, "field");
            Assert.ArgumentNotNull(fieldType, "fieldType");
            Assert.ArgumentNotNull(value, "value");
            bool hasRibbon = false;
            string text = string.Empty;
            System.Web.UI.Control editor = this.GetEditor(fieldType);
            if (this.Arguments.ShowInputBoxes)
            {
                ChildList children = fieldType.Children;
                hasRibbon = !UserOptions.ContentEditor.ShowRawValues && (children["Ribbon"] != null);
                string typeKey = field.TemplateField.TypeKey;
                if ((typeKey == "rich text") || (typeKey == "html"))
                {
                    hasRibbon = hasRibbon && (UserOptions.HtmlEditor.ContentEditorMode != UserOptions.HtmlEditor.Mode.Preview);
                }
            }
            string str2 = string.Empty;
            string str3 = string.Empty;
            int @int = Registry.GetInt("/Current_User/Content Editor/Field Size/" + field.TemplateField.ID.ToShortID(), -1);
            if (@int != -1)
            {
                str2 = $" height:{@int}px";
                Sitecore.Web.UI.HtmlControls.Control control2 = editor as Sitecore.Web.UI.HtmlControls.Control;
                if (control2 != null)
                {
                    control2.Height = new Unit((double)@int, UnitType.Pixel);
                }
                else
                {
                    WebControl control3 = editor as WebControl;
                    if (control3 != null)
                    {
                        control3.Height = new Unit((double)@int, UnitType.Pixel);
                    }
                }
            }
            else if (editor is Frame)
            {
                string style = field.ItemField.Style;
                if (string.IsNullOrEmpty(style) || !style.ToLowerInvariant().Contains("height"))
                {
                    str3 = " class='defaultFieldEditorsFrameContainer'";
                }
            }
            else if (editor is MultilistEx)
            {
                string style = field.ItemField.Style;
                if (string.IsNullOrEmpty(style) || !style.ToLowerInvariant().Contains("height"))
                {
                    str3 = " class='defaultFieldEditorsMultilistContainer'";
                }
            }
            else
            {
                string typeKey = field.ItemField.TypeKey;
                if ((!string.IsNullOrEmpty(typeKey) && typeKey.Equals("checkbox")) && !UserOptions.ContentEditor.ShowRawValues)
                {
                    str3 = "class='scCheckBox'";
                }
            }
            string[] textArray1 = new string[] { "<div style='", str2, "' ", str3, ">" };
            this.AddLiteralControl(parent, string.Concat(textArray1));
            this.AddLiteralControl(parent, text);
            this.SupportAddEditorControl(parent, editor, field, hasRibbon, readOnly, value);
            this.AddLiteralControl(parent, "</div>");
            this.RenderResizable(parent, field);

        }

        private void RenderResizable(System.Web.UI.Control parent, Editor.Field field)
        {
            string str = field.TemplateField.Type;
            if (!string.IsNullOrEmpty(str))
            {
                FieldType fieldType = FieldTypeManager.GetFieldType(str);
                if ((fieldType != null) && fieldType.Resizable)
                {
                    object[] objArray1 = new object[] { "<div style=\"cursor:row-resize; position: relative; height: 5px; width: 100%; top: 0px; left: 0px;\" onmousedown=\"scContent.fieldResizeDown(this, event)\" onmousemove=\"scContent.fieldResizeMove(this, event)\" onmouseup=\"scContent.fieldResizeUp(this, event, '", field.TemplateField.ID.ToShortID(), "')\">", Images.GetSpacer(1, 4), "</div>" };
                    string text = string.Concat(objArray1);
                    this.AddLiteralControl(parent, text);
                    text = "<div class style=\"display:none\" \">" + Images.GetSpacer(1, 4) + "</div>";
                    this.AddLiteralControl(parent, text);
                }
            }
        }

        public void SupportAddEditorControl(System.Web.UI.Control parent, System.Web.UI.Control editor, Editor.Field field, bool hasRibbon, bool readOnly, string value)
        {
            Assert.ArgumentNotNull(parent, "parent");
            Assert.ArgumentNotNull(editor, "editor");
            Assert.ArgumentNotNull(field, "field");
            Assert.ArgumentNotNull(value, "value");
            SetProperties(editor, field, readOnly);
            this.SupportSetValue(editor, value);
            EditorFieldContainer container1 = new EditorFieldContainer(editor);
            container1.ID = field.ControlID + "_container";
            System.Web.UI.Control control = container1;
            Context.ClientPage.AddControl(parent, control);
            SetProperties(editor, field, readOnly);
            SetAttributes(editor, field, hasRibbon);
            SetStyle(editor, field);
            this.SupportSetValue(editor, value);
        }

        public void SupportSetValue(System.Web.UI.Control editor, string value)
        {
            Assert.ArgumentNotNull(editor, "editor");
            Assert.ArgumentNotNull(value, "value");
            if (!(editor is IStreamedContentField))
            {
                IContentField field = editor as IContentField;
                if (field != null)
                {
                    // set the value to show when standard values are used
                    if (field as Sitecore.Support.Shell.Applications.ContentEditor.Security != null || field as Sitecore.Support.Shell.Applications.ContentEditor.AnalyticsTracking != null)
                    {
                        value = "standard_values";
                    }
                    field.SetValue(value);
                }
                else
                {
                    ReflectionUtil.SetProperty(editor, "Value", value);
                }
            }
        }


    }
}