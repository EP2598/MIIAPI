using API.Context;
using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
