using Colleague_Of_The_Month.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Interfaces
{
    public interface IInspireTeam
    {
        void CreateInspire(InspireTeam InspireTeamModel);

        //Dictionary<string, List<InspireTeam>> GetNomineesInspireTeam(int eventid, string period);
        List<QueryInspire> GetNomineesInspireTeam(int eventid, string period);

    }
}
