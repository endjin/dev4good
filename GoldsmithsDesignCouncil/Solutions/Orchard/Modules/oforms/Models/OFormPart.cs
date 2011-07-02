using System.ComponentModel.DataAnnotations;
using Orchard.ContentManagement;

namespace oforms.Models
{
    public class OFormPart : ContentPart<OFormPartRecord>
    {
        [Required]
        public string Method
        {
            get { return Record.Method; }
            set { Record.Method = value; }
        }

        [Required]
        [RegularExpression("[a-zA-Z0-9\\-]{1,40}", ErrorMessage = "The field Name can be alphanumeric, no spaces, up to 40 characters")]
        public string Name
        {
            get {
                return Record.Name;
            }

            set {
                Record.Name = value;
            }
        }
        
        public string Action
        {
            get { return Record.Action; }
            set { Record.Action = value; }
        }

        public string RedirectUrl
        {
            get { return Record.RedirectUrl; }
            set { Record.RedirectUrl = value; }
        }

        public string InnerHtml
        {
            get { return Record.InnerHtml; }
            set { Record.InnerHtml = value; }
        }

        public bool CanUploadFiles
        {
            get { return Record.CanUploadFiles; }
            set { Record.CanUploadFiles = value; }
        }

        public long UploadFileSizeLimit
        {
            get { return Record.UploadFileSizeLimit; }
            set { Record.UploadFileSizeLimit = value; }
        }

        public string UploadFileType
        {
            get { return Record.UploadFileType; }
            set { Record.UploadFileType = value; }
        }

        public bool UseCaptcha
        {
            get { return Record.UseCaptcha; }
            set { Record.UseCaptcha = value; }
        }

        public bool SendEmail
        {
            get { return Record.SendEmail; }
            set { Record.SendEmail = value; }
        }

        public string EmailFromName
        {
            get { return Record.EmailFromName; }
            set { Record.EmailFromName = value; }
        }

        public string EmailFrom
        {
            get { return Record.EmailFrom; }
            set { Record.EmailFrom = value; }
        }

        public string EmailSubject
        {
            get { return Record.EmailSubject; }
            set { Record.EmailSubject = value; }
        }

        public string EmailSendTo
        {
            get { return Record.EmailSendTo; }
            set { Record.EmailSendTo = value; }
        }

        public string EmailTemplate
        {
            get { return Record.EmailTemplate; }
            set { Record.EmailTemplate = value; }
        }

        public bool SaveResultsToDB
        {
            get { return Record.SaveResultsToDB; }
            set { Record.SaveResultsToDB = value; }
        }

        public string ValRequiredFields
        {
            get { return Record.ValRequiredFields; }
            set { Record.ValRequiredFields = value; }
        }

        public string ValNumbersOnly
        {
            get { return Record.ValNumbersOnly; }
            set { Record.ValNumbersOnly = value; }
        }

        public string ValLettersOnly
        {
            get { return Record.ValLettersOnly; }
            set { Record.ValLettersOnly = value; }
        }

        public string ValLettersAndNumbersOnly
        {
            get { return Record.ValLettersAndNumbersOnly; }
            set { Record.ValLettersAndNumbersOnly = value; }
        }

        public string ValDate
        {
            get { return Record.ValDate; }
            set { Record.ValDate = value; }
        }

        public string ValEmail
        {
            get { return Record.ValEmail; }
            set { Record.ValEmail = value; }
        }

        public string ValUrl
        {
            get { return Record.ValUrl; }
            set { Record.ValUrl = value; }
        }

        public bool IsPublished
        {
            get { return ContentItem.VersionRecord != null && ContentItem.VersionRecord.Published; }
        }

        public bool HasDraft
        {
            get
            {
                return (
                           (ContentItem.VersionRecord != null) && (
                               (ContentItem.VersionRecord.Published == false) ||
                               (ContentItem.VersionRecord.Published && ContentItem.VersionRecord.Latest == false)));
            }
        }

        public bool HasPublished
        {
            get
            {
                return IsPublished || ContentItem.ContentManager.Get(Id, VersionOptions.Published) != null;
            }
        }
    }

}