using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Models
{
    public class Division
    {
        [Key]
        public Guid DivisionId { get; set; }

        [Required]
        [Display(Name = "Division Code")]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [Display(Name = "Division Name")]
        [StringLength(50)]
        public string Activity { get; set; }
    }
}
