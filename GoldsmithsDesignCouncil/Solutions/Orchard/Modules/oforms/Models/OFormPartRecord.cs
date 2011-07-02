using System.Collections.Generic;
using Orchard.ContentManagement.Records;
using Orchard.Data.Conventions;
using System.ComponentModel.DataAnnotations;

namespace oforms.Models
{
    public class OFormPartRecord : ContentPartRecord
    {
        public OFormPartRecord() {
            FormResults = new List<OFormResultRecord>();
        }

        public virtual string Name { get; set; }

        [StringLengthMax]
        public virtual string InnerHtml { get; set; }

        public virtual string Method { get; set; }

        public virtual string Action { get; set; }

        public virtual string RedirectUrl { get; set; }

        public virtual bool CanUploadFiles { get; set; }

        public virtual long UploadFileSizeLimit { get; set; }

        /// <summary>
        /// file format (separate by ; , or space)
        /// </summary>
        public virtual string UploadFileType { get; set; }

        public virtual bool UseCaptcha { get; set; }

        public virtual bool SendEmail { get; set; }

        public virtual string EmailFromName { get; set; }

        public virtual string EmailFrom { get; set; }

        public virtual string EmailSubject { get; set; }

        /// <summary>
        /// To? (separate by ; , or space)
        /// </summary>
        public virtual string EmailSendTo { get; set; }

        [StringLengthMax]
        public virtual string EmailTemplate { get; set; }

        public virtual bool SaveResultsToDB { get; set; }

        [StringLength(300)]
        public virtual string ValRequiredFields { get; set; }

        [StringLength(300)]
        public virtual string ValNumbersOnly { get; set; }

        [StringLength(300)]
        public virtual string ValLettersOnly { get; set; }

        [StringLength(300)]
        public virtual string ValLettersAndNumbersOnly { get; set; }

        [StringLength(300)]
        public virtual string ValDate { get; set; }

        [StringLength(300)]
        public virtual string ValEmail { get; set; }

        [StringLength(300)]
        public virtual string ValUrl { get; set; }

        public virtual IList<OFormResultRecord> FormResults { get; set; }

        public virtual void AddFormResult(OFormResultRecord resultRecord)
        {
            resultRecord.OFormPartRecord = this;
            FormResults.Add(resultRecord);
        }

    }
}