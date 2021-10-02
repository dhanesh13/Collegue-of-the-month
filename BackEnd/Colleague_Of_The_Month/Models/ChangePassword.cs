using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Models
{
    public class ChangePassword
    {
        public string Username { get; set; }

        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
