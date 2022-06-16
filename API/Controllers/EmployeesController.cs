using API.Base;
using API.Context;
using API.Models;
using API.Models.API;
using API.Repository;
using API.Repository.Data;
using API.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : BaseController<Employee, EmployeeRepository, string>
    {
        private readonly EmployeeRepository _empRepos;

        public EmployeesController(EmployeeRepository empRepos) : base(empRepos)
        {
            this._empRepos = empRepos;
        }

        [HttpPost]
        [Route("Register")]
        [EnableCors("AllowOrigin")]
        public ActionResult Register(RegisterVM obj)
        {
            var codeRes = _empRepos.Register(obj);

            return StatusCode(codeRes, new
            {
                status = codeRes == 200 ? HttpStatusCode.OK : HttpStatusCode.BadRequest,
                message = codeRes == 200 ? "Insert berhasil." :
                codeRes == 401 ? BaseException.DataDuplicate_Phone :
                codeRes == 402 ? BaseException.DataDuplicate_Email :
                codeRes == 403 ? BaseException.DataDuplicate_Email + " " + BaseException.DataDuplicate_Phone :
                codeRes == 404 ? "Failed to change initial password" : "Insert gagal"
            });
        }

        [HttpPost]
        [Route("Update")]
        public ActionResult UpdateEmp(Employee emp)
        {
            return _empRepos.Update(emp) > 0 ? StatusCode(200, new { status = HttpStatusCode.OK, message = "Update berhasil." }) :
                StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Update gagal" });
        }

        [HttpPost]
        [Route("Delete")]
        public ActionResult DeleteEmp(string NIK)
        {
            return _empRepos.Delete(NIK) > 0 ? StatusCode(200, new { status = HttpStatusCode.OK, message = "Delete berhasil." }) :
                StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Delete gagal" });
        }

        [Authorize(Roles = "Director,Manager")]
        [HttpGet]
        [Route("GetRegisteredData")]
        public ActionResult GetRegisteredData()
        {
            var codeRes = _empRepos.GetRegisteredData();
            return StatusCode(
                codeRes.Count > 0 ? 200 : 400,
                new
                {
                    status = codeRes.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.BadRequest,
                    message = codeRes.Count > 0 ? "Sukses." : "Tidak ada data.",
                    data = codeRes
                }) ;
        }

        [HttpGet("TestCors")]
        [EnableCors("AllowOrigin")]
        public ActionResult TestCORS()
        {
            return Ok("Test CORS berhasil.");
        }

        [HttpGet("GetEmployeeData")]
        [EnableCors("AllowOrigin")]
        public ActionResult GetEmployeeData()
        {
            var codeRes = _empRepos.GetEmployeeData();
            return StatusCode(
                codeRes.Count > 0 ? 200 : 400,
                new
                {
                    status = codeRes.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.BadRequest,
                    message = codeRes.Count > 0 ? "Sukses." : "Tidak ada data.",
                    data = codeRes
                });
        }

        [HttpPost("GetEmployeeDetail")]
        [EnableCors("AllowOrigin")]
        public ActionResult GetEmployeeDetail(GetEmployeeParameterVM obj)
        {
            GetEmployeeResponseVM objResponse = _empRepos.GetEmployeeByNIK(obj.NIK);

            return Ok(objResponse);
        }
    }
}
