using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Models
{
    public class Nominee
    {
        [Key]
        public Guid NomId { get; set; }

        [Required]
        [Display(Name = "Nominee Payroll Id")]
        public int NomineePayrollId { get; set; }

        [Required]
        [Display(Name = "Impact")]
        [StringLength(1500)]
        public string Impact { get; set; }

        [Required]
        [Display(Name = "Be A Spark")]
        [StringLength(1500)]
        public string BeASpark { get; set; }

        [Required]
        [Display(Name = "Voter Payroll Id")]
        public int VoterPayrollId { get; set; }

        [Required]
        [Display(Name = "Period")]
        [StringLength(50)]
        public string Period { get; set; }

        [Display(Name = "Shortlisted")]
        public bool Shortlisted { get; set; }

        [Display(Name = "Managers Rationale")]
        [StringLength(1500)]
        public string ManagersRationale { get; set; }

        [Display(Name = "Winner Rationale")]
        [StringLength(1500)]
        public string WinnerRationale { get; set; }

        
        [Display(Name = "Managers Votes")]
        public int ManagersVotes { get; set; }

        [Display(Name = "Admin Rationale")]
        [StringLength(1500)]
        public string AdminRationale { get; set; }

        [Display(Name = "Winner")]
        public bool? Winner { get; set; }

        [Required]
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        [Required]
        [Display(Name = "Date Last Modified")]
        public DateTime DateLastModified { get; set; }

        
        [Display(Name = "Modified By")]
        [StringLength(50)]
        public string ModifiedBy { get; set; }


        //FK
        [Required]
        [ForeignKey("Event")]
        [Display(Name = "Event Id")]
        public int EventId { get; set; }

        //FK
        [ForeignKey("Voucher")]
        [Display(Name = "Voucher Id")]
        public int? VoucherId { get; set; }

        [NotMapped]
        public string VotersFullName { get; set; }
    }
}
