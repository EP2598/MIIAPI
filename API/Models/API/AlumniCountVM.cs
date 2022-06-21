using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.API
{
    public class AlumniCountVM
    {
        public List<string> UniversityName { get; set; }
        public List<int> UniversityCount { get; set; }
    }
}
