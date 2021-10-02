using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Models
{
    public class VoteLogNominees
    {
        public List<VoteLog> voteLogs { get; set; }
        public List<QueryNominee> nominations { get; set; }

    }
}
