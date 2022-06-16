using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [JsonObject(IsReference = true)]
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
        public IsDeleted IsDeleted { get; set; }
        public Gender Gender { get; set; }

        public virtual Account Account { get; set; }


    }
    public enum Gender
    {
        Male,
        Female
    }
    public enum IsDeleted
    { 
        False,
        True
    }
}
