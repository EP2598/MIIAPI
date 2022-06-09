using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public partial class Education
    {
        [Key]
        public int Id { get; set; }
        public Degree Degree { get; set; }
        [Required]
        public string GPA { get; set; }
        [Required]
        public int University_Id { get; set; }
        //public virtual University University { get; set; }
        //public virtual ICollection<Profiling> Profilings { get; set; }

        #region LazyLoad
        private Lazy<List<Profiling>> _profiling;
        private Lazy<University> _university;

        public Education(int Id = 0)
        {
            this.Id = Id;

            _profiling = new Lazy<List<Profiling>>(() =>
            {
                return new List<Profiling>();
            });
            _university = new Lazy<University>(() =>
            {
                return new University(this.University_Id);
            });
        }

        public List<Profiling> Profiling
        {
            get
            {
                return _profiling.Value;
            }
            set { }
        }

        public University University
        {
            get
            {
                return _university.Value;
            }
            set { }
        } 
        #endregion

    }
    public enum Degree
    {
        D3,
        D4,
        S1,
        S2,
        S3
    }
}
