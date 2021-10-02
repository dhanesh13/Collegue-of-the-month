using Colleague_Of_The_Month.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Services
{
    public interface INominee
    {
        bool Create(Nominee NomineeModel);

        /// <summary>
        /// get all nominations
        /// missing period
        /// </summary>
        /// <param name="eventid"></param>
        /// <returns></returns>
        List<QueryNominee> GetNominees(int eventid, string period );

        /// <summary>
        /// get all nominations
        /// take records per parameter passed
        /// </summary>
        /// <param name="eventid"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        List<QueryNominee> GetNominees(int eventid, int take, string period );

        /// <summary>
        /// get nominations by manager
        /// take first n records passed as parameter
        /// </summary>
        /// <param name="eventid"></param>
        /// <param name="take"></param>
        /// <param name="managerId"></param>
        /// <returns></returns>
        List<QueryNominee> GetNominees(int eventid, int take, int managerId, string period );

        /// <summary>
        /// get nomination by nominationPayrollid
        /// take first n records passed as parameter
        /// </summary>
        /// <param name="eventid"></param>
        /// <param name="take"></param>
        /// <param name="managerId"></param>
        /// <returns></returns>
        List<QueryNominee> GetNominationByNomineePayrollId(int eventid, int nominationPayrollId, string period );

        /// <summary>
        /// approve a nomination
        /// </summary>
        /// <param name="eventid"></param>
        /// <param name="PayrollID"></param>
        /// <param name="period"></param>
        void ShortlistNominee(int eventid, int PayrollID, string period );

        /// <summary>
        /// approve a nomination with rationale
        /// </summary>
        /// <param name="eventid"></param>
        /// <param name="PayrollID"></param>
        /// <param name="period"></param>
        /// <param name="rationale"></param>
        void ShortlistNomineeWithRationale(int eventid, int PayrollID, string rationale, string period);
        /// <summary>
        /// add manager rejection rationale
        /// </summary>
        /// <param name="eventid"></param>
        /// <param name="PayrollID"></param>
        /// <param name="period"></param>
        /// <param name="rationale"></param>
        void NomineeRejectionRationaleByManager(int eventid, int PayrollID, string rationale, string period );


        /// <summary>
        /// approve a nomination with rationale
        /// </summary>
        /// <param name="eventid"></param>
        /// <param name="PayrollID"></param>
        /// <param name="period"></param>
        /// <param name="rationale"></param>
        void ShortlistNomineeWithManagerRationale(int eventid, int PayrollID, string rationale, string period );

        /// <summary>
        /// discard an approved nomination
        /// </summary>
        /// <param name="eventid"></param>
        /// <param name="PayrollID"></param>
        /// <param name="period"></param>
        void RemoveShortlistedNominee(int eventid, int PayrollID, string period );

        /// <summary>
        /// get non shortlisted nominees
        /// returns all
        /// </summary>
        /// <param name="eventid"></param>
        /// <returns></returns>
        List<QueryNominee> GetNonShortListedNominees(int eventid, string period );

        /// <summary>
        /// get rejected nominees
        /// take n records
        /// </summary>
        /// <param name="eventid"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        List<QueryNominee> GetNonShortListedNominees(int eventid, int take, string period );

        /// <summary>
        /// get rejected nominee by nomineeId
        /// </summary>
        /// <param name="eventid"></param>
        /// <param name="nomineeId"></param>
        /// <returns></returns>
        List<QueryNominee> GetNonShortListedNominee(int eventid, int nomineeId, string period );

        /// <summary>
        /// get rejected nominees by manager
        /// </summary>
        /// <param name="eventid"></param>
        /// <param name="managerId"></param>
        /// <returns></returns>
        List<QueryNominee> GetRejectedNomineesByManager(int eventid, int managerId, string period );
        List<QueryNominee> GetTopFiveRejectedNomineesByManager(int eventid, int take, int managerId, string period );
        //test stop

        /// <summary>
        /// admin get voted nominees
        /// </summary>
        /// <param name="eventid"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        List<QueryNominee> AdminGetVotedNominees(int eventid, string period );

        /// <summary>
        /// admin get voted nominees
        /// take n records
        /// per period
        /// </summary>
        /// <param name="eventid">1</param>
        /// <param name="take">20</param>
        /// <param name="period">January2021</param>
        /// <returns></returns>
        List<QueryNominee> AdminGetVotedNominees(int eventid, int take, string period);

        List<QueryNominee> GetShortListedNominees(int eventid, string period );
        List<QueryNominee> GetShortListedNominees(int eventid, int take, string period );
        
             List<QueryNominee> GetShortListedNomineeByNomineePayrollID(int eventid, int id, string period);
        List<QueryNominee> GetShortListedNomineesByManager(int eventid, int take,int managerId, string period );
        List<QueryNominee> GetShortListedNominee(int eventid, int nomineeId, string period );

        void WinnerNominee(int eventid, int PayrollID, string period);
        void DeleteWinner(int eventid, int PayrollID, string period);

        /// <summary>
        /// Change nominee status to winner : Winner: 0=>1
        /// </summary>
        /// <param name="eventid"></param>
        /// <param name="PayrollID"></param>
        /// <param name="period"></param>
        /// 

        List<QueryHOFWinners> GetHallOfFameWinner(int eventid, string period);

    }
}
