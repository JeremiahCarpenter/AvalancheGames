using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogicLayer;
using AvalancheGamesWeb.Models;

namespace AvalancheGamesWeb.Controllers
{
    public class RegisterUserController : Controller
    {
        public List<SelectListItem> GetGameItems(ContextBLL ctx)
        {
            List<SelectListItem> ProposedReturnValue = new List<SelectListItem>();
            List<GameBLL> games = ctx.GetGames(0, 25);
            foreach (GameBLL game in games)
            {
                SelectListItem item = new SelectListItem();
                item.Value = game.GameID.ToString();
                item.Text = game.GameName;
                ProposedReturnValue.Add(item);
            }
            return ProposedReturnValue;
        }

        //List<SelectListItem> GetRoleItems()
        //{
        //    List<SelectListItem> ProposedReturnValue = new List<SelectListItem>();
        //    using (ContextBLL ctx = new ContextBLL())
        //    {
        //        List<RoleBLL> roles = ctx.GetRoles(0, 25);
        //        foreach (RoleBLL role in roles)
        //        {
        //            SelectListItem item = new SelectListItem();
        //            item.Value = role.RoleID.ToString();
        //            item.Text = role.RoleName;
        //            ProposedReturnValue.Add(item);
        //        }
        //    }
        //    return ProposedReturnValue;
        //}


        // GET: RegisterUser/Create
        public ActionResult Register()
        {
            RegistrationModel Model = new RegistrationModel();
            using (ContextBLL ctx = new ContextBLL())
            {
                ViewBag.Games = GetGameItems(ctx);
                UserBLL defGame = new UserBLL();
                defGame.UserID = 0;
                CommentBLL defComment = new CommentBLL();
                defComment.CommentID = 0;
                GameBLL thisGame = new GameBLL();
                //GameBLL game = ctx.FindGameByGameID(id);
                thisGame.GameID = 0;
                //CommentBLL comment = ctx.FindCommentByCommentID(id);
                //List<CommentBLL> comment = ctx.GetComments(skip, take);
                //return View(Model);
            }
            return View(Model);
        }

        // POST: RegisterUser/Create
        [HttpPost]
        public ActionResult Register(Models.RegistrationModel collection)
        {
            try
            {
                using (ContextBLL ctx = new ContextBLL())
                {
                    if (!ModelState.IsValid)
                    {
                        ViewBag.Games = GetGameItems(ctx);
                        return View(collection);
                    }
                    UserBLL user = ctx.FindUserByUserName(collection.UserName);
                    if (user != null)
                    {
                        collection.Message = $"The UserName  '{collection.UserName}' already exists in the database";
                        ViewBag.Games = GetGameItems(ctx);
                        return View(collection);
                    }
                    user = new UserBLL();
                    CommentBLL comment = new CommentBLL();
                    user.FirstName = collection.FirstName;
                    user.LastName = collection.LastName;
                    user.UserName = collection.UserName;
                    user.DateOfBirth = collection.DateOfBirth;
                    user.SALT = System.Web.Helpers.Crypto.GenerateSalt(Constants.SaltSize);
                    user.HASH = System.Web.Helpers.Crypto.HashPassword(collection.Password + user.SALT);
                    user.Email = collection.Email;
                    user.RoleID = 3;
                    //comment.Liked = collection.Liked;
                    comment.Liked = true;
                    comment.GameID = collection.GameID;
                    comment.GameName = collection.GameName;
                    comment.GameComment = "User Initial Comment";
                    comment.UserID = ctx.CreateUser(user);
                    
                    ctx.CreateComment(comment);
                    Session["AUTHUserName"] = user.UserName;
                    Session["AUTHRoles"] = user.RoleName;
                    Session["AUTHTYPE"] = "HASHED";



                }
                return RedirectToAction("Index","Home");
            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex;
                return View("Error");
            }
        }

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
        public ActionResult Impersonate(string UserName)
        {
            UserBLL user;
            try
            {
                using (ContextBLL ctx = new ContextBLL())
                {
                    user = ctx.FindUserByUserName(UserName);
                    if (null == user)
                    {
                        return View("ItemNotFound"); // BKW make this view
                    }
                    Session["AUTHUserName"] = user.UserName;
                    Session["AUTHRoles"] = user.RoleName;
                    Session["AUTHTYPE"] = $"IMPERSONATED:{user.UserID}";

                }

            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex;
                return View("Error");
            }

            return RedirectToAction("Index", "Home");
        }

    }
}