using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Colleague_Of_The_Month.Interfaces;
using Colleague_Of_The_Month.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Colleague_Of_The_Month.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InspireTeamController : ControllerBase
    {
        private readonly COTMDBContext _context;
        private readonly IInspireTeam _IInspireTeam;

        public InspireTeamController(COTMDBContext context, IInspireTeam inspireTeam)
        {
            _context = context;
            _IInspireTeam = inspireTeam;
        }

        #region Inspire Team
        [HttpPost("createInspireTeam")]
        public IActionResult CreateInspire(InspireTeam InspireTeamModel)
        {
            _IInspireTeam.CreateInspire(InspireTeamModel);
            return Ok(InspireTeamModel);
        }

        [HttpGet("getIT/{eventid}/{period}")]
        public string GetNomineesInspireTeam(int eventid, string period)
        {
            return JsonConvert.SerializeObject(_IInspireTeam.GetNomineesInspireTeam(eventid, period));
        }
        #endregion
    }
}
