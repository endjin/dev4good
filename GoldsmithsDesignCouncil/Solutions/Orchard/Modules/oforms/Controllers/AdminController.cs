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
    [ValidateInput(false)]
    public class AdminController : Controller, IUpdateModel
    {
        private readonly IOrchardServices _services;
        private readonly ISiteService _siteService;
        private readonly IOFormService _formService;
        private readonly ISerialService _serial;
        private readonly IContentManager _contentManager;
        private readonly ITransactionManager _transactionManager;
        private readonly IRepository<OFormResultRecord> _resultsRepo;
        private readonly IRepository<OFormFileRecord> _fileRepo;

        public AdminController(IOrchardServices services, 
            IShapeFactory shapeFactory, 
            ISiteService siteService,
            IOFormService formService, 
            ISerialService serial,
            IContentManager contentManager,
            ITransactionManager transactionManager,
            IRepository<OFormResultRecord> resultsRepo,
            IRepository<OFormFileRecord> fileRepo)
        {
            this._services = services;
            _siteService = siteService;
            this._formService = formService;
            this._serial = serial;
            this.Shape = shapeFactory;
            this._contentManager = contentManager;
            this._transactionManager = transactionManager;
            _resultsRepo = resultsRepo;
            this._fileRepo = fileRepo;
            T = NullLocalizer.Instance;
        }

        dynamic Shape { get; set; }
        public Localizer T { get; set; }

        public ActionResult Index()
        {
            if (!_services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to list users")))
                return new HttpUnauthorizedResult();

            var forms = _services.ContentManager.Query<OFormPart, OFormPartRecord>(VersionOptions.Latest).List();
            CheckValidSerial();
            return View(forms.ToList());
        }

        private void CheckValidSerial() {
            ViewData["validSn"] = _serial.ValidateSerial();
        }

        public ActionResult License()
        {
            if (!_services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to list users")))
                return new HttpUnauthorizedResult();
            var sn =  _serial.ReadSerialFromFile();
            ViewData["sn"] = sn;
            return View();
        }

        [HttpPost, ActionName("License")]
        public ActionResult LicensePOST(string sn)
        {
           if (!_services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to manage users")))
                return new HttpUnauthorizedResult();

            _serial.SaveSerialToFile(sn);
            
            if (_serial.ValidateSerial())
            {
                _services.Notifier.Information(T("License updated successfully, enjoy using oForms"));
            } else {
                _services.Notifier.Information(T("Incorrect License, please try again"));    
            }
            
           return RedirectToAction("License");
            
        }

        public ActionResult Create(string template)
        {
            if (!_services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to create forms")))
                return new HttpUnauthorizedResult();

            var form = _services.ContentManager.New<OFormPart>("OForm");
            if (form == null)
                return HttpNotFound();
            
            if (!string.IsNullOrEmpty(template)) {
            	OFormTemplateHelper.PreFillForm(template, form, _services);
            }

            dynamic model = _services.ContentManager.BuildEditor(form);
            // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
            return View((object)model);
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePOST(string apply)
        {
            if (!_services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to create forms")))
                return new HttpUnauthorizedResult();
            
            var form = _services.ContentManager.New<OFormPart>("OForm");
            _contentManager.Create(form, VersionOptions.Draft);
            dynamic model = _contentManager.UpdateEditor(form, this);

            if (!ModelState.IsValid)
            {
                _transactionManager.Cancel();
                // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
                return View((object)model);
            }

            _contentManager.Flush();

            _services.Notifier.Information(T("Form {0} created successfully", form.Name));
            if (string.IsNullOrEmpty(apply))
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Edit", new { form.Id });
        }

        public ActionResult Edit(int id)
        {
            if (!_services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not allowed to edit form")))
                return new HttpUnauthorizedResult();

            var form = _contentManager.Get(id, VersionOptions.Latest);
            if (form == null)
                return HttpNotFound();

            dynamic model = _services.ContentManager.BuildEditor(form);
            // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
            return View((object)model);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPOST(int id, string apply)
        {
            if (!_services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Couldn't edit form")))
                return new HttpUnauthorizedResult();

            var form = _contentManager.Get(id, VersionOptions.Latest);
            if (form == null)
                return HttpNotFound();

            dynamic model = _services.ContentManager.UpdateEditor(form, this);
            if (!ModelState.IsValid)
            {
                _services.TransactionManager.Cancel();
                // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
                return View((object)model);
            }

            _contentManager.Flush();
            _services.Notifier.Information(T("Form information updated"));

            if (string.IsNullOrEmpty(apply)) {
                return RedirectToAction("Index");
            }
            
            return RedirectToAction("Edit", new {id});
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (!_services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Couldn't delete form")))
                return new HttpUnauthorizedResult();

            var form = this._contentManager.Get(id, VersionOptions.Latest);
            if (form == null)
                return HttpNotFound();
            
            _contentManager.Remove(form);

            _services.Notifier.Information(T("Form {0} deleted successfully", form.As<OFormPart>().Name));

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Publish(int id)
        {
            if (!_services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Couldn't delete form")))
                return new HttpUnauthorizedResult();

            var form = this._contentManager.Get(id, VersionOptions.Latest);
            if (form == null)
                return HttpNotFound();
            
            _contentManager.Publish(form);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Unpublish(int id)
        {
            if (!_services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Couldn't delete form")))
                return new HttpUnauthorizedResult();

            var form = this._contentManager.Get(id, VersionOptions.Latest);
            if (form == null)
                return HttpNotFound();

            _contentManager.Unpublish(form);

            return RedirectToAction("Index");
        }

        public ActionResult ShowFormResults(int id, PagerParameters pagerParameters) 
        {
            if (!_services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to view form results")))
                return new HttpUnauthorizedResult();

            var form = this._contentManager.Get<OFormPart>(id, VersionOptions.Latest);
            if (form == null)
                return HttpNotFound();

            CheckValidSerial();
            var pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);

            var formResults = _resultsRepo.Table.Where(x => x.OFormPartRecord == form.Record);

            var pagerShape = Shape.Pager(pager).TotalItemCount(formResults.Count());
            var results = formResults
                .OrderByDescending(x => x.CreatedDate)
                .Skip(pager.GetStartIndex()).Take(pager.PageSize);

            return View(new FormResultViewModel {
                    FormName = form.Name,
                    Results = results.ToList(),
                    Pager = pagerShape
                });
        }

        public ActionResult ShowResultDetails(int id) 
        {
            if (!_services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to view form results")))
                return new HttpUnauthorizedResult();

            var result = _resultsRepo.Fetch(x => x.Id == id).SingleOrDefault();
            if (result == null) {
                return HttpNotFound();
            }

            this.CheckValidSerial();

            return View(result);
        }

        [HttpPost]
        public ActionResult DeleteResult(int id, int formId, int? currentPage, int? pageSize)
        {
            if (!_services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to view form results")))
                return new HttpUnauthorizedResult();

            var result = _resultsRepo.Fetch(x => x.Id == id && x.OFormPartRecord.Id == formId).SingleOrDefault();
            if (result == null)
            {
                return HttpNotFound();
            }

            this.CheckValidSerial();

            _resultsRepo.Delete(result);

            _services.Notifier.Information(T("Result was successfully removed"));

            return RedirectToAction("ShowFormResults", new { id = formId, page = currentPage, pageSize });
        }

        public ActionResult DownloadResultFile(int id)
        {
            if (!_services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to view result files")))
                return new HttpUnauthorizedResult();

            var file = _fileRepo.Fetch(x => x.Id == id).SingleOrDefault();
            if (file == null)
            {
                return HttpNotFound();
            }

            return File(file.Bytes, file.ContentType ?? "application/octet-stream", file.OriginalName);
        }

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties)
        {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        public void AddModelError(string key, LocalizedString errorMessage)
        {
            ModelState.AddModelError(key, errorMessage.ToString());
        }
    }
}