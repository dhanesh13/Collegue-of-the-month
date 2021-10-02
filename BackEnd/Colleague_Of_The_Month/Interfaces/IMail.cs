using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Interfaces
{
    public interface IMail
    {
        bool CheckEmailSentForCOTMEvent(string period);

        void UpdateMailSentWhenCOTMOpen(string period);

        bool CheckEmailSentForShortlistPeriod(string period);

        void UpdateMailSentWhenShortlistPeriodOpen(string period);
    }
}
