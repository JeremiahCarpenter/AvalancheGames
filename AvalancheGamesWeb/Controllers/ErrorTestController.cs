using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogicLayer;

namespace AvalancheGamesWeb.Controllers
{
    [AvalancheGamesWeb.Models.MustBeInRole(Roles = Constants.AdminRoleName)]
    public class ErrorTestController : Controller
    {
        // GET: ErrorTest
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SimulateDivideByZero()
        {//Onshore does not like single letter nameing conventions DON'T FORGET TO CHANGE THIS
            int i = 0;
            int j = 10 / i;
            return View();
        }
        public ActionResult SimulateDALNotConnected()
        {
            using (ContextBLL ctx = new ContextBLL())
            {
                ctx.GenerateNotConnected();
                return View();
            }
        }
        public ActionResult SimulateDALProcedureNotFound()
        {
            using (ContextBLL ctx = new ContextBLL())
            {
                ctx.GenerateStoredProcedureNotFound();
                return View();
            }
        }
        public ActionResult SimulateDALParameterNotFound()
        {
            using (ContextBLL ctx = new ContextBLL())
            {
                ctx.GenerateParameterNotIncluded();
                return View();
            }
        }
    }
}
