﻿using DocManager.Application.Logic;
using DocManager.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Mvc;
using WebApplication1.Helper_Code.Objects;
using WebApplication1.Helpers;
using WebApplication1.Mappings;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class EmbarquesController : Controller
    {
        private EmbarquesHelper eh = new EmbarquesHelper();
        private ReceiptsLogic rlogic = new ReceiptsLogic();
        private ReceiptStatusLogic rstatusLogic = new ReceiptStatusLogic();
        private AccountsLogic accountsLogic = new AccountsLogic();
        // GET: Embarques
        public ActionResult Index()
        {
            return View(eh.ListaEmbarques());
        }


        public JsonResult GetReceipt(int? id)
        {
            var request = rlogic.EmbarquexId(id.GetValueOrDefault());
            var mapperRequest = EmbarquesProfile.InitializeAutomapper();
            var response = mapperRequest.Map<Models.Embarques>(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }



        public ActionResult LoadEditReceiptPopup(int EmbarqueId)
        {
            try
            {
                var request = rlogic.EmbarquexId(EmbarqueId);
                var mapperRequest = EmbarquesProfile.InitializeAutomapper();
                var response = mapperRequest.Map<Models.Embarques>(request);

                var statuses = rstatusLogic.ListaEmbarquesStatus();

                ViewBag.StatusId = new SelectList(statuses, "StatusId", "StatusDescription");
                return PartialView("_EditInfoReceipt", response);
            }
            catch (Exception ex)
            {

                return PartialView("_EditInfoReceipt");
            }
        }




        //GET: Embarques/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }






            var request = rlogic.EmbarquexId(id.GetValueOrDefault());
            var mapperRequest = EmbarquesProfile.InitializeAutomapper();
            var response = mapperRequest.Map<Models.Embarques>(request);






            ViewBag.Cuentas = accountsLogic.CuentasxEmbarques(id.GetValueOrDefault());

            ViewBag.Archivos = rlogic.ArchivosxEmbarque(id.GetValueOrDefault());

            if (response == null)
            {
                return HttpNotFound();
            }
            return View(response);
        }

        // GET: Embarques/Create
        public ActionResult Create()
        {
            InitCreate();

            return View();
        }

        private void InitCreate()
        {
            // Initialization.  
            EmbarquesViewModel model = new EmbarquesViewModel
            {
                SelectedMultiAccountId = new List<int>(),
                SelectedAccountLst = new List<AccountObj>()
            };



            var statuses = rstatusLogic.ListaEmbarquesStatus();

            ViewBag.StatusId = new SelectList(statuses, "StatusId", "StatusDescription");

            ViewBag.AccountsId = eh.GetAccountsList();
        }



        /// <summary>  
        /// Get country method.  
        /// </summary>  
        /// <returns>Return country for drop down list.</returns>  






        // POST: Embarques/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmbarquesViewModel embarques, FormCollection formval)
        {






            var cuentas = Request.Form["SelectedMultiAccountId"].Split(',');

            EmbarquesDTO embarquesBD = new EmbarquesDTO();
            embarquesBD.Destino = embarques.Destino;
            embarquesBD.Fechaarribo = embarques.Fechaarribo;
            embarquesBD.Referencia = embarques.Referencia;
            embarquesBD.Origen = embarques.Origen;
            embarquesBD.CantidadContainers = embarques.CantidadContainers;
            embarquesBD.Mercancia = embarques.Mercancia;
            embarquesBD.StatusId = embarques.StatusId;

            embarquesBD.files = embarques.files;


            string ruta = Path.Combine(Server.MapPath("~/UploadedFiles/"));
            rlogic.InsertReceipts(embarquesBD, ruta, cuentas);




            return RedirectToAction("Index");
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
        // GET: Embarques/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Embarques embarques = db.Embarques.Find(id);
        //    if (embarques == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.StatusId = new SelectList(db.ReceiptsStatus, "StatusId", "StatusDescription", embarques.StatusId);
        //    return View(embarques);
        //}

        // POST: Embarques/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult Edit(Embarques embarques)
        {

            rlogic.UpdateReceipts(embarques);


            return Json(new { redirectToUrl = Url.Action("Details/" + embarques.EmbarqueId, "Embarques") });



            // ViewBag.StatusId = new SelectList(db.ReceiptsStatus, "StatusId", "StatusDescription", embarques.StatusId);


        }

        //// GET: Embarques/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Embarques embarques = db.Embarques.Find(id);
        //    if (embarques == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(embarques);
        //}

        //// POST: Embarques/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Embarques embarques = db.Embarques.Find(id);
        //    db.Embarques.Remove(embarques);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}


    }
}