using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Colleague_Of_The_Month.Interfaces;
using Colleague_Of_The_Month.Models;
using Colleague_Of_The_Month.Services;
using Colleague_Of_The_Month.Email;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Colleague_Of_The_Month.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class COTMController : ControllerBase
    {
        private readonly COTMDBContext _context;
        private readonly INominee _INominee;
        private readonly IDetails _IDetails;
        private readonly IDateAccess _IDateAccess;
        private readonly IBusinessUnit _IBusinessUnit;
        private readonly ICostCentre _ICostCentre;
        private readonly IDepartment _IDepartment;
        private readonly IDivision _IDivision;
        private readonly IEmployee _IEmployee;
        private readonly IEvent _IEvent;
        private readonly ISubdivision _ISubdivision;
        private readonly IVoucher _IVoucher;
        private readonly IMail _IMail;
        private readonly IEmailSender _emailSender;

        public COTMController(COTMDBContext context, INominee iNominee, IBusinessUnit iBusinessUnit, ICostCentre iCostCentre,
                                IDetails iDetails, IDateAccess iDateAccess, IDepartment iDepartment, IDivision iDivision,
                                IEmployee iEmployee, IEvent iEvent, ISubdivision iSubdivision, IVoucher iVoucher,IMail iMail, IEmailSender emailSender)
        {
            _context = context;
            _INominee = iNominee;
            _IDetails = iDetails;
            _IDateAccess = iDateAccess;
            _IBusinessUnit = iBusinessUnit;
            _ICostCentre = iCostCentre;
            _IDepartment = iDepartment;
            _IDivision = iDivision;
            _IEmployee = iEmployee;
            _IEvent = iEvent;
            _ISubdivision = iSubdivision;
            _IVoucher = iVoucher;
            _IMail = iMail;
            _emailSender = emailSender;
        }

        [HttpPost("create")]
        public bool Create([FromBody] Nominee NomineeModel)
        {   
            bool saveOk = false;
            try
            {
                saveOk = _INominee.Create(NomineeModel); 
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return saveOk;
        }

        [HttpGet("getEmployeeDetails")]
        public async Task<List<Details>> GetEmployeeDetails()
        {
            return await _IDetails.GetEmployeeDetails();
        }

        [HttpGet("getDivisions")]
        public async Task<List<Division>> GetDivisions()
        {
            return await _IDivision.GetDivisions();
        }

        [HttpGet("getSubdivisions")]
        public async Task<List<Subdivision>> GetSubdivisions()
        {
            return await _ISubdivision.GetSubdivisions();
        }

        [HttpGet("getBusinessUnits")]
        public async Task<List<BusinessUnit>> GetBusinessUnits()
        {
            return await _IBusinessUnit.GetBusinessUnits();
        }

        [HttpGet("getDepartments")]
        public async Task<List<Department>> GetDepartments()
        {
            return await _IDepartment.GetDepartments();
        }

        [HttpGet("getNominees/{eventid}/{period}")]
        public string GetNominees(int eventid, string period)
        {
            return JsonConvert.SerializeObject(_INominee.GetNominees(eventid,period));
        }

        [HttpGet("getNominees/{eventid}/{take}/{period}")]
        public List<QueryNominee> GetNominees(int eventid, int take, string period)
        {
            return _INominee.GetNominees(eventid, take, period);

        }

        [HttpGet("admingetvotednominees/{eventid}/{period}")]
        public string GetVotedNominees(int eventid,string period)
        {
            return JsonConvert.SerializeObject(_INominee.AdminGetVotedNominees(eventid,period));
        }

        [HttpGet("admingetvotednominees/{eventid}/{take}/{period}")]
        public string GetVotedNominees(int eventid, int take,string period)
        {
            return JsonConvert.SerializeObject(_INominee.AdminGetVotedNominees(eventid, take,period));
        }

        [HttpGet("getnominees/{eventid}/{take}/{mid}/{period}")]
        public string GetNominees(int eventid, int take, int mid, string period)
        {
            return JsonConvert.SerializeObject(_INominee.GetNominees(eventid, take, mid, period));
        }

        [HttpGet("getCostCentres")]
        public async Task<List<CostCentre>> GetCostCentres()
        {
            return await _ICostCentre.GetCostCentres();
        }

        [HttpGet("getEmployees")]
        public async Task<List<Employee>> GetEmployees()
        {
            return await _IEmployee.GetEmployees();
        }

        [HttpGet("getemployee/{payrollid}")]
        public Employee GetEmployees(int payrollid)
        {
            return _IEmployee.GetEmployees(payrollid);
        }

        [HttpGet("getEvents")]
        public async Task<List<Event>> GetEvents()
        {
            return await _IEvent.GetEvents();
        }

        [HttpGet("getVouchers")]
        public async Task<List<Voucher>> GetVouchers()
        {
            return await _IVoucher.GetVouchers();
        }

        [HttpPost("vouchercreate")]
        public bool PostVoucher(QueryVoucher incomingVoucherData)
        {
            bool saveOk = false;
            try
            {
                saveOk = _IVoucher.PostVoucher(incomingVoucherData);
                if (saveOk)
                {
                    var queryVoucher = _context.Voucher
                     
                     .Where(v => v.EventID == incomingVoucherData.EventID && v.PayrollID == incomingVoucherData.PayrollID && v.Period == incomingVoucherData.Period && v.VoucherSubmissionCheck == true);
                    string[] details = new string[8];
                    foreach (var element in queryVoucher)
                    {
                        details[0] = element.FirstName;
                        details[1] = element.LastName;
                        details[2] = element.FirstNameManager;
                        details[3] = element.LastNameManager;
                        details[4] = element.Department;
                        details[5] = element.Division;
                        details[6] = element.Period;
                        details[7] = element.VoucherName;
                    }

                    //retrieve email addresses
                    var emails = _IDetails.GetTestEmails();
                    //retrieve closing date for voting session
                    // var closingdate = _IVotes.GetClosingDateOfCOTMEvent();
                    var message = new Message(emails, "Colleague Of The Month Voucher", "Voucher Test");
                    _emailSender.SendEmailForChoiceOfVouchers(message, details);

                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return saveOk;
        }

        [HttpGet("getEmployeesByFilter")]
        public async Task<List<QueryEmployee>> GetEmployeesByDivDepMan()
        {
            return await _IEmployee.GetEmployeesByDivDepMan();
        }

        [HttpGet("getDateAccess")]
        public bool DateAccess()
        {
            return _IDateAccess.DateAccess();
        }

        [HttpGet("getDateAccessShortlist")]
        public bool DateAccessShortlist()
        {
            return _IDateAccess.DateAccessShortlist();
        }

        [HttpGet("getPeriod")]
        public string GetPeriod()
        {
            return _IDateAccess.GetPeriod();
        }

        
        [HttpPut("managerShortlistNominee/{eventid}/{payrollid}/{period}")]
        public IActionResult ShortlistNominee(int eventid, int payrollid, string period)
        {


            if (ModelState.IsValid)
            {
                try
                {
                    _INominee.ShortlistNominee(eventid, payrollid, period);

                }
                catch (Exception e)
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"{e.Message}");
                }
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }

        }


        //// PUT api/<COTMController>/RemoveShortlistedNominee Active
        [HttpPut("/RemoveShortlistedNominee/{payrollid}/{period}")]
        public IActionResult RemoveShortlistedNominee(int eventid, int payrollid, string period)
        {


            if (ModelState.IsValid)
            {
                try
                {
                    _INominee.RemoveShortlistedNominee(eventid, payrollid, period);

                }
                catch (Exception e)
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"{e.Message}");
                }
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        [HttpGet("nonshortlistednominees/{eventid}/{period}")]
        public string GetNonShortlistedNomineesAsync(int eventid, string period)
        {
            return JsonConvert.SerializeObject(_INominee.GetNonShortListedNominees(eventid, period));
        }

        [HttpGet("nonshortlistednominees/{eventid}/{take}/{period}")]
        public string GetNonShortListedNominees(int eventid, int take, string period)
        {
            return JsonConvert.SerializeObject(_INominee.GetNonShortListedNominees(eventid, take, period));
        }

        [HttpGet("nonshortlistednominee/{eventid}/{id}/{period}")]
        public string GetNonShortListedNominee(int eventid, int id, string period)
        {
            return JsonConvert.SerializeObject(_INominee.GetNonShortListedNominee(eventid, id, period));
        }

        [HttpGet("shortlistednominees/{eventid}/{period}")]
        public string GetShortlistedNomineesAsync(int eventid, string period)
        {
            return JsonConvert.SerializeObject(_INominee.GetShortListedNominees(eventid, period));
        }

        [HttpGet("shortlistednominees/{eventid}/{take}/{period}")]
        public string GetShortListedNominees(int eventid, int take, string period)
        {
            return JsonConvert.SerializeObject(_INominee.GetShortListedNominees(eventid, take,period));
        }

        [HttpGet("shortlistednominee/{eventid}/{id}/{period}")]
        public string GetShortListedNominee(int eventid, int id, string period)
        {
            return JsonConvert.SerializeObject(_INominee.GetShortListedNominee(eventid, id,period));
        }
        [HttpGet("shortlistednomineeByNomineePayrollID/{eventid}/{id}/{period}")]
        public string GetShortListedNomineeByNomineePayrollID(int eventid, int id, string period)
        {
            return JsonConvert.SerializeObject(_INominee.GetShortListedNomineeByNomineePayrollID(eventid, id, period));
        }

        [HttpGet("shortlistednomineesbymanager/{eventid}/{take}/{mid}/{period}")]
        public string GetShortListedNomineesByManager(int eventid, int take,int mid,string period)
        {
            return JsonConvert.SerializeObject(_INominee.GetShortListedNomineesByManager(eventid, take,mid,period));
        }

        [HttpGet("rejectednomineesbymanager/{eventid}/{mid}/{period}")]
        public string GetRejectedNomineesByManager(int eventid, int mid, string period)
        {
            return JsonConvert.SerializeObject(_INominee.GetRejectedNomineesByManager(eventid, mid, period));
        }

        [HttpGet("topfiverejectednomineesbymanager/{eventid}/{take}/{mid}/{period}")]
        public string GetTopFiveRejectedNomineesByManager(int eventid, int take, int mid, string period)
        {
            return JsonConvert.SerializeObject(_INominee.GetTopFiveRejectedNomineesByManager(eventid, take, mid, period));
        }


        //// PUT api/<COTMController>/managerShortlistNominee Active
        [HttpPut("/adminWinnerNominee/{eventid}/{payrollid}/{period}")]
        public IActionResult WinnerNominee(int eventid, int payrollid,string period)
        {


            if (ModelState.IsValid)
            {
                try
                {
                    _INominee.WinnerNominee(eventid, payrollid,period);

                }
                catch (Exception e)
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"{e.Message}");
                }
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        //// PUT api/<COTMController>/managerShortlistNominee Active
        [HttpPut("/adminDeleteWinner/{eventid}/{payrollid}/{period}")]
        public IActionResult DeleteWinner(int eventid, int payrollid,string period)
        {


            if (ModelState.IsValid)
            {
                try
                {
                    _INominee.DeleteWinner(eventid, payrollid, period);

                }
                catch (Exception e)
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"{e.Message}");
                }
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        //// Patch api/<COTMController>/shortlistNomineeWithRationale Active
        [HttpPatch("shortlistNomineeWithRationale/{eventid}/{PayrollID}/{rationale}/{period}")]
        public IActionResult ShortlistNomineeWithRationale(int eventid, int PayrollID, string rationale, string period)
        {


            if (ModelState.IsValid)
            {
                try
                {
                    _INominee.ShortlistNomineeWithRationale(eventid, PayrollID, rationale, period);

                }
                catch (Exception e)
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"{e.Message}");
                }
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        //// Patch api/<COTMController>/shortlistNomineeWithRationale Active
        [HttpPatch("shortlistNomineeWithManagerRationale/{eventid}/{PayrollID}/{rationale}/{period}")]
        public IActionResult ShortlistNomineeWithManagerRationale(int eventid, int PayrollID, string rationale, string period)
        {


            if (ModelState.IsValid)
            {
                try
                {
                    _INominee.ShortlistNomineeWithManagerRationale(eventid, PayrollID, rationale,period);

                }
                catch (Exception e)
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"{e.Message}");
                }
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }

        }
        //// Patch api/<COTMController>/managerRejectionRationale Active
        [HttpPatch("managerRejectionRationale/{eventid}/{PayrollID}/{rationale}/{period}")]
        public IActionResult NomineeRejectionRationaleByManager(int eventid, int PayrollID, string rationale, string period)
        {


            if (ModelState.IsValid)
            {
                try
                {
                    _INominee.NomineeRejectionRationaleByManager(eventid, PayrollID, rationale,period);

                }
                catch (Exception e)
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"{e.Message}");
                }
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        [HttpGet("getHallOfFameWinner/{eventid}/{period}")]
        public string GetHallOfFameWinner(int eventid, string period)
        {
            return JsonConvert.SerializeObject(_INominee.GetHallOfFameWinner(eventid, period));
        }

        [HttpGet("getAllEmail")]
        public List<QueryEmail> GetEmployeeEmails()
        {
            return _IDetails.GetEmployeeEmails();
        }

        [HttpGet("getAllManagerEmails")]
        public List<QueryEmail> GetAllManagerEmails()
        {
            return _IDetails.GetAllManagerEmails();
        }


        [HttpGet("getAllWinnersEmails")]
        public List<QueryEmail> GetWinnersEmails()
        {
            return _IDetails.GetWinnersEmails();
        }

        [HttpGet("getTestEmails")]
        public List<string> GetTestEmails()
        {
            return _IDetails.GetTestEmails();
        }

        [HttpGet("getAllEmailList")]
        public List<string> GetAllEmployeeEmails()
        {
            return _IDetails.GetAllEmployeeEmails();
        }

        [HttpGet("getAllManagerEmailsList")]
        public List<string> GetAllManagerEmailsList()
        {
            return _IDetails.GetAllManagerEmailsList();
        }
        [HttpGet("getAllWinnersEmailsList")]
        public List<string> GetWinnersEmailsList()
        {
            return _IDetails.GetWinnersEmailsList();
        }

        [HttpGet("checkEmailSentForCOTMEvent/{period}")]
        public bool CheckEmailSentForCOTMEvent(string period)
        {
            return _IMail.CheckEmailSentForCOTMEvent(period);
        }


        [HttpGet("checkEmailSentForShortlistPeriod/{period}")]
        public bool CheckEmailSentForShortlistPeriod(string period)
        {
            return _IMail.CheckEmailSentForShortlistPeriod(period);
        }

        #region Voucher
        [HttpGet("showVoucher/{nomineePayrollId}/{period}/{eventid}")]
        public bool showVoucher(int nomineePayrollId, string period, int eventId)
        {
            bool saveOk = false;
            try
            {
                saveOk = _IVoucher.showVoucher(nomineePayrollId, period, eventId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return saveOk;
        }
        #endregion
    }
}
