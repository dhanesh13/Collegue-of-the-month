using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Models
{
    public class Subdivision
    {
        [Key]
        public Guid SubdivisionId { get; set; }

        [Required]
        [Display(Name = "Subdivision Code")]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [Display(Name = "Subdivision Name")]
        [StringLength(100)]
        public string Name { get; set; }

        //FK
        [Required]
        [ForeignKey("Division")]
        public Guid DivisionId { get; set; }
    }
}
