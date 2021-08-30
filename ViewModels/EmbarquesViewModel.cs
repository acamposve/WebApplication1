using DocManager.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplication1.Helper_Code.Objects;


namespace WebApplication1.ViewModels
{
    public class EmbarquesViewModel
    {
        public int EmbarqueId { get; set; }
        public string Referencia { get; set; }
        public DateTime Fechaarribo { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public int StatusId { get; set; }

        public string CantidadContainers { get; set; }
        public string Mercancia { get; set; }
        public List<ReceiptsStatus> ReceiptsStatus { get; set; }
        public List<Accounts> Accounts { get; set; }


        [Required]
        [Display(Name = "Choose Multiple Accounts")]
        public List<int> SelectedMultiAccountId { get; set; }

        /// <summary>  
        /// Gets or sets selected countries property.  
        /// </summary>  
        public List<AccountObj> SelectedAccountLst { get; set; }


        [Required(ErrorMessage = "Please select file.")]
        [Display(Name = "Browse File")]
        public HttpPostedFileBase[] files { get; set; }
    }
}