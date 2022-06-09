using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public partial class Profiling
    {
        [Key]
        public string NIK { get; set; }
        [Required]
        public int Education_Id { get; set; }
        //public virtual Education Education { get; set; }
        //public virtual Account Account { get; set; }

        #region LazyLoad
        private Lazy<Account> _account;
        private Lazy<Education> _education;

        public Profiling(string NIK = "", int Education_Id = 0)
        {
            this.NIK = NIK;
            this.Education_Id = Education_Id;

            _account = new Lazy<Account>(() =>
            {
                return new Account(this.NIK);
            });
            _education = new Lazy<Education>(() =>
            {
                return new Education();
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

        public Education Education
        {
            get
            {
                return _education.Value;
            }
            set { }
        } 
        #endregion

    }
}
