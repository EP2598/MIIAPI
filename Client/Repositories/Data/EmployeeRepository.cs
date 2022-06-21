using API.Models;
using API.Models.API;
using Client.Base;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client.Repositories.Data
{
    public class EmployeeRepository : Repository<Employee, string>
    {
        private readonly Address _address;
        private readonly HttpClient httpClient;
        private readonly string request;
        private readonly IHttpContextAccessor _contextAccessor;

        public EmployeeRepository(Address address, string request = "Employees/") : base(address, request)
        {
            this._address = address;
            this.request = request;
            _contextAccessor = new HttpContextAccessor();
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(_address.link)
            };
        }

        public async Task<ResponseObj> GetDataByNIK(GetEmployeeParameterVM obj)
        {
            ResponseObj objResp = new ResponseObj();

            StringContent content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

            var resp = httpClient.PostAsync(request + "GetEmployeeDetail/", content).Result;

            string apiResp = await resp.Content.ReadAsStringAsync();
            if (!String.IsNullOrEmpty(apiResp))
            {
                objResp.statusCode = 200;
                objResp.message = "Success Get Personal Data";
                objResp.data = JsonConvert.DeserializeObject(apiResp);
            }

            return objResp;
        }

        public async Task<ResponseObj> GetRegistered()
        {
            ResponseObj objResp = new ResponseObj();

            using (var resp = await httpClient.GetAsync(request + "GetEmployeeData/"))
            {
                string apiResp = await resp.Content.ReadAsStringAsync();
                if (!String.IsNullOrEmpty(apiResp))
                {
                    objResp.statusCode = 200;
                    objResp.message = "Success Get Registered Data";
                    objResp.data = JsonConvert.DeserializeObject(apiResp);
                }
            }

            return objResp;
        }

    }
}
