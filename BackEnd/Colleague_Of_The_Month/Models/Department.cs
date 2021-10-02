using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Models
{
    public class Department
    {
        [Key]
        public Guid DeptId { get; set; }

        [Required]
        [Display(Name = "Department Code")]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [Display(Name = "Department Name")]
        [StringLength(100)]
        public string Name { get; set; }

        //FK
        [Required]
        [ForeignKey("Department")]
        public Guid UnitId { get; set; }
        
    }
}
