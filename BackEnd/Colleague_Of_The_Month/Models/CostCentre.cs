using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Models
{
    public class CostCentre
    {
        [Key]
        public int CostCentreId { get; set; }

        [Required]
        [Display(Name = "Cost Centre Code")]
        [StringLength(300)]
        public string Code { get; set; }

        [Required]
        [Display(Name = "Cost Centre Activity")]
        [StringLength(300)]
        public string Activity { get; set; }

    }
}
