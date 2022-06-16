using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [JsonObject(IsReference = true)]
    public partial class AccountRoles
    {
        //[Key]
        //public int AccountRolesId { get; set; }
        public string NIK { get; set; }
        public int RolesId { get; set; }
        public virtual Account Account { get; set; }
        public virtual Roles Roles { get; set; }

    }
}
