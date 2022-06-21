using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.API
{
    public class GenderCountVM
    {
        public List<string> Gender { get; set; }
        public List<int> GenderCount { get; set; }
    }
}
