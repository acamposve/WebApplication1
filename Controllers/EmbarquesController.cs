using DocManager.Application.Logic;
using DocManager.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Helper_Code.Objects;
using WebApplication1.Helpers;

namespace WebApplication1.Controllers
{
    public class EmbarquesController : Controller
    {
        private readonly EmbarquesHelper eh = new EmbarquesHelper();
        private readonly ReceiptsLogic rlogic = new ReceiptsLogic();
        private readonly ReceiptStatusLogic rstatusLogic = new ReceiptStatusLogic();
        private readonly AccountsLogic accountsLogic = new AccountsLogic();
        // GET: Embarques
        public ActionResult Index()
        {
            return View(eh.ListaEmbarques());
        }


        public JsonResult GetReceipt(int? id)
        {
            return Json(rlogic.EmbarquexId(id.GetValueOrDefault()), JsonRequestBehavior.AllowGet);
        }



        public ActionResult LoadEditReceiptPopup(int EmbarqueId)
        {
            try
            {
                var statuses = rstatusLogic.ListaEmbarquesStatus();

                ViewBag.StatusId = new SelectList(statuses, "StatusId", "StatusDescription");
                return PartialView("_EditInfoReceipt", rlogic.EmbarquexId(EmbarqueId));
            }
            catch (Exception ex)
            {

                return PartialView("_EditInfoReceipt");
            }
        }


        [HttpPost]

        public JsonResult AddClient(EmbarquesAccountsScript embarques)
        {
            rlogic.AddClientReceipt(int.Parse(embarques.EmbarquesId), int.Parse(embarques.AccountId));
            return Json(new { redirectToUrl = Url.Action("Details/" + embarques.EmbarquesId, "Embarques") });
        }





        [HttpPost]

        public JsonResult DeleteClient(EmbarquesAccountsScript embarques)
        {
            rlogic.DeleteClientReceipt(int.Parse(embarques.EmbarquesId), int.Parse(embarques.AccountId));
            return Json(new { redirectToUrl = Url.Action("Details/" + embarques.EmbarquesId, "Embarques") });
        }



        [HttpPost]

        public JsonResult AddFiles(EmbarquesViewModel embarques)
        {


            if (Request.Files.Count > 0)
            {
                string newFileName = "";
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;



                    List<ReceiptFiles> lista = new List<ReceiptFiles>();

                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];





                        var fname = Path.GetFileName(file.FileName);
                        string extension = Path.GetExtension(fname);
                        newFileName = Guid.NewGuid() + extension;




                        // Get the complete folder path and store the file inside it.  
                        fname = Path.Combine(Server.MapPath("~/UploadedFiles/"), newFileName);
                        file.SaveAs(fname);

                        ReceiptFiles embarquesBD = new ReceiptFiles();
                        embarquesBD.EmbarqueId = embarques.EmbarqueId;
                        embarquesBD.Extension = extension;
                        embarquesBD.Name = newFileName;
                        embarquesBD.Path = fname;
                        embarquesBD.Size = file.ContentLength;
                        lista.Add(embarquesBD);
                    }

                    // Returns message that successfully uploaded  
                    rlogic.AddFiles(lista);
                    return Json(new { redirectToUrl = Url.Action("Details/" + embarques.EmbarqueId, "Embarques") });
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }





        }




        public ActionResult LoadaddFilesPopup(int EmbarqueId)
        {
            try
            {


                EmbarquesViewModel model = new EmbarquesViewModel
                {
                    EmbarqueId = EmbarqueId
                };

                return PartialView("_AddFileReceipt", model);
            }
            catch (Exception ex)
            {

                return PartialView("_EditInfoReceipt");
            }
        }
        public ActionResult LoadaddClientsPopup(int EmbarqueId)
        {
            try
            {

                EmbarquesViewModel model = new EmbarquesViewModel
                {
                    EmbarqueId = EmbarqueId
                };
                ViewBag.AccountsId = eh.GetAccountsList();
                return PartialView("_AddClientsReceipt", model);
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
            ViewBag.Cuentas = accountsLogic.CuentasxEmbarques(id.GetValueOrDefault());



            ViewBag.CuentasDisponibles = accountsLogic.CuentasDisponibles(id.GetValueOrDefault());
            ViewBag.Archivos = rlogic.ArchivosxEmbarque(id.GetValueOrDefault());
            return View(rlogic.EmbarquexId(id.GetValueOrDefault()));
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




        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult Delete(int id)
        {
            var embarque = rlogic.Archivo(id);
            rlogic.DeleteFile(embarque.Path, embarque.FileId);
            return Json(new { redirectToUrl = Url.Action("Details/" + embarque.EmbarqueId, "Embarques") });
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
