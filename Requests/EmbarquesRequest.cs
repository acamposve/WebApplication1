using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Requests
{
    public class EmbarquesRequest
    {
        public string Referencia { get; set; }
        public DateTime Fechaarribo { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public int Status { get; set; }
        public List<string> AccountsId { get; set; }
    }
}