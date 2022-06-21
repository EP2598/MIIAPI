using API.Base;
using API.Models;
using API.Models.API;
using API.Repository.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UniversitiesController : BaseController<University, UniversityRepository, int>
    {
        private readonly UniversityRepository _univRepos;
        public UniversitiesController(UniversityRepository univRepos) : base(univRepos)
        {
            this._univRepos = univRepos;
        }

        [HttpGet("GetAlumniCount")]
        [EnableCors("AllowOrigin")]
        public ActionResult GetAlumniCount() {
            ResponseObj objRes = _univRepos.GetAlumniCount();

            return Ok(objRes);
        }
    }
}
