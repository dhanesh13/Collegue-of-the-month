using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Email
{
    public class EmailConfiguration
    {
        public string From { get; set; }

        // public bool UseDefaultCredentials { get; set; }

        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        // public bool EnableSsl { get; set; }
        public string SmtpServer { get; set; }
        public bool EnableSsl { get; set; }
    }
}
