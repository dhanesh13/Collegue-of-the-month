using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Models
{
    public class BusinessUnit
    {
        [Key]
        public Guid UnitId { get; set; }

        [Required]
        [Display(Name = "Business Unit Code")]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [Display(Name = "Business Unit Name")]
        [StringLength(100)]
        public string Name { get; set; }

        //FK
        [Required]
        [ForeignKey("Subdivision")]
        public Guid Subdivision { get; set; }
        
    }
}
