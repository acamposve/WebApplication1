using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Embarques
    {
        public int EmbarqueId { get; set; }
        public string Referencia { get; set; }
        public DateTime FechaArribo { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public string StatusDescription { get; set; }
        public int StatusId { get; set; }
        public string CantidadContainers { get; set; }
        public string Mercancia { get; set; }
    }
}