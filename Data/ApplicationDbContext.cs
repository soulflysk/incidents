using DOTNETCORE_DEV.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DOTNETCORE_DEV.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Incident> Incidents {get; set; }
        public DbSet<serviceTypes> ServiceTypes {get; set; }
        public DbSet<Employee> Employees {get; set;}
        public DbSet<AssignIncident> AssignIncidents {get; set;}
        public DbSet<IncidentResolution> IncidentResolutions {get; set;}
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Seed ServiceTypes
            modelBuilder.Entity<serviceTypes>().HasData(
                new serviceTypes { serviceTypeId = 1, serviceTypesName = "MS Office" },
                new serviceTypes { serviceTypeId = 2, serviceTypesName = "PC/Mobile Device/Notebook/Printer" },
                new serviceTypes { serviceTypeId = 3, serviceTypesName = "ขอติดตั้ง Software" },
                new serviceTypes { serviceTypeId = 4, serviceTypesName = "ขอบริการเกี่ยวกับบัญชีผู้ใช้งาน" },
                new serviceTypes { serviceTypeId = 5, serviceTypesName = "ระบบงานภายนอก" },
                new serviceTypes { serviceTypeId = 6, serviceTypesName = "ระบบงานภายใน" }
            );
            
            // Seed Employees
            modelBuilder.Entity<Employee>().HasData(
                new Employee { EmployeeID = 1, Name = "สมชาย ใจดี", SupportLevel = 1 },
                new Employee { EmployeeID = 2, Name = "มานี รักดี", SupportLevel = 1 }
            );
        }
    }
}
