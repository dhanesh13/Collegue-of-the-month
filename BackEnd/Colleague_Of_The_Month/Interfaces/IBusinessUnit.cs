using Colleague_Of_The_Month.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Services
{
    public interface IBusinessUnit
    {
        Task<List<BusinessUnit>> GetBusinessUnits();
    }
}
