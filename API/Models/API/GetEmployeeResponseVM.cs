using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.API
{
    public class GetEmployeeResponseVM
    {
        public GetEmployeeResponseVM()
        {
            Roles = new HashSet<Roles>();
            Educations = new HashSet<Education>();
            Universities = new HashSet<University>();
        }
        public virtual Employee Employee { get; set; }
        public virtual Account Account { get; set; }
        public virtual ICollection<Roles> Roles { get; set; }
        public virtual ICollection<Education> Educations { get; set; }
        public virtual ICollection<University> Universities { get; set; }
    }
}
