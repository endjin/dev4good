using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Security;
using Orchard.Settings;
using Orchard.UI.Navigation;
using Orchard.UI.Notify;
using Orchard;
using oforms.Models;
using oforms.Services;
using Orchard.Data;
using System.Text;
using oforms.ViewModels;

namespace oforms.Controllers
{
	public static class OFormTemplateHelper
	{
		public static void PreFillForm(string templateName, OFormPart formPart, IOrchardServices orchardServices)
		{
			switch(templateName){
				case "ContactUs":
					SetFormName(formPart, "contact-us", orchardServices);
					formPart.Method = "POST";
					formPart.InnerHtml = @"<h2>Contact Us</h2>
<label for=""Name"">Name</label><br/>
<input name=""Name"" type=""text"" value="""" /><br/>

<label for=""Email"">Email</label><br/>
<input name=""Email"" type=""text"" value=""""/><br/>

<label for=""Phone"">Phone</label><br/>
<input name=""Phone"" type=""text"" value=""""/><br/>

<label for=""Website"">Website</label><br/>
<input name=""Website"" type=""text"" value=""""/><br/>

<label for=""Topic"">Topic of Inquiry</label><br/>
<input name=""Topic"" type=""text"" value=""""/><br/>

<label for=""Message"">Message</label><br/>
<textarea name=""Message"" cols=""50"" rows=""10""></textarea><br/>

<input type=""submit"" value=""Send"" /><br/>";
					formPart.SaveResultsToDB = true;
					formPart.CanUploadFiles = false;
					formPart.SendEmail = false;
					formPart.ValRequiredFields = "Name, Email, Message, Topic";
					formPart.ValEmail = "Email";
					formPart.ValUrl = "Website";
					break;
			}
		}

		static void SetFormName(OFormPart formPart, string commonName, IOrchardServices orchardServices)
		{
			string formName = commonName;
			var i = -1;
			while (i++ < 10){
				if (i > 0){
					formName = string.Format("{0}-{1}", commonName, i);
				}
				
				var form = orchardServices.ContentManager.Query<OFormPart, OFormPartRecord>(VersionOptions.Latest)
					.Where(x => x.Name == formName)
					.List().FirstOrDefault();
				
				if (form == null) {
					break;
				}
			}
			
			formPart.Name = formName;
		}
	}
}
