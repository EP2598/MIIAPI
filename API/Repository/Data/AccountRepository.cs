using API.Base;
using API.Base.Services;
using API.Context;
using API.Models;
using API.Models.API;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace API.Repository.Data
{
    public class AccountRepository : Repository<MyContext, Account, string>
    {
        public IConfiguration _config;
        private MyContext _context;
        public AccountRepository(IConfiguration config, MyContext context) : base(context) 
        {
            this._context = context;
            this._config = config;
        }

        public int Login(LoginVM obj, out string Token)
        {
            int loginRes = 400;
            var idToken = string.Empty;
            List<Roles> listRoles = new List<Roles>();

            //Employee empObj = _context.Employees.Include(x => x.Account).
            //    ThenInclude(x => x.Profiling).
            //    ThenInclude(x => x.Education).
            //    ThenInclude(x => x.University).Where(x => x.Email == obj.Email).FirstOrDefault();

            Account accObj = (from acc in _context.Accounts
                              join emp in _context.Employees
                              on acc.NIK equals emp.NIK
                              where emp.Email == obj.Email
                              select acc).FirstOrDefault();

            List<Education> eduObj = (from edu in _context.Educations
                                select edu).ToList();

            if (accObj == null)
            {
                Token = null;
                return 401;
            }

            if (!BC.Verify(obj.Password, accObj.Password))
            {
                Token = null;
                return 402;
            }
            else
            {
                loginRes = 200;

                listRoles = (from a in _context.Accounts
                             join b in _context.AccountRoles
                             on a.NIK equals b.NIK
                             join c in _context.Roles
                             on b.RolesId equals c.RolesId
                             where a.NIK == accObj.NIK
                             select c).ToList();
            } 

            if (loginRes == 200)
            {
                var NIK = "";
                var cekRole = "";
                var claims = new List<Claim>();
                claims.Add(new Claim("Email", obj.Email));

                foreach (var roles in listRoles)
                {
                    claims.Add(new Claim("roles", roles.RoleName));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _config["Jwt:Issuer"],
                    _config["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: signIn
                    );

                idToken = new JwtSecurityTokenHandler().WriteToken(token);
                claims.Add(new Claim("TokenSecurity", idToken.ToString()));
            }

            Token = idToken;
            return loginRes;
        }

        public int ForgetPassword(string email)
        {
            EmailService es = new EmailService();

            Account accObj = (from acc in _context.Accounts
                              join emp in _context.Employees
                              on acc.NIK equals emp.NIK
                              where emp.Email.ToLower() == email.ToLower()
                              select acc).FirstOrDefault();

            if (accObj == null) return 401; //Tidak ada account yang dimaksud
            else
            {
                DateTime expiredTime = DateTime.Now.AddMinutes(30);
                string OTP = GenerateOTP();
                string newPass = GenerateOTP(12);


                accObj.OTP = OTP;
                accObj.IsOTPActive = true;
                accObj.ExpiredTime = expiredTime; //Expired dalam 30 menit
                accObj.Password = BC.HashPassword(newPass); //Soft lock account while OTP Active

                _context.SaveChanges();

                es.Send(BaseConstanta.emailSender, email, BaseConstanta.otpSubject, "Your OTP is " + OTP);

                return 200;
            }
        }

        public int ChangePassword(ChangePasswordVM obj)
        {
            Account accObj = (from acc in _context.Accounts
                              join emp in _context.Employees
                              on acc.NIK equals emp.NIK
                              where emp.Email.ToLower() == obj.Email.ToLower()
                              select acc).FirstOrDefault();

            if (accObj == null) return 401;
            else
            {
                if (accObj.IsOTPActive == false) return 402; //OTP Tidak aktif, tidak dapat Forget Password
                if (accObj.OTP != obj.OTP) return 403; //OTP Aktif, tapi salah insert
                if (DateTime.Now > accObj.ExpiredTime)
                {
                    #region Reset OTP
                    accObj.IsOTPActive = false;
                    accObj.OTP = null;

                    _context.SaveChanges();
                    #endregion

                    return 404; //OTP sudah expired.
                }
                else
                {
                    if (obj.NewPassword == obj.RetryPassword)
                    {
                        #region Reset OTP
                        accObj.IsOTPActive = false;
                        accObj.OTP = null;
                        accObj.Password = BC.HashPassword(obj.NewPassword);

                        _context.SaveChanges();
                        #endregion
                        return 200;
                    }
                    else
                    {
                        return 405; //Password input tidak cocok
                    }
                }
            }
        }

        public int SignManager(string NIK)
        {
            #region Check if Employee already assigned as Manager
            AccountRoles accRolesObj = (from a in _context.Employees
                                        join b in _context.Accounts
                                        on a.NIK equals b.NIK
                                        join c in _context.AccountRoles
                                        on b.NIK equals c.NIK
                                        where a.NIK == NIK && c.RolesId == 2
                                        select c).FirstOrDefault(); 
            #endregion

            if (accRolesObj == null)
            {
                AccountRoles accroleObj = new AccountRoles();
                AccountRoles newAccRolesObj = new AccountRoles
                {
                    NIK = NIK,
                    RolesId = (from a in _context.Roles where a.RolesId == 2 || a.RoleName == "Manager" select a.RolesId).FirstOrDefault()
                };

                _context.Add(newAccRolesObj);
                _context.SaveChanges();

                return 200;
            }
            else
            {
                return 400;
            }
        }

        private string GenerateOTP(int length = 7)
        {
            //Referensi : https://www.aspsnippets.com/Articles/Generate-Unique-Random-OTP-One-Time-Password-in-ASPNet-using-C-and-VBNet.aspx
            string OTP = String.Empty;

            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string numbers = "1234567890";
            string characters = numbers + alphabets;

            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (OTP.IndexOf(character) != -1);
                OTP += character;
            }

            return OTP;
        }
    }
}
