using Colleague_Of_The_Month.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Services
{
    public interface IVoucher
    {
        Task<List<Voucher>> GetVouchers();
        bool PostVoucher(QueryVoucher incomingVoucherData);

        bool showVoucher(int nomineePayrollId, string period, int eventId);
    }
}
