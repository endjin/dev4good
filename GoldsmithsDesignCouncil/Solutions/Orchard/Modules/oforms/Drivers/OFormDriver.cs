using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement;
using oforms.Models;

namespace oforms.Drivers
{
    public class OFormDriver : ContentPartDriver<OFormPart>
    {
        protected override DriverResult Display(OFormPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_OForm_Display",
                             () => shapeHelper.Parts_OForm_Display(ContentPart: part, ContentItem: part.ContentItem));
        }

        //GET
        protected override DriverResult Editor(OFormPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_OForm_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/OForms.Form",
                    Model: part,
                    Prefix: Prefix));
        }

        //POST
        protected override DriverResult Editor(
            OFormPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}