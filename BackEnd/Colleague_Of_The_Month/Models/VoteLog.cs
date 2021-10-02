using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Models
{
    public class VoteLog
    {
        [Key]
        public int votelogid { get; set; }
        public int sessionid { get; set; }
        public int nomineepayrollid { get; set; }
        public bool voted { get; set; }
        public string period { get; set; }
        public int eventid { get; set; }
        public int managerpayrollid { get; set; }
        [NotMapped]
        public string name { get; set; }
       // [NotMapped]
       // public Nominee nominations { get; set; }







    }
}
