using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [JsonObject(IsReference =true)]
    public partial class Account
    {

        public Account()
        {
            AccountRoles = new HashSet<AccountRoles>();
        }

        [Key]
        public string NIK { get; set; }
        [Required]
        public string Password { get; set; }
        public string OTP { get; set; }
        public bool IsOTPActive { get; set; }
        public DateTime ExpiredTime { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Profiling Profiling { get; set; }
        public virtual ICollection<AccountRoles> AccountRoles { get; set; }

    }
}
