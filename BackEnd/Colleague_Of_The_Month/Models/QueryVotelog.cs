using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Models
{
    public class QueryVotelog
    {
        public int NomineePayrollid { get; set; }
        public int TotalVote { get; set; }
        public string Period { get; set; }
    }
}
