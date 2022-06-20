using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.API
{
    public class UpdateEducationVM
    {
        public string NIK { get; set; }
        public int UniversityId { get; set; }
        public Degree Degree { get; set; }
        public string GPA { get; set; }
    }
}
