using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public partial class Roles
    {
        public Roles()
        {
            AccountRoles = new HashSet<AccountRoles>();
        }
        [Key]
        public int RolesId { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<AccountRoles> AccountRoles { get; set; }

    }
}
