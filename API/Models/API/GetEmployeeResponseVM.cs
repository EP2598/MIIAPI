using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.API
{
    public class GetEmployeeResponseVM
    {
        #region Employee
        public string NIK { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public decimal Salary { get; set; }
        public string Gender { get; set; }
        #endregion

        #region Education
        public List<string> UniversityName { get; set; }
        public List<string> EducationDegree { get; set; }
        public List<string> EducationGPA { get; set; }
        #endregion

        #region Roles
        public List<string> RoleName { get; set; } 
        #endregion
    }
}
