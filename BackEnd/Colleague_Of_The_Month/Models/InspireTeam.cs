using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Models
{
    public class InspireTeam
    {
        [Key]
        public Guid InspireTeamId { get; set; }

        [Required]
        [StringLength(100)]
        public string TeamName { get; set; }

        [Required]
        [StringLength(1500)]
        public string Impact { get; set; }

        [Required]
        [StringLength(1500)]
        public string BeASpark { get; set; }

        [Required]
        public int VoterPayrollId { get; set; }

        [Required]
        [StringLength(50)]
        public string Period { get; set; }

        public bool? Winner { get; set; }

        [StringLength(1500)]
        public string WinnerRationale { get; set; }     

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public DateTime DateLastModified { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        [Required]
        public int EventId { get; set; }
    }
}
