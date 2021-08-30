using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Requests
{
    public class AccountCreateRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Roles { get; set; }
        public int Id { get; set; }
    }
}