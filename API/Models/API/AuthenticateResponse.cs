using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.API
{
    public class AuthenticateResponse
    {
        public string NIK { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }

        public AuthenticateResponse(Employee emp, string token)
        {
            NIK = emp.NIK;
            Email = emp.Email;
            Token = token;
        }
    }
}
