using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class EmbarquesCuentas
    {
        public string Referencia { get; set; }
        public DateTime FechaArribo { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public string CantidadContainers { get; set; }
        public string Mercancia { get; set; }
        public string StatusDescripcion { get; set; }
    }
}