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
        public Education()
        {
            Profilings = new HashSet<Profiling>();
        }

        [Key]
        public int Id { get; set; }
        public Degree Degree { get; set; }
        [Required]
        public string GPA { get; set; }
        [Required]
        public int University_Id { get; set; }
        public virtual University University { get; set; }
        public virtual ICollection<Profiling> Profilings { get; set; }

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
