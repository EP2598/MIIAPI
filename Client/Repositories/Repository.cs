using Client.Base;
using Client.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client.Repositories
{
    public class Repository<TEntity, TId> : IRepository<TEntity, TId>
        where TEntity : class
    {
        private readonly Address address;
        private string request;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpClient httpClient;

        public Repository(Address address, string request)
        {
            this.address = address;
            this.request = request;
            _contextAccessor = new HttpContextAccessor();
            httpClient = new HttpClient()
            {
                BaseAddress = new Uri(address.link)
            };
        }

        public HttpStatusCode Delete(TId id)
        {
            var res = httpClient.DeleteAsync(request + id).Result;
            return res.StatusCode;
        }

        public async Task<List<TEntity>> Get()
        {
            //request += "Get/";
            List<TEntity> ent = new List<TEntity>();

            using (var resp = await httpClient.GetAsync(request))
            {
                string apiResp = await resp.Content.ReadAsStringAsync();
                ent = JsonConvert.DeserializeObject<List<TEntity>>(apiResp);
            }
            return ent;
        }

        public async Task<TEntity> Get(TId id)
        {
            TEntity ent = null;

            using (var resp = await httpClient.GetAsync(request + id))
            {
                string apiResp = await resp.Content.ReadAsStringAsync();
                ent = JsonConvert.DeserializeObject<TEntity>(apiResp);
            }
            //Newtonsoft.Json versi : 3.0
            return ent;
        }

        public HttpStatusCode Put(TId id, TEntity entity)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            var res = httpClient.PutAsync(request + id, content).Result;
            return res.StatusCode;
        }

        public HttpStatusCode Post(TEntity entity)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            var res = httpClient.PostAsync(address.link + request, content).Result;
            return res.StatusCode;
        }
    }
}
