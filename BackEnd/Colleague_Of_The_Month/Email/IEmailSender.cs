using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Email
{
    public interface IEmailSender
    {
        void SendEmail(Message message, string[] openiningclosingdatedetails);
        void SendEmailManagerVotingSession(Message message, string[] openiningclosingdatedetails);
        void SendEmailForWinners(Message message);
        void SendEmailForVouchers(Message message);
        void SendEmailForChoiceOfVouchers(Message message, String[] details);

        void SendEmailForCOTMEventStart(Message message);
        void SendEmailForShortlistPeriodOpen(Message message);

    }
}
