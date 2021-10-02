using Colleague_Of_The_Month.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Services
{
    public interface IVotes
    {
        List<VoteLog> GetVoteLogs(int sessionId, int eventId, string period);

        VoteLogNominees GetVoteLog(int sessionId, int eventId, int managerId, string period);
        bool Vote(List<VoteLog> vl);
        Task<List<VotingSession>> GetVotingSessions();
        //bool CreateVotingSession(VotingSession vs);
        //bool UpdateVotingSession(int sessionId, bool status);

        bool CreateVotingSession(VotingSession vs);

        List<QueryEvent> GetEventNameDescrip();

        void CloseVotingSession(int sessionId);

        bool EditVotingSession(int sessionId, VotingSession VotingSessionModel);

        VotingSession GetEditSession(int sessionId);

        void StatusClosingDate();

        bool GetActiveCOTMEvent();
        int GetSessionIDofActiveCOTMEvent();
        string GetOpeningDateOfCOTMEvent();
        string GetClosingDateOfCOTMEvent();

        string GetClosingDateOfCOTMVotingSession();
        string GetOpeningDateOfCOTMVotingSession();

        string GetClosingDateOfCOTMShortlistPeriod();

        void VotelogToNominees();
    }
}
