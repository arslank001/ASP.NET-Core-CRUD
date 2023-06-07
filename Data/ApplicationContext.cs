using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUDCoreApplication.Models;
using CRUDCoreApplication.Models.Account;

namespace CRUDCoreApplication.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
        public DbSet<EmployeeModel> Employee { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<ImageModel> Images { get; set; }
    }
}
