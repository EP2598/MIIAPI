using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public partial class Account
    {
        [Key]
        public string NIK { get; set; }
        [Required]
        public string Password { get; set; }
        public string OTP { get; set; }
        public bool IsOTPActive { get; set; }
        public DateTime ExpiredTime { get; set; }
        //public virtual Employee Employee { get; set; }
        //public virtual Profiling Profiling { get; set; }
        //public virtual ICollection<AccountRoles> AccountRoles { get; set; }

        #region LazyLoad
        private Lazy<Employee> _employee;
        private Lazy<Profiling> _profiling;
        private Lazy<List<AccountRoles>> _accountroles;

        public Account(string NIK = "")
        {
            this.NIK = NIK;
            _employee = new Lazy<Employee>(() =>
            {
                return new Employee(this.NIK);
            });
            _profiling = new Lazy<Profiling>(() =>
            {
                return new Profiling(this.NIK);
            });
            _accountroles = new Lazy<List<AccountRoles>>(() =>
            {
                return new List<AccountRoles>();
            });
        }

        public Employee Employee
        {
            get
            {
                return _employee.Value;
            }
            set
            {
                
            }
        }

        public Profiling Profiling
        {
            get
            {
                return _profiling.Value;
            }
            set { }
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
