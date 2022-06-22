using API.Models;
using API.Models.API;
using Client.Base;
using Client.Repositories.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Controllers
{
    public class LoginController : BaseController<Account, AccountRepository, string>
    {
        private readonly AccountRepository _accRepos;
        public LoginController(AccountRepository accRepos) : base(accRepos)
        {
            this._accRepos = accRepos;
        }

        [HttpPost]
        public async Task<JsonResult> Auth(LoginVM objReq)
        {
            var jwtToken = await _accRepos.Auth(objReq);
            var token = jwtToken.data;

            HttpContext.Session.SetString("JWToken", token.ToString());

            return Json(jwtToken);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
