using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogicLayer;
using AvalancheGamesWeb.Models;

namespace AvalancheGamesWeb.Controllers
{

    [MustBeLoggedIn]
    public class GameController : Controller
    {
        public ActionResult Page(int? PageNumber, int? PageSize)
        {
            int PageN = (PageNumber.HasValue) ? PageNumber.Value : 0;
            int PageS = (PageSize.HasValue) ? PageSize.Value : ApplicationConfig.DefaultPageSize;
            ViewBag.PageNumber = PageNumber;
            ViewBag.PageSize = PageSize;
            List<GameBLL> Model = new List<GameBLL>();
            try
            {
                using (ContextBLL ctx = new ContextBLL())
                {
                    ViewBag.TotalCount = ctx.ObtainGameCount();
                    Model = ctx.GetGames(PageN * PageS, PageS);

                }
                return View("Index", Model);
            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex;
                return View("Errorr");
            }
        }

        //List<SelectListItem> GetUserItems()
        //{
        //    List<SelectListItem> ProposedReturnValue = new List<SelectListItem>();
        //    using (ContextBLL ctx = new ContextBLL())
        //    {
        //        List<UserBLL> users = ctx.GetUsers(0, 25);
        //        foreach (UserBLL user in users)
        //        {
        //            SelectListItem item = new SelectListItem();
        //            item.Value = user.UserID.ToString();
        //            item.Text = user.Email;
        //            ProposedReturnValue.Add(item);
        //        }
        //    }
        //    return ProposedReturnValue;
        //}
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
        // GET: Game
        public ActionResult Index()
        {
            List<GameBLL> Model = new List<GameBLL>();
            try
            {
                using (ContextBLL ctx = new ContextBLL())
                {
                    ViewBag.PageNumber = 0;
                    ViewBag.PageSize = ApplicationConfig.DefaultPageSize;
                    ViewBag.TotalCount = ctx.ObtainGameCount();
                    Model = ctx.GetGames(0, ViewBag.PageSize);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex;
                return View("Error");
            }
            return View(Model);//model is list of roles, name of view is same as method name
        }

        // GET: Game/Details/5
        public ActionResult Details(int id)
        {
            GameBLL Game;
            try
            {
                using (ContextBLL ctx = new ContextBLL())
                {
                    Game = ctx.FindGameByGameID(id);
                    if (null == Game)
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
            return View(Game);
        }

        // GET: Game/Create
        [MustBeInRole(Roles = "Administrator")]
        public ActionResult Create()
        {
            GameBLL defGame = new GameBLL();
            defGame.GameID = 0;
            return View(defGame);
        }

        // POST: Game/Create
        [MustBeInRole(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Create(BusinessLogicLayer.GameBLL collection)
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
                    ctx.CreateGame(collection);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex;
                return View("Error");
            }
        }

        // GET: Game/Edit/5
        [MustBeInRole(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            GameBLL Game;
            try
            {
                using (ContextBLL ctx = new ContextBLL())
                {
                    Game = ctx.FindGameByGameID(id);
                    if (null == Game)
                    {
                        ViewBag.GameName = GetGameItems();
                        return View("ItemNotFound");

                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex;
                return View("Error");
            }
            ViewBag.GameName = GetGameItems();
            return View(Game);
        }

        // POST: Game/Edit/5
        [MustBeInRole(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Edit(int id, BusinessLogicLayer.GameBLL collection)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(collection);
                }// TODO: Add update logic here
                using (ContextBLL ctx = new ContextBLL())
                {
                    ctx.UpdateGame(collection);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.exception = ex;
                return View("Error");
            }
        }

        // GET: Game/Delete/5
        [MustBeInRole(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            GameBLL Game;
            try
            {
                using (ContextBLL ctx = new ContextBLL())
                {
                    Game = ctx.FindGameByGameID(id);
                    if (null == Game)
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
            return View(Game);
        }

        // POST: Game/Delete/5
        [MustBeInRole(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Delete(int id, BusinessLogicLayer.GameBLL collection)
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
                    ctx.DeleteGame(id);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex;
                return View("Error");
            }
        }

        public ActionResult Snake()
        {
            return View();
        }

        public ActionResult SnakeResult()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SnakeResult(int id)
        {
            using (ContextBLL ctx = new ContextBLL())
            {
                ViewBag.score = id;
                TempData["score"] = id;
                return View("SnakeScore");
            }
        }

        [HttpGet]
        public ActionResult SnakeSaveScore()
        {
            try
            {
                using (ContextBLL ctx = new ContextBLL())
                {
                    ScoreBLL thisScore = new ScoreBLL();

                    var name = ctx.FindUserByUserName(User.Identity.Name);
                    thisScore.Score = (int)TempData["score"];
                    thisScore.UserID = name.UserID;
                    thisScore.GameID = Constants.SnakeGame;

                    ctx.CreateScore(thisScore);

                    return RedirectToAction("Index", "MyScores");
                }
            }
            catch (Exception Ex)
            {
                ViewBag.Exception = Ex;
                return View("Error");
            }
        }

        public ActionResult PongBall()
        {
            return View();
        }


        public ActionResult PongBallResult()
        {
            return View();
        }


        [HttpGet]
        public ActionResult PongBallResult(int id)
        {
            using (ContextBLL ctx = new ContextBLL())
            {
                ViewBag.score = id;
                TempData["score"] = id;
                return View("PongballScore");
            }
        }

        [HttpGet]
        public ActionResult PongBallSaveScore()
        {
            try
            {
                using (ContextBLL ctx = new ContextBLL())
                {
                    ScoreBLL thisScore = new ScoreBLL();

                    var name = ctx.FindUserByUserName(User.Identity.Name);
                    thisScore.Score = (int)TempData["score"];
                    thisScore.UserID = name.UserID;
                    thisScore.GameID = Constants.PongBallGame;

                    ctx.CreateScore(thisScore);

                    return RedirectToAction("Index", "MyScores");
                }
            }
            catch (Exception Ex)
            {
                ViewBag.Exception = Ex;
                return View("Error");
            }
        }

        public ActionResult Floaty()
        {
            return View();
        }

        public ActionResult FloatyResult()
        {
            return View();
        }

        [HttpGet]
        public ActionResult FloatyResult(int id)
        {
            using (ContextBLL ctx = new ContextBLL())
            {
                ViewBag.score = id;
                TempData["score"] = id;
                return View("FloatyScore");
            }
        }

        [HttpGet]
        public ActionResult FloatySaveScore()
        {
            try
            {
                using (ContextBLL ctx = new ContextBLL())
                {
                    ScoreBLL thisScore = new ScoreBLL();

                    var name = ctx.FindUserByUserName(User.Identity.Name);
                    thisScore.Score = (int)TempData["score"];
                    thisScore.UserID = name.UserID;
                    thisScore.GameID = Constants.FloatyGame;

                    ctx.CreateScore(thisScore);

                    return RedirectToAction("Index", "MyScores");
                }
            }
            catch (Exception Ex)
            {
                ViewBag.Exception = Ex;
                return View("Error");
            }
        }
        #region Original ScoreSavingCode(Currently no longer needed)
        //public ActionResult Snake()
        //{
        //    return View();
        //}

        //public ActionResult SnakeResult()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult SnakeResult(ScoreBLL collection)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return View(collection);
        //        }
        //        using (ContextBLL ctx = new ContextBLL())
        //        {
        //            ctx.CreateScore(collection);
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception Ex)
        //    {
        //        ViewBag.Exception = Ex;
        //        return View("Error");
        //    }
        //}
        //[HttpGet]
        //public ActionResult SnakeResult(int id)
        //{

        //    using (ContextBLL ctx = new ContextBLL())
        //    {
        //        ScoreBLL thisScore = new ScoreBLL();
        //            // string email = Session["AUTHEmail"].ToString();
        //            var name = ctx.FindUserByUserName(User.Identity.Name);
        //            thisScore.Score = id;
        //            thisScore.UserID = name.UserID;
        //            thisScore.GameID = Constants.SnakeGame;
        //            ViewBag.score = id;
        //        ctx.CreateScore(thisScore);
        //        //return View("SnakeScore");
        //        // return View(thisScore);
        //        return RedirectToAction("Index", "MyScores");
        //        //return View("SnakeScore");
        //    }
        //    }






        //public ActionResult PongBall()
        //{
        //    return View();
        //}


        //public ActionResult PongBallResult()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult PongBallResult(ScoreBLL collection)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return View(collection);
        //        }
        //        using (ContextBLL ctx = new ContextBLL())
        //        {
        //            ctx.CreateScore(collection);
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception Ex)
        //    {
        //        ViewBag.Exception = Ex;
        //        return View("Error");
        //    }
        //}
        //[HttpGet]
        //public ActionResult PongBallResult(int id)
        //{

        //    using (ContextBLL ctx = new ContextBLL())
        //    {
        //        ScoreBLL thisScore = new ScoreBLL();
        //        // string email = Session["AUTHEmail"].ToString();
        //        var name = ctx.FindUserByUserName(User.Identity.Name);
        //        thisScore.Score = id;
        //        thisScore.UserID = name.UserID;
        //        thisScore.GameID = Constants.PongBallGame;
        //        ViewBag.score = id;
        //        ctx.CreateScore(thisScore);
        //        //return View("SnakeScore");
        //        // return View(thisScore);
        //        return RedirectToAction("Index", "MyScores");
        //    }
        //}
        //public ActionResult Floaty()
        //{
        //    return View();
        //}


        //public ActionResult FloatyResult()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult FloatyResult(ScoreBLL collection)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return View(collection);
        //        }
        //        using (ContextBLL ctx = new ContextBLL())
        //        {
        //            ctx.CreateScore(collection);
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception Ex)
        //    {
        //        ViewBag.Exception = Ex;
        //        return View("Error");
        //    }
        //}
        //[HttpGet]
        //public ActionResult FloatyResult(int id)
        //{

        //    using (ContextBLL ctx = new ContextBLL())
        //    {
        //        ScoreBLL thisScore = new ScoreBLL();
        //        // string email = Session["AUTHEmail"].ToString();
        //        var name = ctx.FindUserByUserName(User.Identity.Name);

        //        thisScore.Score = id;
        //        thisScore.UserID = name.UserID;
        //        thisScore.GameID = Constants.FloatyGame;
        //        ViewBag.score = id;
        //        ctx.CreateScore(thisScore);
        //        //return View("SnakeScore");
        //        // return View(thisScore);
        //        return RedirectToAction("Index", "MyScores");
        //    }
        //}
        //[HttpPost]
        //public ActionResult SnakeResult(ScoreBLL collection)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return View(collection);
        //        }
        //        using (ContextBLL ctx = new ContextBLL())
        //        {
        //            ctx.CreateScore(collection);
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception Ex)
        //    {
        //        ViewBag.Exception = Ex;
        //        return View("Error");
        //    }
        //}
        //public ActionResult PongBall()
        //{
        //    return View();
        //}
        //public ActionResult PongBallResult(int id)
        //{
        //    ViewBag.score = id;
        //    return View("PongballScore");
        //}
        //public ActionResult Floaty()
        //{
        //    return View();
        //}
        //public ActionResult FloatyResult(int id)
        //{
        //    ViewBag.score = id;
        //    return View("FloatyScore");
        //}

        //public ActionResult SnakeResult()
        //{
        //    return View();
        //}
        //public ActionResult SnakeResult(int id)
        //{
        //    ViewBag.score = id;
        //    return View("SnakeScore");
        //}
        #endregion
    }
}
