using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Applications.ContentEditor;
using Sitecore.Shell.Applications.ContentManager;

namespace Sitecore.Support.Shell.Applications.ContentManager.Dialogs.ResetFields
{
    public class ResetFieldsPreview : Editor
    {
        // Methods
        public ResetFieldsPreview()
        {
            base.RenderWarnings = false;
        }

        protected override EditorFormatter GetEditorFormatter() =>
            new ResetFieldsFormatter();

        protected override bool GetReadOnly(Item item)
        {
            Assert.ArgumentNotNull(item, "item");
            return true;
        }
    }
}