using Client.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Base
{
    public class BaseController<TEntity, TRepository, TId> : Controller
        where TEntity : class
        where TRepository : IRepository<TEntity, TId>
    {
        private readonly TRepository _repos;

        public BaseController(TRepository repos)
        {
            this._repos = repos;
        }

        [HttpGet]
        public async Task<JsonResult> GetAll()
        {
            var res = await _repos.Get();
            return Json(res);
        }

        [HttpGet]
        public async Task<JsonResult> Get(TId id)
        {
            var res = await _repos.Get(id);
            return Json(res);
        }

        [HttpPost]
        public JsonResult Post(TEntity ent)
        {
            var res = _repos.Post(ent);
            return Json(res);
        }

        [HttpPut]
        public JsonResult Put(TEntity ent, TId id)
        {
            var res = _repos.Put(id, ent);
            return Json(res);
        }

        [HttpDelete]
        public JsonResult Delete(TId id)
        {
            var res = _repos.Delete(id);
            return Json(res);
        }
    }
}
