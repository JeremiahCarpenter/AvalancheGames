using AvalancheGamesWeb.Models;
using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AvalancheGamesWeb.Controllers
{
    [MustBeInRole(Roles = "Administrator")]

    public class UserController : Controller
    {
        //returning a list  List is the return type
        List<SelectListItem> GetRoleItems()
        {//creating empty list items
            List<SelectListItem> ProposedReturnValue = new List<SelectListItem>();
            using (ContextBLL ctx = new ContextBLL())
            {
                List<RoleBLL> roles = ctx.GetRoles(0, 25);
                foreach (RoleBLL role in roles)
                {//redefining object
                    SelectListItem item = new SelectListItem();
                    item.Value = role.RoleID.ToString();
                    item.Text = role.RoleName;
                    ProposedReturnValue.Add(item);
                }
            }
            return ProposedReturnValue;
        }
        public ActionResult Page(int? PageNumber, int? PageSize)
        {
            int PageN = (PageNumber.HasValue) ? PageNumber.Value : 0;
            int PageS = (PageSize.HasValue) ? PageSize.Value : ApplicationConfig.DefaultPageSize;
            ViewBag.PageNumber = PageNumber;
            ViewBag.PageSize = PageSize;
            List<UserBLL> Model = new List<UserBLL>();
            try
            {
                using (ContextBLL ctx = new ContextBLL())
                {
                    ViewBag.TotalCount = ctx.ObtainUserCount();
                    Model = ctx.GetUsers(PageN * PageS, PageS);
                }
                return View("Index", Model);
            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex;
                return View("Error");
            }
        }
        //[MustBeInRole(Roles = "Administrator")]
        // GET: User
        public ActionResult Index()
        {
            List<UserBLL> Model = new List<UserBLL>();
            try
            {
                using (ContextBLL ctx = new ContextBLL())
                {
                    ViewBag.PageNumber = 0;
                    ViewBag.PageSize = ApplicationConfig.DefaultPageSize;
                    ViewBag.TotalCount = ctx.ObtainUserCount();
                    Model = ctx.GetUsers(0, ViewBag.PageSize);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex;
                return View("Error");
            }
            return View(Model);//model is list of roles, name of view is same as method  name
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            UserBLL User;
            try
            {
                using (ContextBLL ctx = new ContextBLL())
                {
                    User = ctx.FindUserByUserID(id);
                    if (null == User)
                    {
                        return View("ItemNotFound");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex;
                return View("Error");
            }
            return View(User);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            Models.CreateUser NewUser = new CreateUser();
            NewUser.RoleID = 0;
            //UserBLL defUser = new UserBLL();
            ViewBag.Roles = GetRoleItems();
            return View(NewUser);
            //defUser.UserID = 0;
            //return View(defUser);
        }

        // POST: User/Create

        [HttpPost]
        public ActionResult Create(Models.CreateUser info)
        {
            try
            { 
            //using (BusinessLogicLayer.ContextBLL ctx = new BusinessLogicLayer.ContextBLL())
            //{
                if (!ModelState.IsValid)
                {
                    return View(info);
                }
                using (BusinessLogicLayer.ContextBLL ctx = new BusinessLogicLayer.ContextBLL())
                {
                    BusinessLogicLayer.UserBLL user = ctx.FindUserByUserName(info.UserName);
                    //if (user != null)
                    //{
                    //    info.Message = $"The EMail Address '{info.Email}' already exists in the database";
                    //    return View(info);
                    //}
                    user = new UserBLL();
                    user.FirstName = info.FirstName;
                    user.LastName = info.LastName;
                    user.UserName = info.UserName;
                    user.DateOfBirth = info.DateOfBirth;
                    user.RoleID = info.RoleID;
                    user.SALT = System.Web.Helpers.Crypto.
                        GenerateSalt(Constants.SaltSize);
                    user.HASH = System.Web.Helpers.Crypto.
                        HashPassword(info.Password + user.SALT);
                    user.Email = info.Email;
                    ctx.CreateUser(user);
                    Session["AUTHUserName"] = user.UserName;
                    Session["AUTHRoles"] = user.RoleName;
                    Session["AUTHTYPE"] = "HASHED";
                }
                return RedirectToAction("Index");
            }
            catch (Exception Ex)
            {
                ViewBag.Exception = Ex;
                return View("Error");
            }
        }

        //public ActionResult Create(BusinessLogicLayer.UserBLL collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here
        //        using (ContextBLL ctx = new ContextBLL())
        //        {
        //            ViewBag.Roles = GetRoleItems();
        //            ctx.CreateUser(collection);
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Exception = ex;
        //        return View("Error");
        //    }
        //}

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            UserBLL User;
            try
            {
                using (ContextBLL ctx = new ContextBLL())
                {
                    User = ctx.FindUserByUserID(id);
                    if (null == User)
                    {
                        ViewBag.Roles = GetRoleItems();
                        return View("ItemNotFound");

                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex;
                return View("Error");
            }
            ViewBag.Roles = GetRoleItems();
            return View(User);
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, BusinessLogicLayer.UserBLL collection)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(collection);
                }
                using (ContextBLL ctx = new ContextBLL())
                {

                    ctx.UpdateUser(collection);
                }
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex;
                return View("Error");
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            UserBLL User;
            try
            {
                using (ContextBLL ctx = new ContextBLL())
                {
                    User = ctx.FindUserByUserID(id);
                    if (null == User)
                    {
                        return View("ItemNotFound");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex;
                return View("Error");
            }
            return View(User);
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, BusinessLogicLayer.UserBLL collection)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(collection);
                }
                // TODO: Add delete logic here
                using (ContextBLL ctx = new ContextBLL())
                {
                    ctx.DeleteUser(id);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex;
                return View("Error");
            }
        }
        public ActionResult Impersonate(int id)
        {
            UserBLL user;
            try
            {
                using (ContextBLL ctx = new ContextBLL())
                {
                    user = ctx.FindUserByUserID(id);
                    if (null == user)
                    {
                        return View("ItemNotFound"); 
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
