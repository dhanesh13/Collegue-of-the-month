using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Models
{
    public class Employee
    {   
        [Key]
        public Guid EmployeeId { get; set; }

        [Required]
        [Display(Name = "Payroll Id")]
        public int PayrollId { get; set; }
   

        //FK
        [ForeignKey("Cost Centre")]
        public int CostCentreId { get; set; }

        //FK
        [ForeignKey("Division")]
        public Guid DivisionId { get; set; }

        //FK
        [ForeignKey("Subdivision")]
        public Guid SubdivisionId { get; set; }

        //FK
        [ForeignKey("BusinessUnit")]
        public Guid UnitId { get; set; }

        //FK
        [ForeignKey("Department")]
        public Guid DeptId { get; set; }

        [Display(Name = "Role")]
        public int Role { get; set; }
    }
}
