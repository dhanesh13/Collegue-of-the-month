using Colleague_Of_The_Month.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Services
{
    public interface IDetails
    {
       Task<List<Details>> GetEmployeeDetails();

        //Task<List<QueryEmployee>> GetEmployeesByDivDepMan();

        List<QueryEmail> GetEmployeeEmails();
        List<QueryEmail> GetAllManagerEmails();
        List<QueryEmail> GetWinnersEmails();
        List<string> GetTestEmails();
        List<string> GetAllEmployeeEmails();
        List<string> GetAllManagerEmailsList();
        List<string> GetWinnersEmailsList();



    }
}

