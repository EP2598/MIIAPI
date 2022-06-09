using API.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Base
{
    public class BaseController<Entity, Repository, Key> : ControllerBase
        where Entity : class
        where Repository : IRepository<Entity, Key>
    {
        private readonly Repository repos;
        public BaseController(Repository repository)
        {
            this.repos = repository;
        }

        [HttpGet]
        [Route("Get")]
        public ActionResult<Entity> Get()
        {
            var enumObj = repos.Get();

            if (enumObj.Count() > 0) return Ok(enumObj);
            else return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Tidak ada data." });
        }

        [HttpGet]
        [Route("GetById/{key}")]
        public ActionResult<Entity> GetById(Key key)
        {
            var enumObj = repos.Get(key);
            var options = new JsonSerializerOptions { WriteIndented = true };
            return StatusCode(enumObj == null ? 200 : 400, new
            {
                status = enumObj != null ? HttpStatusCode.OK : HttpStatusCode.BadRequest,
                message = enumObj != null ? JsonSerializer.Serialize(enumObj) : "Tidak ada data."
            });
        }

        [HttpPost]
        [Route("Insert")]
        public ActionResult<Entity> Insert(Entity ent)
        {
            var codeRes = repos.Insert(ent);
            return StatusCode(codeRes, new
            {
                status = codeRes == 200 ? HttpStatusCode.OK : HttpStatusCode.BadRequest,
                message = codeRes == 200 ? "Insert berhasil." : "Insert gagal."
            });
        }

        [HttpPost]
        [Route("Update")]
        public ActionResult<Entity> Update(Entity ent)
        {
            var codeRes = repos.Update(ent);
            return StatusCode(codeRes, new
            {
                status = codeRes == 200 ? HttpStatusCode.OK : HttpStatusCode.BadRequest,
                message = codeRes == 200 ? "Update berhasil." : "Update gagal."
            });
        }

        [HttpPost]
        [Route("Delete")]
        public ActionResult<Entity> Delete(Key key)
        {
            var codeRes = repos.Delete(key);
            return StatusCode(codeRes, new
            {
                status = codeRes == 200 ? HttpStatusCode.OK : HttpStatusCode.BadRequest,
                message = codeRes == 200 ? "Delete berhasil." : "Delete gagal."
            });
        }
    }
}
