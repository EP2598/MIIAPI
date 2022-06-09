using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public partial class AccountRoles
    {
        //[Key]
        //public int AccountRolesId { get; set; }
        public string NIK { get; set; }
        public int RolesId { get; set; }
        //public virtual Account Account { get; set; }
        //public virtual Roles Roles { get; set; }

        #region LazyLoad
        private Lazy<Account> _account;
        private Lazy<Roles> _roles;

        public AccountRoles(string NIK = "", int RolesId = 0)
        {
            this.NIK = NIK;
            this.RolesId = RolesId;
            _account = new Lazy<Account>(() =>
            {
                return new Account(this.NIK);
            });
            _roles = new Lazy<Roles>(() =>
            {
                return new Roles(this.RolesId);
            });
        }

        public Account Account
        {
            get
            {
                return _account.Value;
            }
            set { }
        }

        public Roles Roles
        {
            get
            {
                return _roles.Value;
            }
            set { }
        } 
        #endregion

    }
}
