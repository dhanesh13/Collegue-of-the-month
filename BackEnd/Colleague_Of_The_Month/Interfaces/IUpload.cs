using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Interfaces
{
    public interface IUpload
    {
        bool Upload(IFormCollection formCollection);
    }
}
