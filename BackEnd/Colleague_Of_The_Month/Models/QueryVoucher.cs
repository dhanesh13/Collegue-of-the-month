using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Models
{
    public class QueryVoucher
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
    }
}
