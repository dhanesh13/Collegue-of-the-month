using Colleague_Of_The_Month.Interfaces;
using Colleague_Of_The_Month.Models;
using Colleague_Of_The_Month.Email;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Services
{
    public class COTMService : INominee,IDetails ,IDateAccess, IBusinessUnit, ICostCentre, IDepartment, IDivision,
                                IEmployee, IEvent, ISubdivision, IVoucher,IMail
    {
        private readonly COTMDBContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IDetails _IDetails;

        public COTMService(COTMDBContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
          
           

        }

        SharedLibrary sl = new SharedLibrary();

        #region generic
        public async Task<List<Division>> GetDivisions()
        {
            return await _context.Division.ToListAsync();
        }

        public async Task<List<Subdivision>> GetSubdivisions()
        {
            return await _context.Subdivision.ToListAsync();
        }

        public async Task<List<BusinessUnit>> GetBusinessUnits()
        {
            return await _context.BusinessUnit.ToListAsync();
        }

        public async Task<List<Department>> GetDepartments()
        {
            return await _context.Department.ToListAsync();
        }
        #endregion


        #region nominate
        public bool Create(Nominee NomineeModel)
        {
            try
            {
                if (RestrictVotingToOne(NomineeModel.NomineePayrollId, NomineeModel.VoterPayrollId, NomineeModel.Period, NomineeModel.EventId) == true)
                {
                    return true;
                }
                else
                {
                    _context.Add(NomineeModel);
                    _context.SaveChanges();
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // restrict voting to one
        public bool RestrictVotingToOne(int nomineePayrollId, int voterPayrollId, string period, int eventId)
        {
            bool voted = _context.Nominees.Where(u => u.NomineePayrollId == nomineePayrollId
                                                   && u.VoterPayrollId == voterPayrollId
                                                   && u.Period == period
                                                   && u.EventId == eventId).Any();
            if (voted)
            {
                return true;
            }
            else
            {
                return false;
            }         
        }
        #endregion

        #region employee details
        public async Task<List<QueryEmployee>> GetEmployeesByDivDepMan()
        {
            var query = (from employeedetails in _context.Details
                         join emp in _context.Employee on employeedetails.EmployeeId equals emp.EmployeeId
                         join dep in _context.Department on emp.DeptId equals dep.DeptId
                         join div in _context.Division on emp.DivisionId equals div.DivisionId
                         join man in _context.Details on employeedetails.ManagerId equals man.EmployeeId
                         select new QueryEmployee
                         {
                             PayrollID = emp.PayrollId,
                             EmployeeFirstName = employeedetails.FirstName,
                             EmployeeLastName = employeedetails.LastName,
                             EmployeeFirstNameManager = man.FirstName,
                             EmployeeLastNameManager = man.LastName,
                             EmployeeDepartment = dep.Name,
                             EmployeeDivision = div.Activity
                         }).ToListAsync();
            //return await _context.EmployeeDetails.ToListAsync();
            return await query;

        }



        public QueryEmployee GetEmployeesDetailsWithoutManager(int epayrollId)
        {

            var query = (from employeedetails in _context.Details
                         join emp in _context.Employee on employeedetails.EmployeeId equals emp.EmployeeId
                         join dep in _context.Department on emp.DeptId equals dep.DeptId
                         join div in _context.Division on emp.DivisionId equals div.DivisionId
                         where emp.PayrollId == epayrollId
                         select new QueryEmployee
                         {
                             PayrollID = emp.PayrollId,
                             EmployeeFirstName = employeedetails.FirstName,
                             EmployeeLastName = employeedetails.LastName,
                             EmployeeDepartment = dep.Name,
                             EmployeeDivision = div.Activity

                         }).FirstOrDefault();
            //return await _context.EmployeeDetails.ToListAsync();
            return query;

        }
        public async Task<List<Details>> GetEmployeeDetails()
        {
            return await _context.Details.ToListAsync();
        }

        public Employee GetEmployees(int payrollid)
        {
            return _context.Employee.Where(e => e.PayrollId == payrollid).FirstOrDefault();
        }

        #endregion

        #region emails

        public void UpdateMailSentWhenCOTMOpen(string period)
        {
            (from p in _context.Mail where p.Period == period select p).ToList().ForEach(x => x.MailSent = true);
            _context.SaveChanges();
        }
        public bool CheckEmailSentForCOTMEvent(string period)
        {
            bool exist = _context.Mail.Where(u => u.Period == period).Any();
            if (exist)
            {
                bool mailSent = _context.Mail.Where(u => u.Period == period && u.MailSent == true).Any();
                if (mailSent)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Mail newMail = new Mail();
                newMail.Period = period;
                newMail.MailSent = false;
                newMail.MailSentManagers = false;
                newMail.MailSentManagersShortlistPeriod = false;
                newMail.MailSentWinners = false;
                newMail.MailSentVouchers = false;
                _context.Add(newMail);
                _context.SaveChanges();
                return false;

            }
        }
        public void UpdateMailSentWhenShortlistPeriodOpen(string period)
        {
            (from p in _context.Mail where p.Period == period select p).ToList().ForEach(x => x.MailSentManagersShortlistPeriod = true);
            _context.SaveChanges();
        }
        public bool CheckEmailSentForShortlistPeriod(string period)
        {

            bool mailSent = _context.Mail.Where(u => u.Period == period && u.MailSentManagersShortlistPeriod == true).Any();
            if (mailSent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<QueryEmail> GetEmployeeEmails()
        {
            var query = (from employeedetails in _context.Details


                         select new QueryEmail
                         {

                             Email = employeedetails.EmailAddress

                         }).ToList();

            return query;
        }
        public List<QueryEmail> GetAllManagerEmails()
        {
            var query = (from employeedetails in _context.Details
                         join mdetail in _context.Details on employeedetails.EmployeeId equals mdetail.ManagerId

                         select new QueryEmail
                         {

                             Email = employeedetails.EmailAddress

                         }).Distinct().ToList();

            return query;
        }
        public List<QueryEmail> GetWinnersEmails()
        {
            var query = (from employee in _context.Employee
                         join det in _context.Details on employee.EmployeeId equals det.EmployeeId
                         join nom in _context.Nominees on employee.PayrollId equals nom.NomineePayrollId
                         where nom.Winner == true


                         select new QueryEmail
                         {

                             Email = det.EmailAddress

                         }).Distinct().ToList();

            return query;
        }
        public List<string> GetTestEmails()
        {
            var query = new List<string>();
           // query.Add("tanzila.purrahoo@sdworx.com");
           // query.Add("hibah.khodabocus@sdworx.com");
            //query.Add("poovanen.seenan@sdworx.com");
           query.Add("fawwaaz.koodruth@sdworx.com");
            //query.Add("sameer.boodhoo@sdworx.com");
            //query.Add("viswamithe.ramessur@sdworx.com");




            return query;
        }
        public List<string> GetAllEmployeeEmails()
        {
            var emails = (from employeedetails in _context.Details


                          select new QueryEmail
                          {

                              Email = employeedetails.EmailAddress

                          }).ToList();
            var query = new List<string>();
            foreach (QueryEmail element in emails)
            {
                query.Add(element.Email);

            }

            return query;
        }

        public List<string> GetAllManagerEmailsList()
        {
            var emails = (from employeedetails in _context.Details
                          join mdetail in _context.Details on employeedetails.EmployeeId equals mdetail.ManagerId

                          select new QueryEmail
                          {

                              Email = employeedetails.EmailAddress

                          }).Distinct().ToList();

            var query = new List<string>();
            foreach (QueryEmail element in emails)
            {
                query.Add(element.Email);

            }
            return query;
        }
        public List<string> GetWinnersEmailsList()
        {
            string period = GetPeriod();
            var emails = (from employee in _context.Employee
                          join det in _context.Details on employee.EmployeeId equals det.EmployeeId
                          join nom in _context.Nominees on employee.PayrollId equals nom.NomineePayrollId
                          where nom.Winner == true && nom.Period == period && nom.ManagersVotes > 0


                          select new QueryEmail
                          {

                              Email = det.EmailAddress

                          }).Distinct().ToList();
            var query = new List<string>();
            foreach (QueryEmail element in emails)
            {
                query.Add(element.Email);

            }

            return query;
        }
        #endregion

        #region date
        public bool DateAccess()
        {
            int rangeDay = 7;
            int rangeYear = 1;
            DateTime validDateVote = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            DateTime beginDate;
            DateTime endDate;
            int lastDays = DateTime.DaysInMonth(validDateVote.Year, validDateVote.Month);
            int lastMonth = validDateVote.Month;
            int lastYear = validDateVote.Year;

            if (validDateVote.Day <= rangeDay)
            {
                lastDays = DateTime.DaysInMonth(validDateVote.Year, validDateVote.AddMonths(-1).Month);
                lastMonth = validDateVote.AddMonths(-1).Month;
            }

            if (validDateVote.Month <= rangeYear && validDateVote.Day <= rangeDay)
            {
                lastYear = validDateVote.AddYears(-1).Year;
            }

            beginDate = new DateTime(lastYear, lastMonth, lastDays).AddDays(-6);
            endDate = new DateTime(lastYear, lastMonth, lastDays).AddDays(7);

            int datediff = (endDate - beginDate).Days + 1;
            var lastWednesdayOfTheMonth = Enumerable.Range(0, datediff)
                .Select(numDay => beginDate.AddDays(numDay))
                .Where(day => day.DayOfWeek == DayOfWeek.Wednesday)
                .GroupBy(day => day.Month)
                .Select(grp => grp.Max(day => day));

            var firstWed = lastWednesdayOfTheMonth.ElementAt(0);
            var lastWed = lastWednesdayOfTheMonth.ElementAt(1);

            if (validDateVote >= firstWed && validDateVote <= lastWed)
                return true;

            return false;
        }

        public bool DateAccessShortlist()
        {
            int rangeDay = 7;
            int rangeYear = 1;
            DateTime validDateVote = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            DateTime beginDate;
            DateTime endDate;
            int lastDays = DateTime.DaysInMonth(validDateVote.Year, validDateVote.Month);
            int lastMonth = validDateVote.Month;
            int lastYear = validDateVote.Year;

            if (validDateVote.Day <= rangeDay)
            {
                lastDays = DateTime.DaysInMonth(validDateVote.Year, validDateVote.AddMonths(-1).Month);
                lastMonth = validDateVote.AddMonths(-1).Month;
            }

            if (validDateVote.Month <= rangeYear && validDateVote.Day <= rangeDay)
            {
                lastYear = validDateVote.AddYears(-1).Year;
            }

            beginDate = new DateTime(lastYear, lastMonth, lastDays).AddDays(-6);
            endDate = new DateTime(lastYear, lastMonth, lastDays).AddDays(7);

            int datediff = (endDate - beginDate).Days + 1;
            var lastWednesdayOfTheMonth = Enumerable.Range(0, datediff)
                .Select(numDay => beginDate.AddDays(numDay))
                .Where(day => day.DayOfWeek == DayOfWeek.Wednesday)
                .GroupBy(day => day.Month)
                .Select(grp => grp.Max(day => day));
            var firstWed = lastWednesdayOfTheMonth.ElementAt(0);
            var lastWed = lastWednesdayOfTheMonth.ElementAt(1);


            var firstShorlistDay = lastWed.AddDays(1);
            var lastShorlistDay = firstShorlistDay.AddDays(6);
            // DateTime validDateVote1 = lastWed.AddDays(2);
            //if (validDateVote >= firstShorlistDay && validDateVote <= lastShorlistDay)
            //    return true;

            return false;
        }

        public string GetPeriod()
        {
            if (DateAccess() == false)
            {
                var dateNow = DateTime.Now;
                var lastYear = dateNow.Year;
                if (dateNow.Month <= 1)
                {
                    lastYear = dateNow.AddYears(-1).Year;
                }
                var periodDate = new DateTime(lastYear, dateNow.AddMonths(-1).Month, dateNow.Day, 0, 0, 0);
                var period = periodDate.ToString("MMMMyyyy");
                return period;
            }
            else
            {
                int rangeDay = 7;
                int rangeYear = 1;
                DateTime validDateVote = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                DateTime beginDate;
                DateTime endDate;
                int lastDays = DateTime.DaysInMonth(validDateVote.Year, validDateVote.Month);
                int lastMonth = validDateVote.Month;
                int lastYear = validDateVote.Year;

                if (validDateVote.Day <= rangeDay)
                {
                    lastDays = DateTime.DaysInMonth(validDateVote.Year, validDateVote.AddMonths(-1).Month);
                    lastMonth = validDateVote.AddMonths(-1).Month;
                }

                if (validDateVote.Month <= rangeYear && validDateVote.Day <= rangeDay)
                {
                    lastYear = validDateVote.AddYears(-1).Year;
                }

                beginDate = new DateTime(lastYear, lastMonth, lastDays).AddDays(-6);
                endDate = new DateTime(lastYear, lastMonth, lastDays).AddDays(7);

                int datediff = (endDate - beginDate).Days + 1;
                var lastWednesdayOfTheMonth = Enumerable.Range(0, datediff)
                    .Select(numDay => beginDate.AddDays(numDay))
                    .Where(day => day.DayOfWeek == DayOfWeek.Wednesday)
                    .GroupBy(day => day.Month)
                    .Select(grp => grp.Max(day => day));

                var firstWed = lastWednesdayOfTheMonth.ElementAt(0);
                var lastWed = lastWednesdayOfTheMonth.ElementAt(1);

                var year = validDateVote.Year;
                if (validDateVote.Month <= 1)
                {
                    year = validDateVote.AddYears(-1).Year;
                }
                var periodDate = new DateTime(year, firstWed.Month, validDateVote.Day, 0, 0, 0);
                var period = periodDate.ToString("MMMMyyyy");
                return period;
            }
        }


        public string GetOpenPeriod()
        {
            var date = System.DateTime.Now;
            var lastDayOfMonth = DateTime.DaysInMonth(date.Year, date.Month);
            var openingDate = new DateTime(date.Year, date.Month, lastDayOfMonth, 0, 0, 0).AddDays(-6);



            var period = openingDate.ToString("MMMyyyy");



            return period;
        }

        #endregion

        #region nominees
        public List<QueryNominee> GetNominees(int eventid, string period)
        {
            var nomi = _context.Nominees
                        .AsEnumerable()
                        .Where(n => n.EventId == eventid && n.Period == period)
                        .GroupBy(n => n.NomineePayrollId)
                        .ToDictionary(g => g.Key, g => g.ToList());

            var moi = new List<QueryNominee>();

            foreach (KeyValuePair<int, List<Nominee>> entry in nomi)
            {
                QueryNominee qe = new QueryNominee();
                qe.query = sl.GetEmployeesDetails(entry.Key, _context);
                qe.nominations = entry.Value;
                qe.nominationsCount = entry.Value.Count();
                qe.employeeId = entry.Key;
                moi.Add(qe);
            }

            return moi;
        }

        public List<QueryNominee> GetNominees(int eventid, int take, string period)
        {
            Dictionary<int, List<Nominee>> nomi;

            nomi = _context.Nominees
                    .AsEnumerable()
                    .Where(n => n.EventId == eventid && n.Period == period)
                    .GroupBy(n => n.NomineePayrollId)
                    .OrderByDescending(n => n.Count())
                    .Take(take)
                    .ToDictionary(g => g.Key, g => g.ToList());

            //var moi = new Dictionary<int, QueryNominee>();
            var toi = new List<QueryNominee>();

            foreach (KeyValuePair<int, List<Nominee>> entry in nomi)
            {
                QueryNominee qe = new QueryNominee();
                qe.employeeId = entry.Key;
                qe.query = sl.GetEmployeesDetails(entry.Key, _context);
                qe.nominations = entry.Value;
                qe.nominationsCount = entry.Value.Count();
                qe.employeeId = entry.Key;

                toi.Add(qe);

                //moi.Add(entry.Key, qe);
            }

            return toi;
        }

        public List<QueryNominee> GetNominees(int eventid, int take, int managerId, string period)
        {
            Dictionary<int, List<Nominee>> nomi;
            var mid = _context.Employee.Where(e => e.PayrollId == managerId).FirstOrDefault().EmployeeId;
            var xxx = (from n in _context.Nominees
                       join e in _context.Employee on n.NomineePayrollId equals e.PayrollId
                       join d in _context.Details on e.EmployeeId equals d.EmployeeId
                       where d.ManagerId.Equals(mid)
                       select n).ToList();

            nomi = xxx
                    .Where(n => n.EventId == eventid && n.Period == period)
                    .GroupBy(n => n.NomineePayrollId)
                    .OrderByDescending(n => n.Count())
                    .Take(take)
                    .ToDictionary(g => g.Key, g => g.ToList());

            var toi = new List<QueryNominee>();

            foreach (KeyValuePair<int, List<Nominee>> entry in nomi)
            {
                QueryNominee qe = new QueryNominee();
                qe.query = sl.GetEmployeesDetails(entry.Key, _context);
                qe.nominations = entry.Value;
                qe.nominationsCount = entry.Value.Count();
                qe.employeeId = entry.Key;
                List<int> invalidVoters = new List<int>();

                foreach (Nominee nominee in qe.nominations)
                {
                    string vName, vLastName = string.Empty;
                    QueryEmployee vqq = new QueryEmployee();
                    vqq = GetEmployeesDetailsWithoutManager(nominee.VoterPayrollId);

                    if (vqq == null)
                    {
                        invalidVoters.Add(nominee.VoterPayrollId);
                    }
                    else
                    {
                        vName = vqq.EmployeeFirstName;
                        vLastName = vqq.EmployeeLastName;
                        nominee.VotersFullName = vName + " " + vLastName;
                    }

                }

                toi.Add(qe);
            }

            return toi;
        }

        public List<QueryNominee> GetNominationByNomineePayrollId(int eventid, int nominationPayrollId, string period)
        {
            var nomi = _context.Nominees
                        .AsEnumerable()
                        .Where(n => n.EventId == eventid && n.NomineePayrollId == nominationPayrollId && n.Period == period)
                        .GroupBy(n => n.NomineePayrollId)
                        .ToDictionary(g => g.Key, g => g.ToList());

            var moi = new List<QueryNominee>();

            foreach (KeyValuePair<int, List<Nominee>> entry in nomi)
            {
                QueryNominee qe = new QueryNominee();
                qe.query = sl.GetEmployeesDetails(entry.Key, _context);
                qe.nominations = entry.Value;
                qe.nominationsCount = entry.Value.Count();
                qe.employeeId = entry.Key;

                moi.Add(qe);
            }

            return moi;
        }

        #endregion

        #region votes

        public List<QueryNominee> AdminGetVotedNominees(int eventid, string period)
        {
            Dictionary<int, List<Nominee>> nomi;

            nomi = _context.Nominees
                    .AsEnumerable()
                    .Where(n => n.EventId == eventid && n.Shortlisted == true && n.Period == period && n.ManagersVotes > 0)
                    .OrderByDescending(n => n.ManagersVotes)
                    .GroupBy(n => n.NomineePayrollId)
                    .ToDictionary(g => g.Key, g => g.ToList());

            var moi = new List<QueryNominee>();

            if (nomi.Count > 0)
            {
                foreach (KeyValuePair<int, List<Nominee>> e in nomi)
                {
                    QueryNominee qe = new QueryNominee();
                    qe.query = sl.GetEmployeesDetails(e.Key, _context);
                    qe.managerVotesCount = e.Value[0].ManagersVotes;
                    qe.nominationsCount = e.Value.Count;
                    qe.nominations = e.Value;
                    qe.employeeId = e.Key;

                    moi.Add(qe);

                }
            }
            return moi;
        }



        public List<QueryNominee> AdminGetVotedNominees(int eventid, int take, string period)
        {
            Dictionary<int, int> nomi;

            nomi = _context.Nominees
                    .AsEnumerable()
                    .Where(n => n.EventId == eventid && n.Shortlisted == true && n.Period == period && n.ManagersVotes > 0)
                    .OrderByDescending(n => n.ManagersVotes)
                    .GroupBy(n => n.NomineePayrollId)
                    .Take(take)
                    .ToDictionary(g => g.Key, g => g.Count());

            var xxxx = _context.Nominees
                    .AsEnumerable()
                    .Where(n => n.EventId == eventid && n.Shortlisted == true && n.Period == period)
                    .OrderByDescending(n => n.ManagersVotes)
                    .GroupBy(n => n.NomineePayrollId)
                    .Take(take)
                    .ToDictionary(g => g.Key, g => g.ToList());

            var moi = new List<QueryNominee>();

            foreach (KeyValuePair<int, List<Nominee>> entry in xxxx)
            {
                QueryNominee qe = new QueryNominee();
                qe.query = sl.GetEmployeesDetails(entry.Key, _context);
                qe.managerVotesCount = nomi.Where(n => n.Key == entry.Key).FirstOrDefault().Value;
                qe.nominationsCount = entry.Value.Count;
                qe.nominations = entry.Value;
                qe.employeeId = entry.Key;

                moi.Add(qe);
            }

            return moi;
        }


        #endregion

        #region generic

        public async Task<List<CostCentre>> GetCostCentres()
        {
            return await _context.CostCentre.ToListAsync();
        }

        public async Task<List<Employee>> GetEmployees()
        {
            return await _context.Employee.ToListAsync();
        }

        public async Task<List<Event>> GetEvents()
        {
            return await _context.Event.ToListAsync();
        }

        public async Task<List<Voucher>> GetVouchers()
        {
            return await _context.Voucher.ToListAsync();
        }

        #endregion

        #region shortlist/nonshortlist

        public void ShortlistNominee(int eventid, int PayrollID, string period)
        {
            var shortlistNominee = _context.Nominees.Where(element =>
            element.NomineePayrollId == PayrollID &&
            element.EventId == eventid && element.Period == period).ToList();

            shortlistNominee.ForEach((element) =>
          element.Shortlisted = true
          );
            _context.SaveChanges();
        }

        public void ShortlistNomineeWithRationale(int eventid, int PayrollID, string rationale, string period)
        {
            var shortlistNominee = _context.Nominees.Where(element =>
            element.NomineePayrollId == PayrollID &&
            element.EventId == eventid && element.Period == period).ToList();

            shortlistNominee.ForEach((element) =>
            element.AdminRationale = rationale
            );
            shortlistNominee.ForEach((element) =>
            element.Shortlisted = true
            );

            _context.SaveChanges();
        }
        public void NomineeRejectionRationaleByManager(int eventid, int PayrollID, string rationale, string period)
        {
            var shortlistNominee = _context.Nominees.Where(element =>
            element.NomineePayrollId == PayrollID &&
            element.EventId == eventid && element.Period == period).ToList();

            shortlistNominee.ForEach((element) =>
            element.ManagersRationale = rationale
            );


            _context.SaveChanges();
        }
        public void ShortlistNomineeWithManagerRationale(int eventid, int PayrollID, string rationale, string period)
        {
            var shortlistNominee = _context.Nominees.Where(element =>
            element.NomineePayrollId == PayrollID &&
            element.EventId == eventid && element.Period == period).ToList();

            shortlistNominee.ForEach((element) =>
            element.ManagersRationale = rationale
            );
            shortlistNominee.ForEach((element) =>
            element.Shortlisted = true
            );

            _context.SaveChanges();
        }

        public void RemoveShortlistedNominee(int eventid, int PayrollID, string period)
        {
            var shortlistNominee = _context.Nominees.Where(nomineePayrollID => nomineePayrollID.NomineePayrollId == PayrollID && nomineePayrollID.Period == period).FirstOrDefault();
            if (shortlistNominee.Shortlisted)
            {
                shortlistNominee.Shortlisted = false;
            }

            _context.SaveChanges();
        }

        public List<QueryNominee> GetNonShortListedNominees(int eventid, string period)
        {
            Dictionary<int, List<Nominee>> myDictionary = _context.Nominees.AsEnumerable()
                                                            .Where(n => n.Shortlisted == false && n.EventId == eventid && n.Period == period)
                                                            .GroupBy(n => n.NomineePayrollId)
                                                            .ToDictionary(g => g.Key, g => g.ToList());

            var moi = new List<QueryNominee>();

            foreach (KeyValuePair<int, List<Nominee>> entry in myDictionary)
            {
                QueryNominee qe = new QueryNominee();
                qe.query = sl.GetEmployeesDetails(entry.Key, _context);
                qe.nominations = entry.Value;
                qe.employeeId = entry.Key;

                moi.Add(qe);
            }

            return moi;

        }

        public List<QueryNominee> GetNonShortListedNominee(int eventid, int nomineeId, string period)
        {
            Dictionary<int, List<Nominee>> myDictionary = _context.Nominees.AsEnumerable()
                                                            .Where(n => n.Shortlisted == false && n.NomineePayrollId == nomineeId && n.EventId == eventid && n.Period == period)
                                                            .GroupBy(n => n.NomineePayrollId)
                                                            .ToDictionary(g => g.Key, g => g.ToList());

            var moi = new List<QueryNominee>();

            foreach (KeyValuePair<int, List<Nominee>> entry in myDictionary)
            {
                QueryNominee qe = new QueryNominee();
                qe.query = sl.GetEmployeesDetails(entry.Key, _context);
                qe.nominations = entry.Value;
                qe.employeeId = entry.Key;

                moi.Add(qe);
            }

            return moi;
        }

        public List<QueryNominee> GetNonShortListedNominees(int eventid, int take, string period)
        {
            Dictionary<int, List<Nominee>> myDictionary;
            switch (take)
            {
                case 0:
                    myDictionary = _context.Nominees.AsEnumerable()
                                    .Where(n => n.Shortlisted == false && n.EventId == eventid && n.Period == period)
                                    .GroupBy(n => n.NomineePayrollId)
                                    .ToDictionary(g => g.Key, g => g.ToList());
                    break;
                default:
                    myDictionary = _context.Nominees.AsEnumerable()
                                    .Where(n => n.Shortlisted == false && n.EventId == eventid && n.Period == period)
                                    .GroupBy(n => n.NomineePayrollId)
                                    .OrderByDescending(n => n.Count())
                                    .Take(take)
                                    .ToDictionary(g => g.Key, g => g.ToList());
                    break;
            }

            var moi = new List<QueryNominee>();

            foreach (KeyValuePair<int, List<Nominee>> entry in myDictionary)
            {
                QueryNominee qe = new QueryNominee();
                qe.query = sl.GetEmployeesDetails(entry.Key, _context);
                qe.nominations = entry.Value;
                qe.employeeId = entry.Key;
                qe.nominationsCount = entry.Value.Count();


                List<int> invalidVoters = new List<int>();

                foreach (Nominee nominee in qe.nominations)
                {
                    string vName, vLastName = string.Empty;
                    QueryEmployee vqq = new QueryEmployee();
                    vqq = GetEmployeesDetailsWithoutManager(nominee.VoterPayrollId);

                    if (vqq == null)
                    {
                        invalidVoters.Add(nominee.VoterPayrollId);
                    }
                    else
                    {
                        vName = vqq.EmployeeFirstName;
                        vLastName = vqq.EmployeeLastName;
                        nominee.VotersFullName = vName + " " + vLastName;
                    }

                }

                moi.Add(qe);
            }


            return moi;
        }

        public List<QueryNominee> GetRejectedNomineesByManager(int eventid, int managerId, string period)
        {
            Dictionary<int, List<Nominee>> nomi;
            var mid = _context.Employee.Where(e => e.PayrollId == managerId).FirstOrDefault().EmployeeId;
            var xxx = (from n in _context.Nominees
                       join e in _context.Employee on n.NomineePayrollId equals e.PayrollId
                       join d in _context.Details on e.EmployeeId equals d.EmployeeId
                       where d.ManagerId.Equals(mid)
                       select n).ToList();

            nomi = xxx
                    .Where(n => n.EventId == eventid && n.Shortlisted == false && n.Period == period)
                    .GroupBy(n => n.NomineePayrollId)
                    .OrderByDescending(n => n.Count())
                    .ToDictionary(g => g.Key, g => g.ToList());

            var moi = new List<QueryNominee>();

            foreach (KeyValuePair<int, List<Nominee>> entry in nomi)
            {
                QueryNominee qe = new QueryNominee();
                qe.query = sl.GetEmployeesDetails(entry.Key, _context);
                qe.nominations = entry.Value;
                qe.nominationsCount = entry.Value.Count();

                List<int> invalidVoters = new List<int>();

                foreach (Nominee nominee in qe.nominations)
                {
                    string vName, vLastName = string.Empty;
                    QueryEmployee vqq = new QueryEmployee();
                    vqq = GetEmployeesDetailsWithoutManager(nominee.VoterPayrollId);

                    if (vqq == null)
                    {
                        invalidVoters.Add(nominee.VoterPayrollId);
                    }
                    else
                    {
                        vName = vqq.EmployeeFirstName;
                        vLastName = vqq.EmployeeLastName;
                        nominee.VotersFullName = vName + " " + vLastName;
                    }

                }
                qe.employeeId = entry.Key;

                moi.Add(qe);
            }

            return moi;
        }

        // top five - second card for manager
        public List<QueryNominee> GetTopFiveRejectedNomineesByManager(int eventid, int take, int managerId, string period)
        {
            Dictionary<int, List<Nominee>> nomi;
            var mid = _context.Employee.Where(e => e.PayrollId == managerId).FirstOrDefault().EmployeeId;
            var xxx = (from n in _context.Nominees
                       join e in _context.Employee on n.NomineePayrollId equals e.PayrollId
                       join d in _context.Details on e.EmployeeId equals d.EmployeeId
                       where d.ManagerId.Equals(mid)
                       select n).ToList();

            nomi = xxx
                    .Where(n => n.EventId == eventid && n.Shortlisted == false && n.Period == period)
                    .GroupBy(n => n.NomineePayrollId)
                    .OrderByDescending(n => n.Count())
                    .Take(take)
                    .ToDictionary(g => g.Key, g => g.ToList());

            var moi = new List<QueryNominee>();

            foreach (KeyValuePair<int, List<Nominee>> entry in nomi)
            {
                QueryNominee qe = new QueryNominee();
                qe.query = sl.GetEmployeesDetails(entry.Key, _context);
                qe.nominations = entry.Value;
                qe.nominationsCount = entry.Value.Count();

                List<int> invalidVoters = new List<int>();

                foreach (Nominee nominee in qe.nominations)
                {
                    string vName, vLastName = string.Empty;
                    QueryEmployee vqq = new QueryEmployee();
                    vqq = GetEmployeesDetailsWithoutManager(nominee.VoterPayrollId);

                    if (vqq == null)
                    {
                        invalidVoters.Add(nominee.VoterPayrollId);
                    }
                    else
                    {
                        vName = vqq.EmployeeFirstName;
                        vLastName = vqq.EmployeeLastName;
                        nominee.VotersFullName = vName + " " + vLastName;
                    }

                }
                qe.employeeId = entry.Key;

                moi.Add(qe);
            }

            return moi;
        }

        public List<QueryNominee> GetShortListedNominees(int eventid, string period)
        {
            Dictionary<int, List<Nominee>> myDictionary = _context.Nominees.AsEnumerable()
                                                            .Where(n => n.Shortlisted == true && n.EventId == eventid && n.Period == period)
                                                            .GroupBy(n => n.NomineePayrollId)
                                                            .ToDictionary(g => g.Key, g => g.ToList());

            var moi = new List<QueryNominee>();

            foreach (KeyValuePair<int, List<Nominee>> entry in myDictionary)
            {
                QueryNominee qe = new QueryNominee();
                qe.query = sl.GetEmployeesDetails(entry.Key, _context);
                qe.nominations = entry.Value;

                List<int> invalidVoters = new List<int>();

                foreach (Nominee nominee in qe.nominations)
                {
                    string vName, vLastName = string.Empty;
                    QueryEmployee vqq = new QueryEmployee();
                    vqq = GetEmployeesDetailsWithoutManager(nominee.VoterPayrollId);

                    if (vqq == null)
                    {
                        invalidVoters.Add(nominee.VoterPayrollId);
                    }
                    else
                    {
                        vName = vqq.EmployeeFirstName;
                        vLastName = vqq.EmployeeLastName;
                        nominee.VotersFullName = vName + " " + vLastName;
                    }

                }



                qe.employeeId = entry.Key;

                moi.Add(qe);
            }

            return moi;
        }

        public List<QueryNominee> GetShortListedNominee(int eventid, int nomineeId, string period)

        {
            Dictionary<int, List<Nominee>> myDictionary = _context.Nominees.AsEnumerable()
                                                            .Where(n => n.Shortlisted == true && n.EventId == eventid && n.NomineePayrollId == nomineeId && n.Period == period)
                                                            .GroupBy(n => n.NomineePayrollId)
                                                            .ToDictionary(g => g.Key, g => g.ToList());


            var moi = new List<QueryNominee>();

            foreach (KeyValuePair<int, List<Nominee>> entry in myDictionary)
            {
                QueryNominee qe = new QueryNominee();
                qe.query = sl.GetEmployeesDetails(entry.Key, _context);
                qe.nominations = entry.Value;

                List<int> invalidVoters = new List<int>();

                foreach (Nominee nominee in qe.nominations)
                {
                    string vName, vLastName = string.Empty;
                    QueryEmployee vqq = new QueryEmployee();
                    vqq = GetEmployeesDetailsWithoutManager(nominee.VoterPayrollId);

                    if (vqq == null)
                    {
                        invalidVoters.Add(nominee.VoterPayrollId);
                    }
                    else
                    {
                        vName = vqq.EmployeeFirstName;
                        vLastName = vqq.EmployeeLastName;
                        nominee.VotersFullName = vName + " " + vLastName;
                    }

                }



                qe.employeeId = entry.Key;

                moi.Add(qe);
            }

            return moi;
        }
        public List<QueryNominee> GetShortListedNomineeByNomineePayrollID(int eventid, int nomineeId, string period)

        {
            Dictionary<int, List<Nominee>> myDictionary = _context.Nominees.AsEnumerable()
                                                            .Where(n => n.Shortlisted == true && n.EventId == eventid && n.NomineePayrollId == nomineeId && n.Period == period)
                                                            .GroupBy(n => n.NomineePayrollId)
                                                            .ToDictionary(g => g.Key, g => g.ToList());


            var moi = new List<QueryNominee>();

            foreach (KeyValuePair<int, List<Nominee>> entry in myDictionary)
            {
                QueryNominee qe = new QueryNominee();
                qe.query = sl.GetEmployeesDetails(entry.Key, _context);
                qe.nominations = entry.Value;
                qe.nominationsCount = entry.Value.Count();

                List<int> invalidVoters = new List<int>();

                foreach (Nominee nominee in qe.nominations)
                {
                    string vName, vLastName = string.Empty;
                    QueryEmployee vqq = new QueryEmployee();
                    vqq = GetEmployeesDetailsWithoutManager(nominee.VoterPayrollId);

                    if (vqq == null)
                    {
                        invalidVoters.Add(nominee.VoterPayrollId);
                    }
                    else
                    {
                        vName = vqq.EmployeeFirstName;
                        vLastName = vqq.EmployeeLastName;
                        nominee.VotersFullName = vName + " " + vLastName;
                    }

                }



                qe.employeeId = entry.Key;

                moi.Add(qe);
            }

            return moi;
        }

        public List<QueryNominee> GetShortListedNominees(int eventid, int take, string period)
        {
            Dictionary<int, List<Nominee>> myDictionary;
            switch (take)
            {
                case 0:
                    myDictionary = _context.Nominees.AsEnumerable()
                                    .Where(n => n.Shortlisted == true && n.EventId == eventid && n.Period == period)
                                    .GroupBy(n => n.NomineePayrollId)
                                    .ToDictionary(g => g.Key, g => g.ToList());
                    break;
                default:
                    myDictionary = _context.Nominees.AsEnumerable()
                                    .Where(n => n.Shortlisted == true && n.EventId == eventid && n.Period == period)
                                    .GroupBy(n => n.NomineePayrollId)
                                    .OrderByDescending(n => n.Count())
                                    .Take(take)
                                    .ToDictionary(g => g.Key, g => g.ToList());
                    break;
            }

            var moi = new List<QueryNominee>();

            foreach (KeyValuePair<int, List<Nominee>> entry in myDictionary)
            {
                QueryNominee qe = new QueryNominee();
                qe.query = sl.GetEmployeesDetails(entry.Key, _context);
                qe.nominations = entry.Value;
                qe.employeeId = entry.Key;

                moi.Add(qe);
            }

            return moi;
        }

        public List<QueryNominee> GetShortListedNomineesByManager(int eventid, int take, int managerId, string period)
        {
            Dictionary<int, List<Nominee>> nomi;
            var mid = _context.Employee.Where(e => e.PayrollId == managerId).FirstOrDefault().EmployeeId;
            var xxx = (from n in _context.Nominees
                       join e in _context.Employee on n.NomineePayrollId equals e.PayrollId
                       join d in _context.Details on e.EmployeeId equals d.EmployeeId
                       where d.ManagerId.Equals(mid)
                       select n).ToList();

            nomi = xxx
                    .Where(n => n.EventId == eventid && n.Shortlisted == true && n.Period == period)
                    .GroupBy(n => n.NomineePayrollId)
                    .OrderByDescending(n => n.Count())
                    .ToDictionary(g => g.Key, g => g.ToList());

            var moi = new List<QueryNominee>();

            foreach (KeyValuePair<int, List<Nominee>> entry in nomi)
            {
                QueryNominee qe = new QueryNominee();
                qe.query = sl.GetEmployeesDetails(entry.Key, _context);
                qe.nominations = entry.Value;
                qe.nominationsCount = entry.Value.Count();

                List<int> invalidVoters = new List<int>();

                foreach (Nominee nominee in qe.nominations)
                {
                    string vName, vLastName = string.Empty;
                    QueryEmployee vqq = new QueryEmployee();
                    vqq = GetEmployeesDetailsWithoutManager(nominee.VoterPayrollId);

                    if (vqq == null)
                    {
                        invalidVoters.Add(nominee.VoterPayrollId);
                    }
                    else
                    {
                        vName = vqq.EmployeeFirstName;
                        vLastName = vqq.EmployeeLastName;
                        nominee.VotersFullName = vName + " " + vLastName;
                    }

                }
                qe.employeeId = entry.Key;

                moi.Add(qe);
            }

            return moi;
        }

        #endregion

        #region Winner
        public void WinnerNominee(int eventid, int PayrollID, string period)
        {
            var getNomineesWithPayrollID = _context.Nominees.Where(nomineePayrollID => nomineePayrollID.NomineePayrollId == PayrollID && nomineePayrollID.EventId == eventid && nomineePayrollID.Period == period);
            //getNomineesWithPayrollID.ForEach(c => c.Winner = true);

            foreach (Nominee nom in getNomineesWithPayrollID)
            {
                nom.Winner = true;
            }

            _context.SaveChanges();
        }

        public void DeleteWinner(int eventid, int PayrollID, string period)
        {
            var getNomineesWithPayrollID = _context.Nominees.Where(nomineePayrollID => nomineePayrollID.NomineePayrollId == PayrollID && nomineePayrollID.EventId == eventid && nomineePayrollID.Period == period);
            //getNomineesWithPayrollID.ForEach(c => c.Winner = true);

            foreach (Nominee nom in getNomineesWithPayrollID)
            {
                nom.Winner = false;
            }

            _context.SaveChanges();
        }
        #endregion

        #region Hall of Fame
        public List<QueryHOFWinners> GetHallOfFameWinner(int eventid, string period)
        {
            try
            {
                var query = (from nominees in _context.Nominees
                             join employee in _context.Employee on nominees.NomineePayrollId equals employee.PayrollId
                             join empDetails in _context.Details on employee.EmployeeId equals empDetails.EmployeeId
                             where nominees.Winner == true && nominees.EventId == eventid && nominees.Period.EndsWith(period)

                             select new QueryHOFWinners
                             {
                                 PayrollId = nominees.NomineePayrollId,
                                 FirstName = empDetails.FirstName,
                                 LastName = empDetails.LastName,
                                 EventId = nominees.EventId,
                                 Period = nominees.Period,
                                 Winner = nominees.Winner,
                                 Rationale = nominees.WinnerRationale
                             }).Distinct().ToList();

                return query;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new List<QueryHOFWinners> { };
            }
        }
        #endregion

        #region Voucher
        public bool PostVoucher(QueryVoucher incomingVoucherData)
        {
            try
            {
                var checkvouchersubmission = _context.Voucher
                     .AsEnumerable()
                     .Where(v => v.EventID == incomingVoucherData.EventID && v.PayrollID == incomingVoucherData.PayrollID && v.Period == incomingVoucherData.Period && v.VoucherSubmissionCheck==true)
                     .GroupBy(v => v.PayrollID)
                     .ToDictionary(g => g.Key, g => g.ToList());
                if (checkvouchersubmission.Count() > 0)
                {
                    return false;
                }
                else
                {
                    var nomi = _context.Nominees
                        .AsEnumerable()
                        .Where(n => n.EventId == incomingVoucherData.EventID && n.NomineePayrollId == incomingVoucherData.PayrollID && n.Period == incomingVoucherData.Period)
                        .GroupBy(n => n.NomineePayrollId)
                        .ToDictionary(g => g.Key, g => g.ToList());

                    var moi = new List<Voucher>();

                    foreach (KeyValuePair<int, List<Nominee>> entry in nomi)
                    {
                        QueryEmployee qe = new QueryEmployee();
                        Voucher v = new Voucher();
                        qe = sl.GetEmployeesDetails(entry.Key, _context);

                        v.LastName = qe.EmployeeLastName;
                        v.FirstName = qe.EmployeeFirstName;
                        v.FirstNameManager = qe.EmployeeFirstNameManager;
                        v.LastNameManager = qe.EmployeeLastNameManager;
                        v.Period = incomingVoucherData.Period;
                        v.PayrollID = incomingVoucherData.PayrollID;
                        v.VoucherName = incomingVoucherData.VoucherName;
                        v.VoucherSubmissionCheck = true;
                        v.Department = qe.EmployeeDepartment;
                        v.Division = qe.EmployeeDivision;
                        v.EventID = incomingVoucherData.EventID;


                        moi.Add(v);
                    }
                   
                    Voucher myv = new Voucher();
                    foreach(var element in moi)
                    {
                        myv.PayrollID = element.PayrollID;
                        myv.VoucherName = element.VoucherName;
                        myv.EventID = element.EventID;
                        myv.Period = element.Period;
                        myv.FirstName = element.FirstName;
                        myv.LastName = element.LastName;
                        myv.FirstNameManager = element.FirstNameManager;
                        myv.LastNameManager = element.LastNameManager;
                        myv.Department = element.Department;
                        myv.Division = element.Division;
                        myv.VoucherSubmissionCheck = element.VoucherSubmissionCheck;
                       


                    }
                 




                    _context.Add(myv);
                    _context.SaveChanges();



                   

                    return true;

                }
                

               
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
        #endregion

        #region Voucher
        // show voucher if winner
        public bool showVoucher(int nomineePayrollId, string period, int eventId)
        {
            bool voucher = _context.Nominees.Where(u => u.NomineePayrollId == nomineePayrollId
                                                   && u.Period == period
                                                   && u.EventId == eventId
                                                   && u.Winner == true).Any();
            if (voucher)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

    }
}

