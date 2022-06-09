using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public partial class Roles
    {
        [Key]
        public int RolesId { get; set; }
        public string RoleName { get; set; }

        //public virtual ICollection<AccountRoles> AccountRoles { get; set; }

        #region LazyLoad
        private Lazy<List<AccountRoles>> _accountroles;

        public Roles(int RolesId = 0)
        {
            this.RolesId = RolesId;
            _accountroles = new Lazy<List<AccountRoles>>(() =>
            {
                return new List<AccountRoles>();
            });
        }

        public List<AccountRoles> AccountRoles
        {
            get
            {
                return _accountroles.Value;
            }
            set { }
        } 
        #endregion
    }
}
