using Colleague_Of_The_Month.Models;
using Colleague_Of_The_Month.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotingController : ControllerBase
    {
        public  COTMDBContext _context;

        private readonly IVotes _voteService;

      

        //VoteService vs = new VoteService(_context);

        public VotingController(COTMDBContext context, IVotes iVotes)
        {
            _context = context;
            _voteService = iVotes;
            
        }

        [HttpGet("getVotelogs/{sessionId}/{eventId}/{period}")]
        public List<VoteLog> GetVoteLogs(int sessionId, int eventId, string period="")
        {
            List<VoteLog> vl = new List<VoteLog>();

            try
            {
                vl = _voteService.GetVoteLogs(sessionId, eventId,period);                             
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);                
            }
            return vl;
        }

        [HttpGet("getVotelog/{sessionId}/{eventId}/{managerId}/{period}")]
        public VoteLogNominees GetVoteLog(int sessionId, int eventId, int managerId,string period)
        {
            VoteLogNominees vl = new VoteLogNominees();

            try
            {
                vl = _voteService.GetVoteLog(sessionId, eventId, managerId, period);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return vl;
        }

        [HttpPost("vote")]
        public bool VoteNow(List<VoteLog> votelog)
        {
            bool saveOk = false;

            try
            {
                saveOk = _voteService.Vote(votelog);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return saveOk;
        }

        //[HttpPost("createVotingSession")]
        //public bool CreateVotingSession(VotingSession vs)
        //{
        //    bool saveOk = false;

        //    try
        //    {
        //        saveOk = _IVotes.CreateVotingSession(vs);
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message);
        //    }
        //    return saveOk;
        //}

        [HttpPost("createVotingSession")]
        public bool CreateVotingSession(VotingSession vs)
        {
            bool saveOk = false;
            try
            {
                saveOk = _voteService.CreateVotingSession(vs);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return saveOk;
        }

        [HttpGet("getVotingSessions")]
        public async Task<List<VotingSession>> GetVotingSessions()
        {
            List<VotingSession> vs = new List<VotingSession>();

            try
            {
                vs = await _voteService.GetVotingSessions();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return vs;
        }

        [HttpGet("getEventsByFilter")]
        public List<QueryEvent> GetEventNameDescrip()
        {
            return _voteService.GetEventNameDescrip();
        }

        [HttpPut("closeVotingSession/{sessionId}")]
        public IActionResult CloseVotingSession(int sessionId)
        {
            _voteService.CloseVotingSession(sessionId);
            return Ok();
        }

        [HttpPut("editVotingSession/{sessionId}")]
        public bool EditVotingSession(int sessionId, VotingSession VotingSessionModel)
        {
            bool saveOk = false;
            try
            {
                saveOk = _voteService.EditVotingSession(sessionId, VotingSessionModel);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return saveOk;
        }

        [HttpGet("getEditSession/{sessionId}")]
        public VotingSession GetEditSession(int sessionId)
        {
            return _voteService.GetEditSession(sessionId);
        }

        [HttpPut("statusClosingDate")]
        public IActionResult StatusClosingDate()
        {
            _voteService.StatusClosingDate();
            return Ok();
        }

        [HttpGet("getActiveCOTMEvent")]
        public bool GetActiveCOTMEvent()
        {
            return _voteService.GetActiveCOTMEvent();
        }
        [HttpGet("getSessionIDofActiveCOTMEvent")]
        public int GetSessionIDofActiveCOTMEvent()
        {
            return _voteService.GetSessionIDofActiveCOTMEvent();
        }

        [HttpGet("getClosingDateActiveCOTMEvent")]
        public string GetClosingDateOfCOTMEvent()
        {
            return _voteService.GetClosingDateOfCOTMShortlistPeriod();
        }
        [HttpPut("VotelogCount")]
        public void VotelogToNominees()
        {
            _voteService.VotelogToNominees();
        }


    }
}
