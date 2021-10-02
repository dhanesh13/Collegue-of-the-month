using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Models
{
    public class QueryInspire
    {
        //public int teamId { get; set; }

        public string teamName { get; set; }

        public List<InspireTeam> nominations { get; set; }
    }
}
