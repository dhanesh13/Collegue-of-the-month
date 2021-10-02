using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Colleague_Of_The_Month.Interfaces;
using Colleague_Of_The_Month.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Colleague_Of_The_Month.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private IWebHostEnvironment Environment;
        private IConfiguration Configuration;
        private readonly COTMDBContext _context;
        private readonly IUpload _IUpload;
        public UploadController(IWebHostEnvironment _environment, IConfiguration _configuration, COTMDBContext context, IUpload iUpload)
        {
            Environment = _environment;
            Configuration = _configuration;
            _context = context;
            _IUpload = iUpload;
        }


        [HttpPut("updateEmployeeList"), DisableRequestSizeLimit]
        public async Task<bool> Upload()
        {
            bool saveOk = false;
            try
            {
                var formCollection = await Request.ReadFormAsync();
                saveOk = _IUpload.Upload(formCollection);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return saveOk;
        }
    }
}

