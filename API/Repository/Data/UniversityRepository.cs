using API.Base;
using API.Context;
using API.Models;
using API.Models.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class UniversityRepository : Repository<MyContext, University, int>
    {
        private readonly MyContext _context;
        public UniversityRepository(MyContext context) : base(context)
        {
            this._context = context;
        }
        public University GetUniversityByName(string univName)
        {
            return _context.Universities.Where(x => x.Name == univName).FirstOrDefault();
        }

        public University GetUniversityById(int univId)
        {
            return _context.Universities.Where(x => x.Id == univId).FirstOrDefault();
        }

        public ResponseObj GetAlumniCount()
        {
            ResponseObj objRes = new ResponseObj();

            AlumniCountVM obj = new AlumniCountVM();
            List<string> uniName = new List<string>();
            List<int> uniCount = new List<int>();

            try
            {
                var uniRes = (from e in _context.Employees
                              from p in _context.Profilings
                              .Where(p => p.NIK == e.NIK)
                              from edu in _context.Educations
                              .Where(edu => edu.Id == p.Education_Id)
                              from uni in _context.Universities
                              .Where(uni => uni.Id == edu.University_Id)
                              where e.IsDeleted == 0
                              group new { e, p, edu, uni } by uni.Name into grp
                              select new
                              {
                                  UniversityName = grp.Key,
                                  UniversityCount = grp.Select(x => x.edu).Distinct().Count()
                              });

                foreach (var item in uniRes)
                {
                    uniName.Add(item.UniversityName);
                    uniCount.Add(item.UniversityCount);
                }

                obj.UniversityName = uniName;
                obj.UniversityCount = uniCount;

                objRes.statusCode = Convert.ToInt32(HttpStatusCode.OK);
                objRes.message = BaseException.SuccessRequest;
                objRes.data = obj;
            }
            catch (Exception ex)
            {
                objRes.statusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                objRes.message = BaseException.FailedRequest;
                objRes.data = ex;
            }
            

            return objRes;
        }
    }
}
