using DocManager.Application.Logic;
using DocManager.Core;
using System.Web.Mvc;
using System.Web.Security;

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



                if (user != null)
                {
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
            return View(rl.EmbarquesxCuenta(Session["usuario"].ToString()));
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