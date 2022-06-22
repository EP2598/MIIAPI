using API.Base;
using API.Models;
using API.Models.API;
using API.Repository.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseController<Account, AccountRepository, string>
    {
        private readonly AccountRepository _accRepos;
        public AccountsController(AccountRepository accRepos) : base(accRepos)
        {
            this._accRepos = accRepos;
        }

        [HttpPost]
        [Route("Login")]
        public ActionResult Login(LoginVM obj)
        {
            string Token;
            int loginRes = _accRepos.Login(obj, out Token);
            return StatusCode(loginRes, new
            {
                statusCode =
                    loginRes == 200 ? Convert.ToInt32(HttpStatusCode.OK) : Convert.ToInt32(HttpStatusCode.BadRequest),
                message =
                    loginRes == 200 ? "Login berhasil" :
                    loginRes == 401 ? "Email tidak ditemukan." :
                    loginRes == 402 ? "Password salah." :
                    "Login gagal.",
                data = Token == null ? "No Token" : Token
                
            });
        }

        [HttpPost]
        [Route("ForgetPassword")]
        [EnableCors("AllowOrigin")]
        public ActionResult ForgetPassword(ForgetPasswordVM obj)
        {
            int actionRes = _accRepos.ForgetPassword(obj.Email);
            return StatusCode(actionRes, new
            {
                status =
                    actionRes == 200 ? HttpStatusCode.OK : HttpStatusCode.BadRequest,
                message =
                    actionRes == 200 ? "OTP berhasil dikirimkan. Periksa email." :
                    actionRes == 401 ? "Email tidak ditemukan." :
                    "OTP gagal dikirimkan."
            });
        }

        [HttpPost]
        [Route("ChangePassword")]
        public ActionResult ChangePassword(ChangePasswordVM obj)
        {
            int actionRes = _accRepos.ChangePassword(obj);
            return StatusCode(actionRes,
                new
                {
                    status =
                        actionRes == 200 ? HttpStatusCode.OK : HttpStatusCode.BadRequest,
                    message =
                        actionRes == 401 ? "Email tidak ditemukan." :
                        actionRes == 402 ? "Akun tidak sedang melakukan OTP Request." :
                        actionRes == 403 ? "OTP yang dimasukkan salah." :
                        actionRes == 404 ? "OTP yang dimasukkan sudah expired." :
                        actionRes == 405 ? "Password yang dimasukkan tidak cocok." :
                        "Password berhasil diubah."
                });
        }

        [Authorize(Roles = BaseConstanta.Admin)]
        [HttpPost]
        [Route("SignManager")]
        public ActionResult SignManager(SignManagerVM obj)
        {
            if (User.IsInRole(BaseConstanta.Admin))
            {
                int actionRes = _accRepos.SignManager(obj.NIK);

                return StatusCode(actionRes,
                    new
                    {
                        status =
                            actionRes == 200 ? HttpStatusCode.OK : HttpStatusCode.BadRequest,
                        message =
                            actionRes == 200 ? "Berhasil assign Employee terkait ke Manager." : "Gagal melakukan assign Employee terkait ke Manager."
                    });
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
