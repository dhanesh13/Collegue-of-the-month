// using SendGrid's C# Library
// https://github.com/sendgrid/sendgrid-csharp
using Colleague_Of_The_Month.Interfaces;
using Colleague_Of_The_Month.Services;
using Colleague_Of_The_Month.Email;
using Microsoft.AspNetCore.Mvc;


namespace EmailController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        
        private readonly IEmailSender _emailSender;
        private readonly IDetails _IDetails;
        private readonly IVotes _IVotes;
        private readonly IMail _IMail;

        public EmailController (IEmailSender emailSender,IDetails IDetails, IVotes IVotes,IMail IMail)
        {
            
            _emailSender = emailSender;
            _IDetails = IDetails;
            _IVotes = IVotes;
            _IMail = IMail;
        }


      
        [HttpGet("/sendEmailTest")]
        public void SendEmailTest()

        {
            var emails=_IDetails.GetTestEmails();
            string closingdate = _IVotes.GetClosingDateOfCOTMVotingSession();
            string openingdate = _IVotes.GetOpeningDateOfCOTMVotingSession();
            string[] openiningclosingdatedetails = new string[2];
            openiningclosingdatedetails[0] = openingdate;
            openiningclosingdatedetails[1] = closingdate;

            var message = new Message(emails, "Colleague Of The Month", "Opening and closingdate");


            _emailSender.SendEmail(message, openiningclosingdatedetails);
        }

        [HttpGet("sendEmailManagersForVotingSession")]
        public void SendEmailManagersForVotingSession()

        {
            //retrieve email addresses
            var emails = _IDetails.GetTestEmails();
            //retrieve closing date for voting session
            var closingdate = _IVotes.GetClosingDateOfCOTMVotingSession();

            //need to get open and closing date
            string openingdate = _IVotes.GetOpeningDateOfCOTMVotingSession();
            string[] openiningclosingdatedetails = new string[2];
            openiningclosingdatedetails[0] = openingdate;
            openiningclosingdatedetails[1] = closingdate;

            var message = new Message(emails, "Colleague Of The Month Voting Commitee", "Opening and closingdate");
            _emailSender.SendEmailManagerVotingSession(message, openiningclosingdatedetails);
        }
        [HttpGet("sendEmailForWinners")]
        public void SendEmailForWinners()

        {
            //retrieve email addresses
            var emails = _IDetails.GetTestEmails();
            //retrieve closing date for voting session
            var closingdate = _IVotes.GetClosingDateOfCOTMVotingSession();

            var message = new Message(emails, "Colleague Of The Month Winner", closingdate);
            _emailSender.SendEmailForWinners(message);
        }

        [HttpGet("sendEmailForVouchers")]
        public void SendEmailForVouchers()

        {
            //retrieve email addresses
            var emails = _IDetails.GetTestEmails();
            //retrieve closing date for voting session
            var closingdate = _IVotes.GetClosingDateOfCOTMVotingSession();

            var message = new Message(emails, "Colleague Of The Month Vouchers", closingdate);
            _emailSender.SendEmailForVouchers(message);
        }
        [HttpGet("sendEmailForCOTMEventStart")]
        public void SendEmailForCOTMEventStart()

        {
            //retrieve email addresses
            var emails = _IDetails.GetTestEmails();
            //retrieve closing date for Nomination Period
           
            var closingdate = _IVotes.GetClosingDateOfCOTMEvent();

            var message = new Message(emails, "Nominate Your Colleague Of The Month", closingdate);
            _emailSender.SendEmailForCOTMEventStart(message);
        }
        [HttpGet("sendEmailForShortlistPeriodOpen")]
        public void SendEmailForShortlistPeriodOpen()

        {
            //retrieve email addresses
            var emails = _IDetails.GetTestEmails();
            //retrieve closing date for voting session
            var closingdate = _IVotes.GetClosingDateOfCOTMShortlistPeriod();

            var message = new Message(emails, "COTM Shortlisting period open", closingdate);
            _emailSender.SendEmailForShortlistPeriodOpen(message);
        }

        [HttpPatch("updateMailSentWhenCOTMOpen/{period}")]
        public void UpdateMailSentWhenCOTMOpen(string period)

        {
            _IMail.UpdateMailSentWhenCOTMOpen(period);
        }
        [HttpPatch("updateMailSentWhenShortlistPeriodOpen/{period}")]
        public void UpdateMailSentWhenShortlistPeriodOpen(string period)

        {
            _IMail.UpdateMailSentWhenShortlistPeriodOpen(period);
        }


    }
}