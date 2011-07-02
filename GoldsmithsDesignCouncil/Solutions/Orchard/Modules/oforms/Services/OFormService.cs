using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Orchard.Data;
using Orchard.Messaging.Services;
using oforms.Models;
using Orchard.ContentManagement;
using Orchard;
using System.Xml.Linq;
using System.Xml;
using System.Web;
using System.IO;
using Orchard.Localization;

namespace oforms.Services
{
    public class OFormService : IOFormService
    {
        private readonly IContentManager _contentManager;

        private readonly IOrchardServices _orchardServices;

        private readonly IMessageManager _messageManager;

        private readonly IRepository<OFormResultRecord> _resultRepo;

        private readonly IRepository<OFormFileRecord> _fileRepo;

        public OFormService(IContentManager contentManager, IOrchardServices orchardServices,
                            IMessageManager messageManager, IRepository<OFormResultRecord> resultRepo,
                            IRepository<OFormFileRecord> fileRepo)
        {
            this._resultRepo = resultRepo;
            this._fileRepo = fileRepo;
            this._messageManager = messageManager;
            this._orchardServices = orchardServices;
            this._contentManager = contentManager;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void SubmitForm(OFormPart form, Dictionary<string, string> postData, HttpFileCollectionBase files, string ipSubmiter)
        {
            if (form == null)
                throw new ArgumentNullException("form");
            if (postData == null)
                throw new ArgumentNullException("postData");

            var formResult = SaveFormToDB(form, postData, files, ipSubmiter);
            postData.Add("oforms.formResult.Id", formResult.Id.ToString());

            if (form.SendEmail) {
                _messageManager.Send(form.Record.ContentItemRecord, 
                    MessageTypes.SendFormResult, 
                    "email", 
                    postData);
            }

            if (!form.SaveResultsToDB)
            {
                _resultRepo.Delete(formResult);
            }
        }

        public OFormPart GetFormPartByName(string name, VersionOptions options)
        {
            return _contentManager.Query<OFormPart, OFormPartRecord>(options)
                .Where(f => f.Name == name)
                .List()
                .FirstOrDefault();
        }

        private OFormResultRecord SaveFormToDB(OFormPart form, Dictionary<string, string> postData, HttpFileCollectionBase files, string ipSubmiter)
        {
            var xdoc = ConvertToXDocument(postData);
            var resultRecord = new OFormResultRecord
            {
                Xml = xdoc.ToString(),
                CreatedDate = DateTime.UtcNow,
                Ip = ipSubmiter
            };

            if (form.CanUploadFiles && files.Count > 0)
            {
                foreach (string key in files.Keys)
                {
                    if (files[key].ContentLength == 0) { continue; }

                    CheckFileSize(form, files[key]);
                    CheckFileType(form, files[key]);

                    var formFile = SaveFile(key, files[key]);
                    resultRecord.AddFile(formFile);
                }
            }

            this._resultRepo.Create(resultRecord);
            form.Record.AddFormResult(resultRecord);
            _contentManager.Flush();

            return resultRecord;
        }

        private XDocument ConvertToXDocument(IDictionary<string, string> nvCollection)
        {
            var element = new XElement("form");
            foreach (string key in nvCollection.Keys)
            {
                // don't save anti-forgery token
                if (key == "__RequestVerificationToken") continue;
                // ignore keys that are specific for oforms module
                if (key.StartsWith("oforms.")) continue;
                
                element.Add(new XElement(XmlConvert.EncodeName(key), nvCollection[key]));
            }

            return new XDocument(element);
        }

        private OFormFileRecord SaveFile(string fieldName, HttpPostedFileBase file)
        {
            var formFile = new OFormFileRecord();
            formFile.FieldName = fieldName;
            formFile.OriginalName = file.FileName;
            formFile.ContentType = file.ContentType;
            formFile.Size = file.ContentLength;
            using (var ms = new MemoryStream())
            {
                file.InputStream.CopyTo(ms, 4096);
                formFile.Bytes = ms.ToArray();
                formFile.Size = formFile.Bytes.Length;
            }

            this._fileRepo.Create(formFile);
            return formFile;
        }

        private void CheckFileType(OFormPart form, HttpPostedFileBase postedFile)
        {
            if (string.IsNullOrEmpty(form.UploadFileType)) return;
            var fileTypes = form.UploadFileType.Split(new[] { ';', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (!fileTypes.Any(ext => postedFile.FileName.EndsWith("." + ext)))
            {
                throw new OrchardException(T("File type not supported: " + Path.GetExtension(postedFile.FileName)));
            }
        }

        private void CheckFileSize(OFormPart form, HttpPostedFileBase postedFile)
        {
            if (postedFile.ContentLength / 1000 > form.UploadFileSizeLimit)
            {
                throw new OrchardException(T("File size exceeds allowed limit"));
            }
        }
    }
}