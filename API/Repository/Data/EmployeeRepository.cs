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
using Microsoft.Extensions.Configuration;
using System.Net;
using API.Base;

namespace API.Repository.Data
{
    public class EmployeeRepository : Repository<MyContext, Employee, string>
    {
        private readonly MyContext _context;
        public IConfiguration _config;
        public EmployeeRepository(IConfiguration config, MyContext context) : base(context)
        {
            this._config = config;
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

        public List<GetEmployeeResponseVM> GetEmployeeData()
        {
            List<GetEmployeeResponseVM> listObjResponse = new List<GetEmployeeResponseVM>();
            List<Employee> listEmp = new List<Employee>();

            listEmp = (from a in _context.Employees where a.IsDeleted == (IsDeleted)Enum.Parse(typeof(IsDeleted), "False") select a).ToList();

            for (int i = 0; i < listEmp.Count; i++)
            {
                GetEmployeeResponseVM objResponse = new GetEmployeeResponseVM();

                objResponse.NIK = listEmp[i].NIK;
                objResponse.FirstName = listEmp[i].FirstName;
                objResponse.LastName = listEmp[i].LastName;
                objResponse.Email = listEmp[i].Email;
                objResponse.Phone = listEmp[i].Phone;
                objResponse.BirthDate = listEmp[i].BirthDate;
                objResponse.Salary = listEmp[i].Salary;
                objResponse.Gender = Enum.GetName(typeof(Gender), listEmp[i].Gender);

                listObjResponse.Add(objResponse);
            }

            return listObjResponse;
        }

        public ResponseObj GetGenderCount()
        {
            ResponseObj objRes = new ResponseObj();

            GenderCountVM obj = new GenderCountVM();

            List<string> genName = new List<string>();
            List<int> genCount = new List<int>();

            try
            {
                var req = (from a in _context.Employees
                           where a.IsDeleted == 0
                           group a by a.Gender into aGender
                           select new
                           {
                               GenderName = aGender.Key == 0 ? "Male" : "Female",
                               GenderCount = aGender.Count()
                           });

                foreach (var item in req)
                {
                    genName.Add(item.GenderName);
                    genCount.Add(item.GenderCount);
                }

                obj.Gender = genName;
                obj.GenderCount = genCount;


                objRes.statusCode = Convert.ToInt32(HttpStatusCode.OK);
                objRes.message = BaseException.SuccessRequest;
                objRes.data = obj;
            }
            catch (Exception ex)
            {
                objRes.statusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                objRes.message = BaseException.FailedRequest;
                objRes.data = ex;
            }

            return objRes;
        }

        public GetEmployeeResponseVM GetEmployeeByNIK(string NIK)
        {
            GetEmployeeResponseVM objResponse = new GetEmployeeResponseVM();

            #region Get Employee Data
            Employee empObj = _context.Employees.Find(NIK);
            objResponse.NIK = empObj.NIK;
            objResponse.FirstName = empObj.FirstName;
            objResponse.LastName = empObj.LastName;
            objResponse.Email = empObj.Email;
            objResponse.Phone = empObj.Phone;
            objResponse.BirthDate = empObj.BirthDate;
            objResponse.Salary = empObj.Salary;
            objResponse.Gender = Enum.GetName(typeof(Gender), empObj.Gender);
            #endregion

            #region Get Education Data
            List<string> eduDegree = new List<string>();
            List<string> eduGPA = new List<string>();

            List<Education> eduList = (from a in _context.Educations
                                       join b in _context.Profilings
                                       on a.Id equals b.Education_Id
                                       where b.NIK == NIK
                                       select a).ToList();
            List<string> uniList = new List<string>();


            for (int i = 0; i < eduList.Count; i++)
            {
                eduDegree.Add(Enum.GetName(typeof(Degree), eduList[i].Degree));
                eduGPA.Add(eduList[i].GPA);

                string uniName = (from a in _context.Universities
                                  where a.Id == eduList[i].University_Id
                                  select a.Name).FirstOrDefault();
                uniList.Add(uniName);
            }

            objResponse.UniversityName = uniList;
            objResponse.EducationDegree = eduDegree;
            objResponse.EducationGPA = eduGPA;
            #endregion

            #region Get Roles Data
            List<string> roleList = (from a in _context.AccountRoles
                                     join b in _context.Roles
                                     on a.RolesId equals b.RolesId
                                     where a.NIK == NIK
                                     select b.RoleName).ToList();

            objResponse.RoleName = roleList; 
            #endregion

            return objResponse;
        }
        public ResponseObj UpdateEmployee(UpdateEmployeeVM objReq)
        {
            ResponseObj objRes = new ResponseObj();

            Employee emp = (from a in _context.Employees
                            where a.NIK == objReq.NIK
                            select a).FirstOrDefault();

            emp.NIK = objReq.NIK;
            emp.FirstName = objReq.FirstName;
            emp.LastName = objReq.LastName;
            emp.Phone = objReq.Phone;
            emp.Email = objReq.Email;
            emp.BirthDate = objReq.BirthDate;
            emp.Salary = objReq.Salary;
            emp.Gender = (Gender)Enum.Parse(typeof(Gender), objReq.Gender);

            try
            {
                _context.SaveChanges();
                objRes.statusCode = Convert.ToInt32(HttpStatusCode.OK);
                objRes.message = BaseException.SuccessUpdate;
                objRes.data = BaseException.Msg_SuccessUpdate;
            }
            catch (Exception ex)
            {
                objRes.statusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                objRes.message = BaseException.FailedUpdate;
                objRes.data = ex;
            }
            
            return objRes;
        }
        public ResponseObj UpdateEducation(UpdateEducationVM objReq)
        {
            ResponseObj objRes = new ResponseObj();

            Education eduObj = (from a in _context.Profilings
                                join b in _context.Educations
                                on a.Education_Id equals b.Id
                                where a.NIK == objReq.NIK
                                select b).FirstOrDefault();

            eduObj.University_Id = objReq.UniversityId;
            eduObj.Degree = objReq.Degree;
            eduObj.GPA = objReq.GPA;

            try
            {
                _context.SaveChanges();
                objRes.statusCode = Convert.ToInt32(HttpStatusCode.OK);
                objRes.message = BaseException.SuccessUpdate;
                objRes.data = BaseException.Msg_SuccessUpdate;
            }
            catch (Exception ex)
            {
                objRes.statusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                objRes.message = BaseException.FailedUpdate;
                objRes.data = ex;
            }

            return objRes;
        }
        public ResponseObj DeleteEmployee(GetEmployeeParameterVM objReq)
        {
            ResponseObj objRes = new ResponseObj();

            Employee empObj = (from a in _context.Employees where a.NIK == objReq.NIK select a).FirstOrDefault();

            empObj.IsDeleted = (IsDeleted)Enum.Parse(typeof(IsDeleted), "True");

            try
            {
                _context.SaveChanges();
                objRes.statusCode = Convert.ToInt32(HttpStatusCode.OK);
                objRes.message = BaseException.SuccessDelete;
                objRes.data = BaseException.Msg_SuccessDelete;
            }
            catch (Exception ex)
            {
                objRes.statusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                objRes.message = BaseException.FailedDelete;
                objRes.data = ex;
            }

            return objRes;
        }

        public ResponseObj Register(RegisterVM obj)
        {
            ResponseObj objRes = new ResponseObj();

            int insertRes = 400;

            bool isEmailDuplicate = (from a in _context.Employees where a.Email == obj.Email select a).FirstOrDefault() != null;
            bool isPhoneDuplicate = (from a in _context.Employees where a.Phone == obj.Phone select a).FirstOrDefault() != null;
                        
            if (!isEmailDuplicate && !isPhoneDuplicate)
            {
                #region Employee Object
                Employee emp = new Employee();
                emp.NIK = GenerateNIK();
                emp.FirstName = obj.FirstName;
                emp.LastName = obj.LastName;
                emp.Phone = obj.Phone;
                emp.Gender = (Gender)Enum.Parse(typeof(Gender), obj.Gender);
                emp.Email = obj.Email;
                emp.BirthDate = obj.BirthDate;
                emp.Salary = obj.Salary;
                emp.IsDeleted = (IsDeleted)Enum.Parse(typeof(IsDeleted), "False");
                #endregion

                #region Account Object
                Account accObj = new Account
                {
                    Password = BC.HashPassword(obj.Password),
                    NIK = emp.NIK
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
                    Education_Id = eduObj.Id,
                    NIK = emp.NIK
                };
                #endregion

                #region AccountRoles Object
                AccountRoles accRolesObj = new AccountRoles
                {
                    NIK = emp.NIK,
                    RolesId = 3
                };
                #endregion

                #region Connect object
                profilingObj.Education = eduObj;
                accObj.Profiling = profilingObj;
                accRolesObj.Account = accObj;
                accRolesObj.Roles = (from a in _context.Roles where a.RolesId == 3 select a).FirstOrDefault();
                emp.Account = accObj;
                #endregion

                try
                {
                    _context.Employees.Add(emp);
                    _context.Add(accRolesObj);
                    _context.SaveChanges();

                    insertRes = 200;
                }
                catch (Exception ex)
                {
                    objRes.statusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                    objRes.message = BaseException.FailedRegister;
                    objRes.data = ex;
                    return objRes;
                }
            }
            else 
            {
                objRes.statusCode = Convert.ToInt32(HttpStatusCode.BadRequest);

                if (isPhoneDuplicate && !isEmailDuplicate) 
                {
                    objRes.message = BaseException.FailedRegister;
                    objRes.data = BaseException.DataDuplicate_Phone;
                }
                else if (!isPhoneDuplicate && isEmailDuplicate) 
                {
                    objRes.message = BaseException.FailedRegister;
                    objRes.data = BaseException.DataDuplicate_Email;
                }
                else 
                {
                    objRes.message = BaseException.FailedRegister;
                    objRes.data = BaseException.DataDuplicate_Email + " " + BaseException.DataDuplicate_Phone;
                }

                return objRes;
            }

            if (insertRes == 200)
            {
                AccountRepository accRepos = new AccountRepository(_config, _context);

                insertRes = accRepos.ForgetPassword(obj.Email);

                objRes.statusCode = insertRes == 200 ? 200 : 400;

                objRes.message = insertRes == 200 ? BaseException.SuccessRegister : BaseException.FailedRegister;
                objRes.data = insertRes == 200 ? BaseException.Msg_SuccessRegister : insertRes == 401 ? BaseException.DataNotFound_Email : BaseException.FailedChangePass;
            }

            return objRes;
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
