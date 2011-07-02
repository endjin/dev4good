using System.Collections.Generic;
using Orchard;
using Orchard.ContentManagement;
using oforms.Models;
using System.Web;

namespace oforms.Services
{
    public interface IOFormService : IDependency
    {
        void SubmitForm(OFormPart form, Dictionary<string, string> postData, HttpFileCollectionBase files, string ipSubmiter);

        OFormPart GetFormPartByName(string name, VersionOptions options);
    }
}