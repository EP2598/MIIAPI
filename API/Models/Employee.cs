using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public partial class Employee
    {
        [Key]
        public string NIK { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public decimal Salary { get; set; }
        public bool IsDeleted { get; set; }
        public Gender Gender { get; set; }
        //public virtual Account Account { get; set; }
        #region LazyLoad
        private Lazy<Account> _account;

        public Employee(string NIK)
        {
            this.NIK = NIK;
            _account = new Lazy<Account>(() =>
            {
                return new Account(this.NIK);
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
        #endregion

    }
    public enum Gender
    {
        Male,
        Female
    }
}
