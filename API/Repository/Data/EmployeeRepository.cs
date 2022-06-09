using API.Context;
using API.Models;
using API.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API.Models.API;
using BC = BCrypt.Net.BCrypt;

namespace API.Repository.Data
{
    public class EmployeeRepository : Repository<MyContext, Employee, string>
    {
        private readonly MyContext _context;
        public EmployeeRepository(MyContext context) : base(context)
        {
            this._context = context;
        }

        public List<GetRegisteredEmployeeVM> GetRegisteredData()
        {
            List<GetRegisteredEmployeeVM> listRegisteredEmp = new List<GetRegisteredEmployeeVM>();
            List<Employee> listEmployee = (from a in _context.Employees select a).ToList();

            foreach (var emp in listEmployee)
            {
                Account accObj = (from a in _context.Accounts where a.NIK == emp.NIK select a).FirstOrDefault();
                Education eduObj = (from a in _context.Profilings
                                    join b in _context.Educations
                                    on a.Education_Id equals b.Id
                                    where a.NIK == accObj.NIK
                                    select b).FirstOrDefault();
                string univName = (from a in _context.Universities
                                      where a.Id == eduObj.University_Id
                                      select a.Name).FirstOrDefault();

                GetRegisteredEmployeeVM registerViewObj = new GetRegisteredEmployeeVM()
                {
                    FullName = emp.FirstName + " " + emp.LastName,
                    Phone = emp.Phone,
                    BirthDate = emp.BirthDate,
                    Salary = emp.Salary,
                    Degree = Enum.GetName(typeof(Degree),eduObj.Degree),
                    Gender = Enum.GetName(typeof(Gender), emp.Gender),
                    Email = emp.Email,
                    GPA = eduObj.GPA,
                    UniversityName = univName
                };

                listRegisteredEmp.Add(registerViewObj);
            }

            return listRegisteredEmp;
        }

        public Employee GetEmployeeByNIK(string NIK)
        {
            return _context.Employees.Where(x => x.NIK == NIK).FirstOrDefault();
        }
                
        public int Register(RegisterVM obj)
        {
            int insertRes = 400;

            bool isEmailDuplicate = (from a in _context.Employees where a.Email == obj.Email select a).FirstOrDefault() != null;
            bool isPhoneDuplicate = (from a in _context.Employees where a.Phone == obj.Phone select a).FirstOrDefault() != null;
                        
            if (!isEmailDuplicate && !isPhoneDuplicate)
            {
                #region Employee Object
                Employee emp = new Employee(GenerateNIK());
                emp.FirstName = obj.FirstName;
                emp.LastName = obj.LastName;
                emp.Phone = obj.Phone;
                emp.Gender = (Gender)Enum.Parse(typeof(Gender), obj.Gender);
                emp.Email = obj.Email;
                emp.BirthDate = obj.BirthDate;
                emp.Salary = obj.Salary;
                emp.IsDeleted = false;
                #endregion

                #region Account Object
                Account accObj = new Account
                {
                    Password = BC.HashPassword(obj.Password),
                    Employee = emp
                };
                #endregion

                #region Education Object
                Education eduObj = new Education
                {
                    Degree = (Degree)obj.Degree,
                    GPA = obj.GPA,
                    University_Id = obj.UniversityId
                };
                #endregion

                #region Profiling Object
                Profiling profilingObj = new Profiling
                {
                    Education_Id = eduObj.Id
                };
                #endregion

                #region AccountRoles Object
                AccountRoles accRolesObj = new AccountRoles
                {
                    RolesId = 3
                };
                #endregion

                try
                {
                    _context.Employees.Add(emp);
                    //_context.Add(accRolesObj);
                    insertRes = _context.SaveChanges();
                    insertRes = 200;
                }
                catch (Exception ex)
                {
                    return insertRes;
                }
            }
            else 
            {
                if (isPhoneDuplicate && !isEmailDuplicate) insertRes = 401;
                else if (!isPhoneDuplicate && isEmailDuplicate) insertRes = 402;
                else insertRes = 403;

                return insertRes;
            }

            return insertRes;
        }

        private string GenerateNIK()
        {
            string NIK = "";

            NIK = DateTime.Now.ToString("MM/dd/yyyy");
            NIK = NIK.Replace("/", String.Empty);

            int NIKCount = (from a in _context.Employees select a).Count();
            NIKCount++;
            //string lastNIK = (from a in _context.Employees orderby a.NIK ascending select a.NIK).LastOrDefault();

            //if (!String.IsNullOrWhiteSpace(lastNIK)) lastNIK = lastNIK[8..];
            //else lastNIK = "0000";

            //int convertedNIK = Convert.ToInt32(lastNIK);
            //convertedNIK++;

            if (NIKCount < 10)
            {
                NIK += "000" + NIKCount.ToString(); // MMDDYYYY000X
            }
            else if (NIKCount < 100)
            {
                NIK += "00" + NIKCount.ToString(); //MMDDYYYY00XX
            }
            else if (NIKCount < 1000)
            {
                NIK += "0" + NIKCount.ToString(); //MMDDYYYY0XXX
            }
            else
            {
                NIK += NIKCount.ToString(); //MMDDYYYYXXXX
            }

            return NIK;
        }
    }
}
