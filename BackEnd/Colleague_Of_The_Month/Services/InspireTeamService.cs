using Colleague_Of_The_Month.Interfaces;
using Colleague_Of_The_Month.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Services
{
    public class InspireTeamService : IInspireTeam
    {

        private readonly COTMDBContext _context;

        public InspireTeamService(COTMDBContext context)
        {
            _context = context;
        }

        #region Inspire Team
        public void CreateInspire(InspireTeam InspireTeamModel)
        {
            _context.Add(InspireTeamModel);
            _context.SaveChanges();
        }

        // get overall nominees for Inspire Team
        //public Dictionary<string, List<InspireTeam>> GetNomineesInspireTeam(int eventid, string period)
        //{
        //    var nomi = _context.InspireTeam
        //                .AsEnumerable()
        //                .Where(n => n.EventId == eventid && n.Period == period)
        //                .GroupBy(n => n.TeamName)
        //                .ToDictionary(g => g.Key, g => g.ToList());

        //    return nomi;
        //}



        public List<QueryInspire> GetNomineesInspireTeam(int eventid, string period)
        {
            var nomi = _context.InspireTeam
                        .AsEnumerable()
                        .Where(n => n.EventId == eventid && n.Period == period)
                        .GroupBy(n => n.TeamName)
                        .ToDictionary(g => g.Key, g => g.ToList());

            var moi = new List<QueryInspire>();

            foreach (KeyValuePair<string, List<InspireTeam>> entry in nomi)
            {
                QueryInspire qe = new QueryInspire();
                //qe.teamName = sl.GetEmployeesDetails(entry.Key, _context);
                qe.nominations = entry.Value;
                qe.teamName = entry.Key;
                moi.Add(qe);
            }



            return moi;
        }



        #endregion
    }
}
