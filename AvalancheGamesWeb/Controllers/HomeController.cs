using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogicLayer;


namespace AvalancheGamesWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        //public ActionResult Roles()
        //{
        //    using (BusinessLogicLayer.ContextBLL ctx = new BusinessLogicLayer.ContextBLL())
        //    {
        //        List<BusinessLogicLayer.RoleBLL> model = ctx.GetRoles(0, 100);
        //        return View(model);
        //    }
        //}

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Remove("AUTHUserName");
            Session.Remove("AUTHRoles");
            Session.Remove("AuthType");
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Login()
        {
            // displays empty login screen with predefined returnURL
            Models.LoginModel mapper = new Models.LoginModel();
            mapper.Message = TempData["Message"]?.ToString() ?? "";
            mapper.ReturnURL = TempData["ReturnURL"]?.ToString() ?? @"~/Home";
            mapper.UserName = "";
            mapper.Password = "";
            return View(mapper);
        }
        [HttpPost]
        public ActionResult Login(Models.LoginModel info)
        {
            if (!ModelState.IsValid)
            {
                return View(info);
            }
            using (BusinessLogicLayer.ContextBLL ctx = new BusinessLogicLayer.ContextBLL())
            {
                BusinessLogicLayer.UserBLL user = ctx.FindUserByUserName(info.UserName);
                if (user == null)
                {
                    info.Message = $"The UserName '{info.UserName}' does not exist in the database";
                    return View(info);
                }
                string actual = user.HASH;
                string potential = info.Password;  
                string ValidationType = $"ClearText:({user.UserID})";
                //bool validateduser = potential == actual;
                bool validateduser = potential == actual;
                if (!validateduser)
                {
                    potential = info.Password + user.SALT;

                    validateduser = System.Web.Helpers.Crypto.VerifyHashedPassword(actual, potential);
                    ValidationType = $"HASHED:({user.UserID})";
                }
                if (validateduser)
                {
                    Session["AUTHUserName"] = user.UserName;
                    Session["AUTHRoles"] = user.RoleName;
                    Session["AUTHTYPE"] = ValidationType;
                    return Redirect(info.ReturnURL);
                }
                info.Message = "The UserName or Password was incorrect";
                return View(info);
            }
        }
        //public ActionResult Register()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Register(Models.RegistrationModel info)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(info);
        //    }
        //    using (BusinessLogicLayer.ContextBLL ctx = new BusinessLogicLayer.ContextBLL())
        //    {
        //        BusinessLogicLayer.UserBLL user = ctx.FindUserByUserName(info.UserName);
        //        //if (user != null)
        //        //{
        //        //    info.Message = $"The EMail Address '{info.Email}' already exists in the database";
        //        //    return View(info);
        //        //}
        //        user = new UserBLL();
        //        user.FirstName = info.FirstName;
        //        user.LastName = info.LastName;
        //        user.UserName = info.UserName;
        //        user.DateOfBirth = info.DateOfBirth;
        //        user.SALT = System.Web.Helpers.Crypto.
        //            GenerateSalt(Constants.SaltSize);
        //        user.HASH = System.Web.Helpers.Crypto.
        //            HashPassword(info.Password + user.SALT);
        //        user.Email = info.Email;
        //        user.RoleID = 3;
        //        ctx.CreateUser(user);
        //        Session["AUTHUserName"] = user.UserName;
        //        Session["AUTHRoles"] = user.RoleName;
        //        Session["AUTHTYPE"] = "HASHED";
        //        return RedirectToAction("Index");
        //    }
        //}

        public ActionResult Hash()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View("NotLoggedIn");

            }
            if (User.Identity.AuthenticationType.StartsWith("HASHED"))
            {
                return View("AlreadyHashed");
            }
            if (User.Identity.AuthenticationType.StartsWith("IMPERSONATED"))
            {
                return View("ActionNotAllowed");
            }
            using (BusinessLogicLayer.ContextBLL ctx = new BusinessLogicLayer.ContextBLL())
            {
                BusinessLogicLayer.UserBLL user = ctx.FindUserByUserName(User.Identity.Name);
                if (user == null)
                {
                    Exception Message = new Exception($"The UserName '{User.Identity.Name}' does not exist in the database");
                    ViewBag.Exception = Message;
                    return View("Error");
                }
                user.SALT = System.Web.Helpers.Crypto.GenerateSalt(Constants.SaltSize);
                user.HASH = System.Web.Helpers.Crypto.HashPassword(user.HASH + user.SALT);
                ctx.UpdateUser(user);

                string ValidationType = $"HASHED:({user.UserID})";

                Session["AUTHUserName"] = user.UserName;
                Session["AUTHRoles"] = user.RoleName;
                Session["AUTHTYPE"] = ValidationType;

                return RedirectToAction("Index", "Home");
            }
        }
    }
}