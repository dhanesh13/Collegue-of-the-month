using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Models
{
    public class QueryNominee
    {
        public int employeeId { get; set; }
        public QueryEmployee query { get; set; }
        public int nominationsCount { get; set; }
        public int managerVotesCount { get; set; }
        public List<Nominee> nominations { get; set; }
    }
}
