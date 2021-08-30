using DocManager.Application.Data.UnitOfWork;
using DocManager.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace WebApplication1.Controllers
{
    public class ReceiptsStatusController : Controller
    {
        private UnitOfWork uow = new UnitOfWork(ConfigurationManager.ConnectionStrings["DapperCon"].ConnectionString);

        // GET: ReceiptsStatus
        public ActionResult Index()
        {
            return View(uow.ReceiptStatusRepository.All());
        }

        // GET: ReceiptsStatus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReceiptsStatus receiptsStatus = uow.ReceiptStatusRepository.Find(id.GetValueOrDefault());
            if (receiptsStatus == null)
            {
                return HttpNotFound();
            }
            return View(receiptsStatus);
        }

        // GET: ReceiptsStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReceiptsStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StatusId,StatusDescription")] ReceiptsStatus receiptsStatus)
        {
            if (ModelState.IsValid)
            {
                uow.ReceiptStatusRepository.Add(receiptsStatus);
                uow.Commit();
                return RedirectToAction("Index");
            }

            return View(receiptsStatus);
        }

        // GET: ReceiptsStatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReceiptsStatus receiptsStatus = uow.ReceiptStatusRepository.Find(id.GetValueOrDefault());
            if (receiptsStatus == null)
            {
                return HttpNotFound();
            }
            return View(receiptsStatus);
        }

        // POST: ReceiptsStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StatusId,StatusDescription")] ReceiptsStatus receiptsStatus)
        {
            if (ModelState.IsValid)
            {
                uow.ReceiptStatusRepository.Update(receiptsStatus);
                uow.Commit();
                return RedirectToAction("Index");
            }
            return View(receiptsStatus);
        }

        // GET: ReceiptsStatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReceiptsStatus receiptsStatus = uow.ReceiptStatusRepository.Find(id.GetValueOrDefault());
            if (receiptsStatus == null)
            {
                return HttpNotFound();
            }
            return View(receiptsStatus);
        }

        // POST: ReceiptsStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ReceiptsStatus receiptsStatus = uow.ReceiptStatusRepository.Find(id);
            uow.ReceiptStatusRepository.Delete(receiptsStatus);

            uow.Commit();
            return RedirectToAction("Index");
        }


    }
}
