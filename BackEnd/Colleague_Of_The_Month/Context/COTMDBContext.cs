using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Models
{
    public class COTMDBContext : DbContext
    {
        public COTMDBContext(DbContextOptions<COTMDBContext> options) : base(options)
        {

        }

        public DbSet<Details> Details { get; set; }

        public DbSet<Division> Division { get; set; }

        public DbSet<Subdivision> Subdivision { get; set; }

        public DbSet<BusinessUnit> BusinessUnit { get; set; }

        public DbSet<Department> Department { get; set; }

        public DbSet<Nominee> Nominees { get; set; }

        public DbSet<CostCentre> CostCentre { get; set; }

        public DbSet<Employee> Employee { get; set; }

        public DbSet<Event> Event { get; set; }

        public DbSet<Voucher> Voucher { get; set; }

        public DbSet<VoteLog> VoteLog { get; set; }

        public DbSet<VotingSession> VotingSession { get; set; }
        public DbSet<Mail> Mail { get; set; }

        public DbSet<InspireTeam> InspireTeam { get; set; }
        public DbSet<QueryVoucher> QueryVoucher { get; set; }

    }
}
