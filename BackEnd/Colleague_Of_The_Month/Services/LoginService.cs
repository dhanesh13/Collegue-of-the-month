using Colleague_Of_The_Month.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Services
{
    public class LoginService : ILogin
    {
        private readonly COTMDBContext _context;

        public LoginService(COTMDBContext context)
        {
            _context = context;
        }

        public Login Authenticate(Login LoginModel)
        {
            try {
                var passwordHash = MD5.MD5Hash(LoginModel.Password);

                var query = (from employeedetails in _context.Details
                             join emp in _context.Employee on employeedetails.EmployeeId equals emp.EmployeeId
                             where (employeedetails.EmailAddress == LoginModel.Username &&
                             employeedetails.Password == passwordHash)

                             select new Login
                             {
                                 PayrollID = emp.PayrollId,
                                 EmployeeFirstName = employeedetails.FirstName,
                                 EmployeeLastName = employeedetails.LastName,
                                 EmployeeRole = emp.Role,
                                 ManagerGuidId = employeedetails.ManagerId,
                                 ManagerId = _context.Employee.Where(e => e.EmployeeId == employeedetails.ManagerId).FirstOrDefault().PayrollId,
                                 isAuth = true
                             }).FirstOrDefault();

                if (query != null)
                {                  
                    return query;
                }
                else
                {
                    var invalidLogin = new Login
                    {
                        isAuth = false
                    };
                    return invalidLogin;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw ex;
            }
        }


        public bool ChangePassword(ChangePassword ChangePasswordModel)
        {
            var oldPasswordHash= MD5.MD5Hash(ChangePasswordModel.OldPassword);

            var query = _context.Details.Where(employeeDetails => employeeDetails.EmailAddress == 
            ChangePasswordModel.Username && employeeDetails.Password == oldPasswordHash).FirstOrDefault();

            var newPasswordHash = MD5.MD5Hash(ChangePasswordModel.NewPassword);
            var confirmNewPasswordHash = MD5.MD5Hash(ChangePasswordModel.ConfirmNewPassword);

            if (query == null)
            {
                return false;
            }

            if (confirmNewPasswordHash == newPasswordHash)
            {
                query.Password = confirmNewPasswordHash;
                _context.SaveChanges();
                return true;
            }
            
            return false;          
        }
    }
}
