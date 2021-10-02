using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Models
{
    public class VotingSession
    {
        [Key]
        public int sessionid { get; set; }
        public string period { get; set; }
        public int eventid { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
        public bool status { get; set; }
    }
}
