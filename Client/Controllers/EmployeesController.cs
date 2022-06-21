using API.Models;
using API.Models.API;
using Client.Base;
using Client.Repositories.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Controllers
{
    public class EmployeesController : BaseController<Employee, EmployeeRepository, string>
    {
        private readonly EmployeeRepository _repos;
        public EmployeesController(EmployeeRepository repos) : base(repos)
        {
            this._repos = repos;
        }

        [HttpGet]
        public async Task<JsonResult> GetRegistered()
        {
            var res = await _repos.GetRegistered();
            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> GetDataByNIK(GetEmployeeParameterVM obj)
        {
            var res = await _repos.GetDataByNIK(obj);
            return Json(res);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
