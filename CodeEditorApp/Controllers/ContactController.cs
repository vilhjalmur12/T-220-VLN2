using CodeEditorApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CodeEditorApp.Controllers
{
    public class ContactController : Controller
    {

        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.LogIn = new LoginViewModel();
            ViewBag.Register = new RegisterViewModel();
            ViewBag.Contact = new ContactViewModel();
            ViewBag.Make = "Contact";
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MailMessage msg = new MailMessage();
                    //Email from user that is trying to contact us, here we will know that the email is from our website
                    msg.From = new MailAddress("collabcodeinfo@gmail.com");
                    //Our email
                    msg.To.Add("collabcodeinfo@gmail.com");
                    msg.Subject = "Email sent from website, from: " + vm.Email;
                    msg.Body = vm.Message;

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;

                    smtp.Credentials = new System.Net.NetworkCredential
                    ("collabcodeinfo@gmail.com", "CollabCodeRu");

                    smtp.EnableSsl = true;

                    smtp.Send(msg);

                    ModelState.Clear();
                    ViewBag.Message = "Thank you for Contacting us ";
                }
                catch (Exception ex)
                {
                    ModelState.Clear();
                    ViewBag.Message = $" Sorry we are facing Problem here {ex.Message}";
                }
            }
            ViewBag.LogIn = new LoginViewModel();
            ViewBag.Register = new RegisterViewModel();
            ViewBag.Contact = vm;
            ViewBag.Make = "Contact";
            return View("../Home/Index", vm);
        }
        public ActionResult Error()
        {
            return View();
        }
    }
}
