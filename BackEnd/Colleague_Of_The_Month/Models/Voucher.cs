using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Models
{
    public class Voucher
    {

        [Key]
        public int VoucherID { get; set; }

        [Required]
        [Display(Name = "Voucher Name")]
        [StringLength(50)]
        public string VoucherName { get; set; }

        [Required]
        [Display(Name = "Nominee PayrollID")]
        public int PayrollID { get; set; }
        [Required]
        [Display(Name = "EventID")]
        public int EventID { get; set; }
        [Required]
        [Display(Name = "Period")]
        [StringLength(50)]
        public string Period { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(100)]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "First Name Manager")]
        [StringLength(100)]
        public string FirstNameManager { get; set; }
        [Required]
        [Display(Name = "Last Name Manager")]
       
        public string LastNameManager { get; set; }

        [Required]
        [Display(Name = "Department")]
        
        public string Department { get; set; }
        [Required]
        [Display(Name = "Division")]
        [StringLength(100)]
        public string Division { get; set; }

        [Required]
        [Display(Name = "Voucher Submission Check")]
        
        public Boolean VoucherSubmissionCheck { get; set; }
    }
}
