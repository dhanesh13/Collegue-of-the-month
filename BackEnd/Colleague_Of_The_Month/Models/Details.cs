using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Models
{
    public class Details
    {
        [Key]
        public Guid DetailsId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Display(Name = "Preferred Name")]
        [StringLength(50)]
        public string PreferredName { get; set; }

        
        [Display(Name = "Manager Id")]
        public Guid? ManagerId { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        [StringLength(60)]
        public string EmailAddress { get; set; }

        //FK
        [Required]
        [ForeignKey("Employee")]
        [Display(Name = "Employee Id")]
        public Guid EmployeeId { get; set; }

        [Display(Name = "Password")]
        [StringLength(100)]
        public string Password { get; set; }
    }
}
