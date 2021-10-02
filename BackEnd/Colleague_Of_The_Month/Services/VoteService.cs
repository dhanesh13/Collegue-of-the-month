using Colleague_Of_The_Month.Models;
using Colleague_Of_The_Month.Email;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Services
{
    public class VoteService : IVotes
    {
        private readonly COTMDBContext _context;
        private readonly IDateAccess _IDateAccess;
        private readonly IEmailSender _emailSender;
        private readonly IDetails _IDetails;


        public VoteService(COTMDBContext context, IDateAccess iDateAccess)
        {
            _context = context;
            _IDateAccess = iDateAccess;
        }
     

        public List<VoteLog> GetVoteLogs(int sessionId, int eventId, string period)
        {
            List<VoteLog> vlList = new List<VoteLog>();

            try
            {
                vlList = _context.VoteLog.AsEnumerable().Where(v => v.sessionid == sessionId && v.eventid == eventId && v.period == period).Select(v => v).ToList();

            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", "boule crazer: " + ex.Message);
            }

            return vlList;
        }

        public VoteLogNominees GetVoteLog(int sessionId, int eventId, int managerId, string period)
        {
            VoteLogNominees vln = new VoteLogNominees();
            COTMService cts = new COTMService(_context,_emailSender);

            try
            {

                vln.voteLogs = _context.VoteLog.AsEnumerable().Where(v => v.sessionid == sessionId && v.eventid == eventId && v.managerpayrollid == managerId).Select(v => v).ToList();
                vln.nominations = cts.GetShortListedNominees(eventId, 0, period);

            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", "boule crazer: " + ex.Message);
            }

            return vln;
        }

        public bool Vote(List<VoteLog> vl)
        {


            try
            {

                foreach (VoteLog v in vl)
                {
                    var xx = _context.VoteLog.Where(r => r.nomineepayrollid == v.nomineepayrollid && r.managerpayrollid == v.managerpayrollid
                    && r.sessionid == v.sessionid).FirstOrDefault();

                    if (null != xx)
                    {
                        _context.VoteLog.Remove(xx);
                        _context.Entry(xx).State = EntityState.Deleted;
                        _context.SaveChanges();
                    }


                }


            }
            catch (Exception ex)
            { }

            try
            {
                //await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE votelog");
                //_context.VoteLog.RemoveRange(vl);


                _context.VoteLog.AddRange(vl);
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<List<VotingSession>> GetVotingSessions()
        {
            return await _context.VotingSession.ToListAsync();
        }

        //public bool CreateVotingSession(VotingSession vs)
        //{
        //    try
        //    {
        //        _context.VotingSession.Add(vs);
        //        _context.SaveChanges();

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message);
        //        return false;
        //    }
        //}

        public bool CreateVotingSession(VotingSession vs)
        {
            try
            {
                var exist = false;

                //VotingSession clonemoi = vs;    
                vs.OpenDate = Convert.ToDateTime(vs.OpenDate);
                vs.CloseDate = Convert.ToDateTime(vs.CloseDate);

                var query = _context.VotingSession.Where(votingSession => votingSession.status == true).ToList();
                foreach (var qe in query)
                {
                    if (vs.eventid == qe.eventid)
                    {
                        exist = true;
                        break;
                    }
                }
                if (exist == true)
                {
                    return false;
                }
                else
                {
                    if (_IDateAccess.DateAccess() == false)
                    {
                        var dateNow = DateTime.Now;
                        var lastYear = dateNow.Year;
                        if (dateNow.Month <= 1)
                        {
                            lastYear = dateNow.AddYears(-1).Year;
                        }
                        //var periodDate = new DateTime(lastYear, dateNow.AddMonths(-1).Month, dateNow.Day, 0, 0, 0);
                        //var period = periodDate.ToString("MMMMyyyy");
                        vs.period = "March2021";
                        _context.Add(vs);
                        _context.SaveChanges();
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw ex;
            }

        }

        //public bool UpdateVotingSession(int sessionId, bool status)
        //{
        //    try
        //    {
        //        _context.VotingSession.Where(v => v.sessionid == sessionId).FirstOrDefault().status = status;
        //        _context.SaveChanges();

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message);
        //        return false;
        //    }
        //}

        public List<QueryEvent> GetEventNameDescrip()
        {
            var query = (from votingSession in _context.VotingSession
                         join ev in _context.Event on votingSession.eventid equals ev.EventId
                         where votingSession.status == true

                         select new QueryEvent
                         {
                             SessionId = votingSession.sessionid,
                             EventId = ev.EventId,
                             EventName = ev.Name,
                             EventDescription = ev.Description,
                             Period = votingSession.period,
                             OpeningDate = votingSession.OpenDate,
                             ClosingDate = votingSession.CloseDate,
                             Status = votingSession.status
                         }).ToList();

            return query;

        }

        public void CloseVotingSession(int sessionId)
        {
            var query = _context.VotingSession.Where(votingSession => votingSession.sessionid == sessionId).FirstOrDefault();
            query.status = false;

            _context.SaveChanges();

            VotelogToNominees();


        }

        public bool EditVotingSession(int sessionId, VotingSession VotingSessionModel)
        {
            //var query = _context.VotingSession.Where(votingSession => votingSession.sessionid == sessionId).FirstOrDefault();
            //query.OpenDate = VotingSessionModel.OpenDate;
            //query.CloseDate = VotingSessionModel.CloseDate;
            //query.eventid = VotingSessionModel.eventid;

            //_context.SaveChanges();

            try
            {
                var exist = false;

                var queryEdit = _context.VotingSession.Where(votingSession => votingSession.sessionid == sessionId).FirstOrDefault();

                var query = _context.VotingSession.Where(votingSession => votingSession.status == true).ToList();
                foreach (var qe in query)
                {
                    if ((VotingSessionModel.eventid == qe.eventid) && (queryEdit.eventid != qe.eventid))
                    {
                        exist = true;
                        break;
                    }
                }
                if (exist == true)
                {
                    return false;
                }
                else
                {
                    VotingSessionModel.OpenDate = Convert.ToDateTime(VotingSessionModel.OpenDate);
                    VotingSessionModel.CloseDate = Convert.ToDateTime(VotingSessionModel.CloseDate);
                    queryEdit.OpenDate = VotingSessionModel.OpenDate;
                    queryEdit.CloseDate = VotingSessionModel.CloseDate;
                    queryEdit.eventid = VotingSessionModel.eventid;
                    _context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw ex;
            }
        }

        public VotingSession GetEditSession(int sessionId)
        {
            var query = _context.VotingSession.Where(votingSession => votingSession.sessionid == sessionId).FirstOrDefault();

            return query;
        }



        public void StatusClosingDate()
        {
            try
            {
                var query = _context.VotingSession.Where(votingSession => votingSession.status == true).ToList();
            DateTime dateNow = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            foreach (var qe in query)
            {
                if (dateNow > qe.CloseDate)
                {
                    qe.status = false;
                    _context.SaveChanges();
                    VotelogToNominees();
                }
            }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        public bool GetActiveCOTMEvent()
        {
            var query = (from votingSession in _context.VotingSession
                         join ev in _context.Event on votingSession.eventid equals ev.EventId
                         where (votingSession.status == true && votingSession.eventid == 1)

                         select new QueryEvent
                         {
                             SessionId = votingSession.sessionid,
                             EventId = ev.EventId,
                             EventName = ev.Name,
                             EventDescription = ev.Description,
                             Period = votingSession.period,
                             OpeningDate = votingSession.OpenDate,
                             ClosingDate = votingSession.CloseDate,
                             Status = votingSession.status
                         }).ToList();

            if (query.Count() > 0)
            {
                return true;
            }
            return false;
        }
        public int GetSessionIDofActiveCOTMEvent()
        {
            int sessionid=0;
            var query = (from votingSession in _context.VotingSession
                         join ev in _context.Event on votingSession.eventid equals ev.EventId
                         where (votingSession.status == true && votingSession.eventid == 1)

                         select new QueryEvent
                         {
                             SessionId = votingSession.sessionid,
                             EventId = ev.EventId,
                             EventName = ev.Name,
                             EventDescription = ev.Description,
                             Period = votingSession.period,
                             OpeningDate = votingSession.OpenDate,
                             ClosingDate = votingSession.CloseDate,
                             Status = votingSession.status
                         }).ToList();
            query.ForEach(x => 
            sessionid=x.SessionId);

          
                return sessionid;
            
            
        }
        public string GetClosingDateOfCOTMVotingSession()
        {
            var query = (from votingSession in _context.VotingSession
                         join ev in _context.Event on votingSession.eventid equals ev.EventId
                         where (votingSession.status == true && votingSession.eventid == 1)

                         select new QueryEvent
                         {
                             SessionId = votingSession.sessionid,
                             EventId = ev.EventId,
                             EventName = ev.Name,
                             EventDescription = ev.Description,
                             Period = votingSession.period,
                             OpeningDate = votingSession.OpenDate,
                             ClosingDate = votingSession.CloseDate,
                             Status = votingSession.status
                         }).ToList();
            string closingdate = "";

            if (query.Count() > 0)
            {
                foreach (QueryEvent element in query)
                {
                    closingdate = element.ClosingDate.ToString("dd MMMM yyyy");
                }
                return closingdate;
            }
            else
            {
                return "Error";
            }
        }
        public string GetOpeningDateOfCOTMVotingSession()
        {
            var query = (from votingSession in _context.VotingSession
                         join ev in _context.Event on votingSession.eventid equals ev.EventId
                         where (votingSession.status == true && votingSession.eventid == 1)

                         select new QueryEvent
                         {
                             SessionId = votingSession.sessionid,
                             EventId = ev.EventId,
                             EventName = ev.Name,
                             EventDescription = ev.Description,
                             Period = votingSession.period,
                             OpeningDate = votingSession.OpenDate,
                             ClosingDate = votingSession.CloseDate,
                             Status = votingSession.status
                         }).ToList();
            string openingdate = "";

            if (query.Count() > 0)
            {
                foreach (QueryEvent element in query)
                {
                    openingdate = element.OpeningDate.ToString("dd MMMM yyyy");
                }
                return openingdate;
            }
            else
            {
                return "Error";
            }
        }

        public string GetOpeningDateOfCOTMEvent()
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

            return firstWed.ToString();
        }
        public string GetClosingDateOfCOTMEvent()
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

            return lastWed.ToString();
        }

        public string GetClosingDateOfCOTMShortlistPeriod()
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
            return lastShorlistDay.ToString();
        }
        public void VotelogToNominees()
        {
            //var query = (from v in _context.VoteLog
            //             where v.voted == true
            //             group v by v.nomineepayrollid into g


            //             select new QueryVotelog
            //             {

            //                 NomineePayrollid = g.Key,

            //                 TotalVote = g.Count()


            //             }).ToList();
            //return query;
            Dictionary<int, int> votelogQuery;

            votelogQuery = _context.VoteLog
                    .AsEnumerable()
                    .Where(n => n.voted == true)
                    .GroupBy(n => n.nomineepayrollid)
                    .ToDictionary(g => g.Key, g => g.Count());

            var votelogQueryList = _context.VoteLog
                    .AsEnumerable()
                    .Where(n => n.voted == true)                    
                    .GroupBy(n => n.nomineepayrollid)                    
                    .ToDictionary(g => g.Key, g => g.ToList());
            var votelogQueryFinalList = new List<QueryVotelog>();

            foreach (KeyValuePair<int, List<VoteLog>> entry in votelogQueryList)
            {
                QueryVotelog tempVotelogList = new QueryVotelog();
                tempVotelogList.TotalVote = votelogQuery.Where(n => n.Key == entry.Key).FirstOrDefault().Value;

                tempVotelogList.Period = entry.Value.Select(g => g.period).FirstOrDefault();
                tempVotelogList.NomineePayrollid = entry.Key;

                votelogQueryFinalList.Add(tempVotelogList);
            }
            var queryNominee = new List<Nominee>();
            
        
            foreach (var query in votelogQueryFinalList)
            {
                queryNominee = _context.Nominees
                           .AsEnumerable()
                           .Where(n => n.NomineePayrollId == query.NomineePayrollid && n.Period == query.Period).ToList();

                queryNominee.ForEach((element) =>
                           element.ManagersVotes = query.TotalVote
                           );



            }
             _context.SaveChanges();
            

        }
    }
}
