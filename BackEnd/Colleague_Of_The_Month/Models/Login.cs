using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Models
{
    public class Login
    {
        public int PayrollID { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeLastName { get; set; }

        public int EmployeeRole { get; set; }

        public Guid? ManagerGuidId { get; set; }

        public int ManagerId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool isAuth { get; set; }

        
    }
}
