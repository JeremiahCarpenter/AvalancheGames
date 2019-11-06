using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AvalancheGamesWeb.Models;

namespace AvalancheGamesWeb.Controllers
{
    //[MustBeLoggedIn]
   [MustBeInRole(Roles = "Administrator,PowerUser")]
    public class CommentController : Controller
    {      
        List<SelectListItem> GetUserItems()
        {
            List<SelectListItem> ProposedReturnValue = new List<SelectListItem>();
            using (ContextBLL ctx = new ContextBLL())
            {
                List<UserBLL> users = ctx.GetUsers(0, 25);
                foreach (UserBLL user in users)
                {
                    SelectListItem item = new SelectListItem();
                    item.Value = user.UserID.ToString();
                    item.Text = user.UserName;
                    ProposedReturnValue.Add(item);
                }
            }
            return ProposedReturnValue;
        }
        List<SelectListItem> GetGameItems()
        {
            List<SelectListItem> ProposedReturnValue = new List<SelectListItem>();
            using (ContextBLL ctx = new ContextBLL())
            {
                List<GameBLL> games = ctx.GetGames(0, 25);
                foreach (GameBLL game in games)
                {
                    SelectListItem item = new SelectListItem();
                    item.Value = game.GameID.ToString();
                    item.Text = game.GameName;
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
            List<CommentBLL> Model = new List<CommentBLL>();
            try
            {
                using (ContextBLL ctx = new ContextBLL())
                {
                    ViewBag.TotalCount = ctx.ObtainCommentCount();
                    Model = ctx.GetComments(PageN * PageS, PageS);
                }
                return View("Index", Model);
            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex;
                return View("Error");
            }
        }

        [MustBeInRole(Roles = "Administrator,PowerUser")]

        // GET: Comment
        public ActionResult Index()
        {

            List<CommentBLL> Model = new List<CommentBLL>();
            try
            {
                using (ContextBLL ctx = new ContextBLL())
                {
                    ViewBag.PageNumber = 0;
                    ViewBag.PageSize = ApplicationConfig.DefaultPageSize;
                    ViewBag.TotalCount = ctx.ObtainCommentCount();
                    Model = ctx.GetComments(0, ViewBag.PageSize);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex;
                return View("Error");
            }
            return View(Model);
        }


        // GET: Comment/Details/5
        [MustBeInRole(Roles = "Administrator,PowerUser")]
        public ActionResult Details(int id)
        {
            CommentBLL Comment;
            try
            {
                using (ContextBLL ctx = new ContextBLL())
                {
                    Comment = ctx.FindCommentByCommentID(id);
                    if (null == Comment)
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
            return View(Comment);
        }

        // GET: Comment/Create
        [MustBeInRole(Roles = "Administrator,PowerUser")]
        public ActionResult Create()
        {
            CommentBLL defComment = new CommentBLL();
            defComment.CommentID = 0;
            ViewBag.GameName = GetGameItems();
           // ViewBag.UserName = GetGameItems();
            return View(defComment);
        }

   
        // POST: Comment/Create
        [HttpPost]
        [MustBeInRole(Roles = "Administrator,PowerUser")]
        public ActionResult Create(BusinessLogicLayer.CommentBLL collection)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(collection);
                }
                // TODO: Add insert logic here
                using (ContextBLL ctx = new ContextBLL())
                {
                    UserBLL userRecord = ctx.FindUserByUserName(User.Identity.Name);
                    if (null == userRecord)
                    {
                        return View("UserNotFound");
                    }
                    collection.UserID = userRecord.UserID;
                    ctx.CreateComment(collection);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex;
                return View("Error");
            }
        }


        // GET: Comment/Edit/5
        [MustBeInRole(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            CommentBLL Comment;
            try
            {
                using (ContextBLL ctx = new ContextBLL())
                {
                    Comment = ctx.FindCommentByCommentID(id);
                    if (null == Comment)
                    {
                        return View("ItemNotFound");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Excption = ex;
                return View("Error");
            }
            ViewBag.UserName = GetUserItems();
            ViewBag.GameName = GetGameItems();
            return View(Comment);
        }


        // POST: Comment/Edit/5
        [MustBeInRole(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Edit(int id, BusinessLogicLayer.CommentBLL collection)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(collection);
                }
                // TODO: Add update logic here
                using (ContextBLL ctx = new ContextBLL())
                {
                    ctx.UpdateComment(collection);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex;
                return View("Error");
            }
        }


        // GET: Comment/Delete/5
        [MustBeInRole(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            CommentBLL Comment;
            try
            {
                using (ContextBLL ctx = new ContextBLL())
                {
                    Comment = ctx.FindCommentByCommentID(id);
                    if (null == Comment)
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
            return View(Comment);
        }

        // POST: Comment/Delete/5
        [MustBeInRole(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Delete(int id, BusinessLogicLayer.CommentBLL collection)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(collection);
                }
                using (ContextBLL ctx = new ContextBLL())
                {
                    ctx.DeleteComment(id);
                }
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex;
                return View("Error");
            }
        }
    }
}
