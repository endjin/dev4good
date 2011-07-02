using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using Orchard;
using Orchard.Localization;
using Orchard.Security;
using Orchard.Themes;
using Orchard.Mvc;
using Orchard.ContentManagement;
using Orchard.UI.Notify;
using oforms.Services;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;

namespace oforms.Controllers
{
    [Themed]
    public class HomeController : Controller
    {
        private readonly IOrchardServices _services;
        private readonly ISerialService _serial;
        private readonly IContentManager _contentManager;

        private readonly IOFormService _formService;

        public HomeController(IOFormService formService, 
                              IOrchardServices services,
                              ISerialService serial,
                              IContentManager contentManager)
        {
            this._formService = formService;
            _services = services;
            _serial = serial;
            _contentManager = contentManager;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public ActionResult Index(string name) {
            if (!_services.Authorizer.Authorize(StandardPermissions.AccessFrontEnd, T("Not authorized to view forms")))
                return new HttpUnauthorizedResult();

            var form = _formService.GetFormPartByName(name, VersionOptions.Published);

                if (form == null)
                    return HttpNotFound();
                dynamic model = _services.ContentManager.BuildDisplay(form);
                ViewBag.validSn = _serial.ValidateSerial();
                return new ShapeResult(this, model);
        }

        public ActionResult SubmitForm()
        {
            string name = Request.Params[OFormGlobals.NameKey];
            if (!_services.Authorizer.Authorize(StandardPermissions.AccessFrontEnd, T("Not authorized to submit forms")))
                return new HttpUnauthorizedResult();

            var form = _formService.GetFormPartByName(name, VersionOptions.Latest);
            if (form == null)
                return HttpNotFound();

            if (!form.IsPublished && !_services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to submit form")))
                return HttpNotFound();

            if (form.InnerHtml.Contains("{captcha}") 
                && (string)Session[OFormGlobals.CaptchaKey] != Request.Params[OFormGlobals.CaptchaKey])
            {
                _services.Notifier.Error(T("Incorrect captcha. Please try again."));
                return Index(name);
            }

            var submitData = new Dictionary<string, string>();
            Request.QueryString.CopyTo(submitData);
            Request.Form.CopyTo(submitData);
            submitData.Add(OFormGlobals.CreatedDateKey, DateTime.UtcNow.ToString("dd MMMM yyyy"));
            submitData.Add(OFormGlobals.IpKey, Request.UserHostAddress);
            _formService.SubmitForm(form, submitData, Request.Files, Request.UserHostAddress);

            if (string.IsNullOrEmpty(form.RedirectUrl)) {
                _services.Notifier.Information(T("Form submitted successfully"));

                return form.IsPublished ? RedirectToAction("Index", new { name })
                    : RedirectToAction("Preview", new { form.Id });
            }

            return Redirect(form.RedirectUrl);
        }

        public ActionResult Preview(int id)
        {
            if (!_services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not allowed to edit form")))
                return new HttpUnauthorizedResult();

            var form = _contentManager.Get(id, VersionOptions.Latest);
            if (form == null)
                return HttpNotFound();
            dynamic model = _services.ContentManager.BuildDisplay(form);
            ViewBag.validSn = _serial.ValidateSerial();
            return new ShapeResult(this, model);
        }

        public ActionResult GenerateCaptcha()
        {
            var bitmap = GenerateImage(150, 40);

            var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);

            return new FileStreamResult(ms, "image/bmp");
        }

        private Bitmap GenerateImage(int width, int height)
        {
            var random = new Random();
            string text = GenerateRandomText(random);
            //save to session
            this.Session[OFormGlobals.CaptchaKey] = text;

            // Create a new 32-bit bitmap image.
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            // Create a graphics object for drawing.
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, width, height);

            // Fill in the background.
            HatchBrush hatchBrush = new HatchBrush(
              HatchStyle.SmallConfetti,
              Color.LightGray,
              Color.White);
            g.FillRectangle(hatchBrush, rect);

            // Set up the text font.
            SizeF size;
            float fontSize = rect.Height + 1;
            Font font;
            // Adjust the font size until the text fits within the image.
            do
            {
                fontSize--;
                font = new Font(
                  "Arial",
                  fontSize,
                  FontStyle.Bold);
                size = g.MeasureString(text, font);
            } while (size.Width > rect.Width);

            // Set up the text format.
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            // Create a path using the text and warp it randomly.
            GraphicsPath path = new GraphicsPath();
            path.AddString(
              text,
              font.FontFamily,
              (int)font.Style,
              font.Size, rect,
              format);
            float v = 4F;
            PointF[] points =
                  {
                    new PointF(
                      random.Next(rect.Width) / v,
                      random.Next(rect.Height) / v),
                    new PointF(
                      rect.Width - random.Next(rect.Width) / v,
                      random.Next(rect.Height) / v),
                    new PointF(
                      random.Next(rect.Width) / v,
                      rect.Height - random.Next(rect.Height) / v),
                    new PointF(
                      rect.Width - random.Next(rect.Width) / v,
                      rect.Height - random.Next(rect.Height) / v)
                  };
            Matrix matrix = new Matrix();
            matrix.Translate(0F, 0F);
            path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);

            // Draw the text.
            hatchBrush = new HatchBrush(
              HatchStyle.LargeConfetti,
              Color.LightGray,
              Color.DarkGray);
            g.FillPath(hatchBrush, path);

            // Add some random noise.
            int m = Math.Max(rect.Width, rect.Height);
            for (int i = 0; i < (int)(rect.Width * rect.Height / 30F); i++)
            {
                int x = random.Next(rect.Width);
                int y = random.Next(rect.Height);
                int w = random.Next(m / 50);
                int h = random.Next(m / 50);
                g.FillEllipse(hatchBrush, x, y, w, h);
            }

            // Clean up.
            font.Dispose();
            hatchBrush.Dispose();
            g.Dispose();

            return bitmap;
        }

        private string GenerateRandomText(Random random)
        {
            var ret = string.Empty;
            for (int i = 0; i < 5; i++) 
            {
                if (random.Next() % 3 == 0)
                {
                    ret += (char)random.Next(65, 91);
                }
                else 
                {
                    ret += (char)random.Next(97, 123);
                }
            }

            return ret;
        }
    }
}