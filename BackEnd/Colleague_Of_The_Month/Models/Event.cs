using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        [Display(Name = "Event Name")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Event Description")]
        [StringLength(100)]
        public string Description { get; set; }
    }
}
