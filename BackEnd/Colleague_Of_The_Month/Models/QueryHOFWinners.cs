using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Models
{
    public class QueryHOFWinners
    {
        public int PayrollId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int EventId { get; set; }
        public string Period { get; set; }
        public bool? Winner { get; set; }
        public string Rationale { get; internal set; }
    }
}
