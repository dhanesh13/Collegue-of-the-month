using Colleague_Of_The_Month.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Services
{
    public class SharedLibrary
    {

        public QueryEmployee GetEmployeesDetails(int epayrollId, COTMDBContext _context)
        {

            var query = (from employeedetails in _context.Details
                         join emp in _context.Employee on employeedetails.EmployeeId equals emp.EmployeeId
                         join dep in _context.Department on emp.DeptId equals dep.DeptId
                         join div in _context.Division on emp.DivisionId equals div.DivisionId
                         join man in _context.Details on employeedetails.ManagerId equals man.EmployeeId
                         where emp.PayrollId == epayrollId
                         select new QueryEmployee
                         {
                             PayrollID = emp.PayrollId,
                             EmployeeFirstName = employeedetails.FirstName,
                             EmployeeLastName = employeedetails.LastName,
                             EmployeeFirstNameManager = man.FirstName,
                             EmployeeLastNameManager = man.LastName,
                             EmployeeDepartment = dep.Name,
                             EmployeeDivision = div.Activity

                         }).FirstOrDefault();
            //return await _context.EmployeeDetails.ToListAsync();
            return query;

        }

    }
}
