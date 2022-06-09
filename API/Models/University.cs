using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public partial class University
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        //public virtual ICollection<Education> Educations { get; set; }

        private Lazy<List<Education>> _education;

        public University(int Id = 0)
        {
            _education = new Lazy<List<Education>>(() => 
            {
                return new List<Education>(Id);
            });
        }

        public List<Education> Educations
        {
            get
            {
                return _education.Value;
            }
            set { }
        }

    }
}
