using DocManager.Application.Logic;
using DocManager.Core;
using System.IO;
using System.Web.Mvc;
using System.Web.Security;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {

        private AccountsLogic accountsLogic = new AccountsLogic();
        private readonly ReceiptsLogic rlogic = new ReceiptsLogic();




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
            var datos = rl.EmbarquesxCuenta(Session["usuario"].ToString());


            return View(datos);


        }

        public ActionResult DownloadFile(string name)
        {

            string fullName = Path.Combine(Server.MapPath("~/UploadedFiles/" + name));

            //WebClient webClient = new WebClient();
            //webClient.DownloadFile(ruta + "tablas_payout.jpg", @"C:\imagenesembarques\tablas_payout.jpg");
            //return RedirectToAction("Index");





            byte[] fileBytes = GetFile(fullName);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, name);
        }



        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;

        }

        public ActionResult DetallesEmbarques(int id)
        {
            var datos = rlogic.EmbarquexId(id);
            //ViewBag.Cuentas = accountsLogic.CuentasxEmbarques(id.GetValueOrDefault());



            //ViewBag.CuentasDisponibles = accountsLogic.CuentasDisponibles(id.GetValueOrDefault());
            ViewBag.Imagenes = rlogic.ImagenesxEmbarque(id);
            ViewBag.Documentos = rlogic.DocumentosxEmbarque(id);

            return View(datos);
        }
    }
}