using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Colleague_Of_The_Month.Models;
using Colleague_Of_The_Month.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Colleague_Of_The_Month.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly COTMDBContext _context;
        private readonly ILogin _ILogin;
        public LoginController(COTMDBContext context, ILogin iLogin)
        {
            _context = context;
            _ILogin = iLogin;
        }

        [HttpPost("login")]
        public Login Authenticate(Login LoginModel)
        {
            try
            {
                return _ILogin.Authenticate(LoginModel);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw ex;
            }
        }
      

        [HttpPut("changePassword")]
        public bool ChangePassword(ChangePassword ChangePasswordModel)
        {
            bool saveOk = false;
            try
            {
                saveOk = _ILogin.ChangePassword(ChangePasswordModel);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return saveOk;
        }
    }
}
