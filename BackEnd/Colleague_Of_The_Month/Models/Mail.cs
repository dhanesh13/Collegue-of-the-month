using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Models
{
    public class Mail
    {
        [Key]
        
        public string Period { get; set; }
        public bool MailSent { get; set; }
        public bool MailSentManagers { get; set; }
        public bool MailSentManagersShortlistPeriod { get; set; }
        public bool MailSentWinners { get; set; }
        public bool MailSentVouchers { get; set; }


    }
}
