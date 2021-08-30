using DocManager.Application.Data.UnitOfWork;
using DocManager.Application.Logic;
using DocManager.Core;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Web.Mvc;
using WebApplication1.Mappings;
using WebApplication1.Requests;

namespace WebApplication1.Controllers
{
    public class AccountsController : Controller
    {
        private readonly UnitOfWork uow = new UnitOfWork(ConfigurationManager.ConnectionStrings["DapperCon"].ConnectionString);

        private AccountsLogic logic = new AccountsLogic();
        private RolesLogic rlogic = new RolesLogic();

        // GET: Accounts
        public ActionResult Index()
        {
            var request = logic.ListaCuentas();
            var mapperRequest = CuentasRolesProfile.InitializeAutomapper();
            var response = mapperRequest.Map<List<Models.AccountWithRole>>(request);
            return View(response);
        }

        // GET: Accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var accounts = logic.CuentaxId(id);
            if (accounts == null)
            {
                return HttpNotFound();
            }
            return View(accounts);
        }

        // GET: Accounts/Create
        public ActionResult Create()
        {

            var request = rlogic.ListaRoles();
            var mapperRequest = RolesProfile.InitializeAutomapper();
            var response = mapperRequest.Map<List<Models.Roles>>(request);


            ViewBag.Roles = new SelectList(response, "Id", "RoleName");
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DocManager.Core.AccountCreateRequest accounts)
        {
            if (ModelState.IsValid)
            {
                logic.InsertaCuenta(accounts);

                return RedirectToAction("Index");
            }

            return View(accounts);
        }

        // GET: Accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var accounts = uow.AccountRepository.Find(id.GetValueOrDefault());



            Requests.AccountCreateRequest response = new Requests.AccountCreateRequest();
            response.Email = accounts.Email;
            response.FirstName = accounts.FirstName;
            response.Id = accounts.Id;
            response.LastName = accounts.LastName;
            response.Password = accounts.Password;

            if (response == null)
            {
                return HttpNotFound();
            }
            var request = rlogic.ListaRoles();
            var mapperRequestRoles = RolesProfile.InitializeAutomapper();
            var responseRoles = mapperRequestRoles.Map<List<Models.Roles>>(request);
            ViewBag.Roles = new SelectList(responseRoles, "Id", "RoleName");
            return View(response);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DocManager.Core.AccountCreateRequest accounts)
        {
            if (ModelState.IsValid)
            {

                logic.ActualizaCuenta(accounts);

                return RedirectToAction("Index");
            }
            return View(accounts);
        }

        // GET: Accounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var frombd = logic.CuentaxId(id);


            Models.Accounts accounts = new Models.Accounts();
            accounts.Id = frombd.Id;
            accounts.FirstName = frombd.FirstName;

            if (accounts == null)
            {
                return HttpNotFound();
            }
            return View(accounts);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            logic.EliminarCuenta(id);


            return RedirectToAction("Index");
        }


    }
}
