using Dapper;
using DocManager.Application.Logic;
using DocManager.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication1.Mappings;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {

        private AccountsLogic accountsLogic = new AccountsLogic();
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


        public ActionResult Login()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Accounts objUser)
        {
            if (ModelState.IsValid)
            {
                var user = accountsLogic.Login(objUser); 
                FormsAuthentication.SetAuthCookie(objUser.Email, false);
                Session["usuario"] = objUser.Email.ToLower();

                if (user.Id == 1)
                {

                    return RedirectToAction("AdminDashBoard", "Home");
                }
                else
                {
                    return RedirectToAction("UserDashBoard", "Home");
                }
            }
            ModelState.AddModelError("", "invalid Username or Password");
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult AdminDashBoard()
        {
            ViewBag.Usuario = Session["usuario"].ToString();



            return View();
            //if (Session["UserID"] != null)
            //{
            //    return View();
            //}
            //else
            //{
            //    return RedirectToAction("Login");
            //}
        }
        public ActionResult UserDashBoard()
        {
            ReceiptsLogic rl = new ReceiptsLogic();


            var request = rl.EmbarquesxCuenta(Session["usuario"].ToString());


            var mapperRequest = EmbarquesCuentasProfile.InitializeAutomapper();


            var response = mapperRequest.Map<List<Models.EmbarquesCuentas>>(request);

            return View(response);
            //if (Session["UserID"] != null)
            //{
            //    return View();
            //}
            //else
            //{
            //    return RedirectToAction("Login");
            //}
        }
    }
}