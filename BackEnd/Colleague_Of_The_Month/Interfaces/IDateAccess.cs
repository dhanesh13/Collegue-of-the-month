using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Services
{
    public interface IDateAccess
    {
        bool DateAccess();
        bool DateAccessShortlist();

        string GetPeriod();
    }
}
