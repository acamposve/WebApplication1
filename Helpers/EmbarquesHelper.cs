using DocManager.Application.Logic;
using DocManager.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Helper_Code.Objects;

namespace WebApplication1.Helpers
{
    public class EmbarquesHelper
    {

        private ReceiptsLogic rlogic = new ReceiptsLogic();
        private AccountsLogic accountsLogic = new AccountsLogic();
        public List<Embarques> ListaEmbarques()
        {
            return rlogic.ListaEmbarques(); 
        }






        public IEnumerable<SelectListItem> GetAccountsList()
        {
            // Initialization.  
            SelectList lstobj = null;

            try
            {
                // Loading.  
                var list = LoadData()
                                  .Select(p =>
                                            new SelectListItem
                                            {
                                                Value = p.Id.ToString(),
                                                Text = p.Email
                                            });

                // Setting.  
                lstobj = new SelectList(list, "Value", "Text");
            }
            catch (Exception ex)
            {
                // Info  
                throw ex;
            }

            // info.  
            return lstobj;
        }

        public List<AccountObj> LoadData()
        {
            // Initialization.  
            List<AccountObj> lst = new List<AccountObj>();

            try
            {

                var accounts = accountsLogic.ListaCuentas();





                foreach (var item in accounts)
                {

                    AccountObj account = new AccountObj();


                    account.Id = item.Id;
                    account.Email = item.Email;


                    lst.Add(account);

                }



            }
            catch (Exception ex)
            {
                // info.  
                Console.Write(ex);
            }

            // info.  
            return lst;
        }
    }
}