using ClosedXML.Excel;
using Colleague_Of_The_Month.Interfaces;
using Colleague_Of_The_Month.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace Colleague_Of_The_Month.Services
{
    public class UploadService : IUpload
    {
        private readonly COTMDBContext _context;
        private IWebHostEnvironment Environment;
        private IConfiguration Configuration;

        public UploadService(IWebHostEnvironment _environment, IConfiguration _configuration, COTMDBContext context)
        {
            _context = context;
            Environment = _environment;
            Configuration = _configuration;
        }

        public bool Upload(IFormCollection formCollection)
        {
            #region
            try
            {
                bool updatedSuccessfullyForEmployeeTable = false;
                bool updatedSuccessfullyForDetailsTable = false;
                var file = formCollection.Files.First();
                var folderName = Path.Combine("wwwroot", "Files");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    var wb = new XLWorkbook(fullPath);
                    var ws = wb.Worksheet("Active");

                    //Create a new DataTable.
                    DataTable dt = new DataTable();

                    using (wb)
                    {
                        //Read the first Sheet from Excel file.
                        IXLWorksheet workSheet = wb.Worksheet("Active");

                        //Loop through the Worksheet rows.
                        bool firstRow = true;
                        foreach (IXLRow row in workSheet.Rows())
                        {
                            //Use the first row to add columns to DataTable.
                            if (firstRow)
                            {
                                foreach (IXLCell cell in row.Cells())
                                {
                                    dt.Columns.Add(cell.Value.ToString());
                                }
                                firstRow = false;
                            }
                            else
                            {
                                //Add rows to DataTable.
                                dt.Rows.Add();
                                int i = 0;
                                foreach (IXLCell cell in row.Cells())
                                {
                                    dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                                    i++;
                                }
                            }
                        }
                    }

                    var employeesList = dt.AsEnumerable().Select(x => new
                    {
                        PayrollId = x["Payroll ID"],
                        FirstName = x["First Name"],
                        LastName = x["Last Name"],
                        PreferredName = x["Preferred Name"],
                        Manager = x["Manager"],
                        CostCentreCode = x["Cost Centre Code"],
                        CostCentreName = x["Cost Centre Name"],
                        Division = x["3. Division / Activity"],
                        Subdivision = x["4. Subdivision"],
                        BusinessUnit = x["5. Business Unit"],
                        Department = x["6. Department"],
                        Email = x["Email"]
                    }).Distinct().ToList();
                    
                    foreach (var employee in employeesList)
                    {
                        var exist = false;
                        var query = _context.Employee.ToList();
                        foreach (var employeeDB in query)
                        {
                            try
                            {
                                string[] divisionCodeSplit = employee.Division.ToString().Split('-', 2);
                                var divCode = divisionCodeSplit[0].ToString().ToLower();

                                string[] subdivisionCodeSplit = employee.Subdivision.ToString().Split('-', 2);
                                var subdivCode = subdivisionCodeSplit[0].ToString().ToLower();

                                string[] businessUnitCodeSplit = employee.BusinessUnit.ToString().Split('-', 2);
                                var bUnitCode = businessUnitCodeSplit[0].ToString().ToLower();

                                string[] departmentCodeSplit = employee.Department.ToString().Split('-', 2);
                                var deptCode = departmentCodeSplit[0].ToString().ToLower();

                                var divisionId = _context.Division.Where(divisionName => divisionName.Code.ToLower() == divCode).FirstOrDefault();
                                var subdivisionId = _context.Subdivision.Where(subdivisionName => subdivisionName.Code.ToLower() == subdivCode).FirstOrDefault();
                                var businessUnitId = _context.BusinessUnit.Where(businessUnitName => businessUnitName.Code.ToLower() == bUnitCode).FirstOrDefault();
                                var departmentId = _context.Department.Where(departmentName => departmentName.Code.ToLower() == deptCode).FirstOrDefault();
                                var costCentreId = _context.CostCentre.Where(costCentreCode => costCentreCode.Code.ToLower() == employee.CostCentreCode.ToString().ToLower()).FirstOrDefault();

                                int defaultPayrollId = 0;
                                if (int.TryParse(employee.PayrollId.ToString(), out defaultPayrollId))
                                {
                                    if ((employeeDB.PayrollId == defaultPayrollId) && 
                                        (employeeDB.CostCentreId==costCentreId.CostCentreId) && 
                                        (employeeDB.DivisionId==divisionId.DivisionId) &&
                                        (employeeDB.SubdivisionId==subdivisionId.SubdivisionId) &&
                                        (employeeDB.UnitId==businessUnitId.UnitId) &&
                                        (employeeDB.DeptId ==departmentId.DeptId))
                                    {
                                        exist = true;
                                        break;
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                               Debug.WriteLine(ex.Message);
                            }
                            
                        }

                        if (exist == false)
                        {
                            var employeeModel = new Employee();

                            try
                            {
                                int defaultPayrollId = 0;
                                if (int.TryParse(employee.PayrollId.ToString(), out defaultPayrollId))
                                {
                                    employeeModel.PayrollId = defaultPayrollId;
                                }

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }

                            if (employeeModel.PayrollId ==0)
                            {
                                continue;
                            }

                            string[] divisionCodeSplit = employee.Division.ToString().Split('-', 2);
                            var divCode = divisionCodeSplit[0].ToString().ToLower();

                            string[] subdivisionCodeSplit = employee.Subdivision.ToString().Split('-', 2);
                            var subdivCode = subdivisionCodeSplit[0].ToString().ToLower();

                            string[] businessUnitCodeSplit = employee.BusinessUnit.ToString().Split('-', 2);
                            var bUnitCode = businessUnitCodeSplit[0].ToString().ToLower();

                            string[] departmentCodeSplit = employee.Department.ToString().Split('-', 2);
                            var deptCode = departmentCodeSplit[0].ToString().ToLower();

                            var divisionId = _context.Division.Where(divisionName => divisionName.Code.ToLower() == divCode).FirstOrDefault();
                            var subdivisionId = _context.Subdivision.Where(subdivisionName => subdivisionName.Code.ToLower() == subdivCode).FirstOrDefault();
                            var businessUnitId = _context.BusinessUnit.Where(businessUnitName => businessUnitName.Code.ToLower() == bUnitCode).FirstOrDefault();
                            var departmentId = _context.Department.Where(departmentName => departmentName.Code.ToLower() == deptCode).FirstOrDefault();
                            var costCentreId = _context.CostCentre.Where(costCentreCode => costCentreCode.Code.ToLower() == employee.CostCentreCode.ToString().ToLower()).FirstOrDefault();

                            employeeModel.CostCentreId = costCentreId.CostCentreId;

                            employeeModel.DivisionId = divisionId.DivisionId;
                            employeeModel.SubdivisionId = subdivisionId.SubdivisionId;
                            employeeModel.UnitId = businessUnitId.UnitId;
                            employeeModel.DeptId = departmentId.DeptId;

                            foreach (var emp in employeesList)
                            {
                                var empFirstName = employee.FirstName.ToString();
                                var empLastName = employee.LastName.ToString();
                                if ($"{empLastName}, {empFirstName}" == employee.Manager.ToString())
                                {
                                    employeeModel.Role = 1;
                                    break;
                                }
                                else
                                {
                                    employeeModel.Role = 0;
                                }
                            }
                            var updateEmployee = false;
                            var queryPayrollId = _context.Employee.ToList();
                            foreach (var dbPayrollId in queryPayrollId) {
                                if (dbPayrollId.PayrollId == Convert.ToInt32(employee.PayrollId))
                                {
                                    var employeeId = _context.Employee.Where(empEmployeeId => empEmployeeId.PayrollId == Convert.ToInt32(employee.PayrollId)).FirstOrDefault();
                                    var empInfo =  _context.Employee.Find(employeeId.EmployeeId);
                                    empInfo.CostCentreId= costCentreId.CostCentreId;
                                    empInfo.DivisionId = divisionId.DivisionId;
                                    empInfo.SubdivisionId = subdivisionId.SubdivisionId;
                                    empInfo.UnitId = businessUnitId.UnitId;
                                    empInfo.DeptId = departmentId.DeptId;
                                    _context.SaveChanges();
                                    updateEmployee = true;
                                    updatedSuccessfullyForEmployeeTable = true;
                                    break;
                                }
                            }
                            if (updateEmployee==false)
                            {
                                _context.Employee.Add(employeeModel);
                                _context.SaveChanges();
                                updatedSuccessfullyForEmployeeTable = true;
                            }
                        }
                    }
               
                    foreach (var employeeForDetails in employeesList)
                    {
                        var exist = false;
                        var query = _context.Details.ToList();
                        foreach (var employeeDB in query)
                        {
                            string[] managerName = employeeForDetails.Manager.ToString().Split(',');
                            string managerFirstName;
                            string managerLastName;
                            if (managerName.Count()==1)
                            {
                                 managerFirstName = string.Empty;
                                 managerLastName = !string.IsNullOrWhiteSpace(managerName[0]) ? managerName[0].Trim() : string.Empty;
                            }
                            else
                            {
                                 managerFirstName = !string.IsNullOrWhiteSpace(managerName[1]) ? managerName[1].Trim() : string.Empty;
                                 managerLastName = !string.IsNullOrWhiteSpace(managerName[0]) ? managerName[0].Trim() : string.Empty;
                            }
                            var managerId = _context.Details.Where(empDetails => empDetails.FirstName == managerFirstName && empDetails.LastName == managerLastName).FirstOrDefault();

                            if ((employeeDB.FirstName.ToLower() == employeeForDetails.FirstName.ToString().ToLower()) && 
                                (employeeDB.LastName.ToLower() == employeeForDetails.LastName.ToString().ToLower()) &&
                                (employeeDB.EmailAddress.ToLower()== employeeForDetails.Email.ToString().ToLower()) &&
                                (employeeDB.PreferredName.ToLower()==employeeForDetails.PreferredName.ToString().ToLower()) &&
                                (employeeDB.ManagerId.ToString().ToLower()==managerId.EmployeeId.ToString().ToLower()))
                            {
                                exist = true;
                                break;
                            }
                        }

                        if (exist == false)
                        {
                            var empDetailsModel = new Details();
                            var employeeModel = new Employee();
                            try
                            {
                                int defaultPayrollId = 0;
                                if (int.TryParse(employeeForDetails.PayrollId.ToString(), out defaultPayrollId))
                                {
                                    employeeModel.PayrollId = defaultPayrollId;
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }

                            if (employeeModel.PayrollId == 0)
                            {
                                continue;
                            }

                            empDetailsModel.FirstName = employeeForDetails.FirstName.ToString();
                            empDetailsModel.LastName = employeeForDetails.LastName.ToString();
                            empDetailsModel.PreferredName = employeeForDetails.PreferredName.ToString();

                            string[] managerName = employeeForDetails.Manager.ToString().Split(',');
                            var managerFirstName = !string.IsNullOrWhiteSpace(managerName[1]) ? managerName[1].Trim() : string.Empty;
                            var managerLastName = !string.IsNullOrWhiteSpace(managerName[0]) ? managerName[0].Trim() : string.Empty;
                            var managerId = _context.Details.Where(empDetails => empDetails.FirstName == managerFirstName && empDetails.LastName == managerLastName).FirstOrDefault();
                            empDetailsModel.ManagerId = managerId.EmployeeId;
                            empDetailsModel.EmailAddress = employeeForDetails.Email.ToString();
                            var password = "l@isemoRentre1#]";
                            empDetailsModel.Password = MD5.MD5Hash(password);
                            try
                            {
                                int defaultPayrollId = 0;
                                if (int.TryParse(employeeForDetails.PayrollId.ToString(), out defaultPayrollId))
                                {
                                    var employeeId = _context.Employee.Where(employeePayrollId => employeePayrollId.PayrollId == defaultPayrollId).FirstOrDefault();
                                    empDetailsModel.EmployeeId = employeeId.EmployeeId;
                                }

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }

                            var updateEmployee = false;
                            var queryEmployeeId = _context.Details.ToList();
                            foreach (var dbEmployeeId in queryEmployeeId)
                            {
                                var employeeId = _context.Employee.Where(employeePayrollId => employeePayrollId.PayrollId == Convert.ToInt32(employeeForDetails.PayrollId)).FirstOrDefault();
                                if (dbEmployeeId.EmployeeId == employeeId.EmployeeId)
                                {
                                    var detailsId = _context.Details.Where(empDetailsId => empDetailsId.EmployeeId == employeeId.EmployeeId).FirstOrDefault();
                                    var empInfo = _context.Details.Find(detailsId.DetailsId);
                                    empInfo.FirstName = employeeForDetails.FirstName.ToString();
                                    empInfo.LastName = employeeForDetails.LastName.ToString();
                                    empInfo.PreferredName = employeeForDetails.PreferredName.ToString();
                                    empInfo.ManagerId = managerId.EmployeeId;
                                    empInfo.EmailAddress = employeeForDetails.Email.ToString();
                                    _context.SaveChanges();
                                    updateEmployee = true;
                                    updatedSuccessfullyForDetailsTable = true;
                                    break;
                                }
                            }
                            if (updateEmployee == false)
                            {
                                _context.Details.Add(empDetailsModel);
                                _context.SaveChanges();
                                updatedSuccessfullyForDetailsTable = true;
                            }
                        }
                    }
                    var queryForDelete = _context.Employee.ToList();
                    foreach (var employeeDBForDelete in queryForDelete)
                    {
                        var exist = false;
                        foreach (var employeeForDelete in employeesList)
                        {
                            int defaultPayrollId = 0;
                            if (int.TryParse(employeeForDelete.PayrollId.ToString(), out defaultPayrollId))
                            {
                                if (defaultPayrollId == employeeDBForDelete.PayrollId)
                                {
                                    exist= true;
                                    break;
                                }
                            }
                        }

                        if(exist == false)
                        {
                            var empInfoModel =  _context.Employee.Find(employeeDBForDelete.EmployeeId);
                            _context.Employee.Remove(empInfoModel);
                            _context.SaveChanges();

                            var detailsId = _context.Details.Where(empDetailsId => empDetailsId.EmployeeId == employeeDBForDelete.EmployeeId).FirstOrDefault();
                            var empDetailsModel = _context.Details.Find(detailsId.DetailsId);
                            _context.Details.Remove(empDetailsModel);
                            _context.SaveChanges();
                        }
                    }
                }
                if (updatedSuccessfullyForEmployeeTable == true && updatedSuccessfullyForDetailsTable == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw ex.InnerException;
            }
            #endregion

        }
    }

}
