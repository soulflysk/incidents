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
        
    }
}
