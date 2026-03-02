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
        public DbSet<IncidentResolution> IncidentResolutions {get; set;}
        public DbSet<IncidentAssignment> IncidentAssignments {get; set;}
        public DbSet<AssignmentResolution> AssignmentResolutions {get; set;}
        public DbSet<AssignmentResolutionAttachment> AssignmentResolutionAttachments {get; set;}
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure Incident entity
            modelBuilder.Entity<Incident>(entity =>
            {
                entity.HasKey(e => e.IncidentId);
                entity.Property(e => e.IncidentId)
                    .ValueGeneratedOnAdd();
            });
            
            // Configure IncidentResolution entity
            modelBuilder.Entity<IncidentResolution>(entity =>
            {
                entity.HasKey(e => e.ResolutionId);
                entity.Property(e => e.ResolutionId)
                    .ValueGeneratedOnAdd();
                
                entity.HasOne(e => e.Incident)
                    .WithMany(i => i.IncidentResolutions)
                    .HasForeignKey(e => e.IncidentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            // Configure IncidentAssignment entity
            modelBuilder.Entity<IncidentAssignment>(entity =>
            {
                entity.HasKey(e => e.AssignmentId);
                entity.Property(e => e.AssignmentId)
                    .ValueGeneratedOnAdd();
                
                // Configure self-referencing relationship for ParentAssignment
                entity.HasOne(e => e.ParentAssignment)
                    .WithMany()
                    .HasForeignKey(e => e.ParentAssignmentId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                // Configure foreign keys with NO ACTION to prevent cascade cycles
                entity.HasOne(e => e.AssignedByEmployee)
                    .WithMany()
                    .HasForeignKey(e => e.AssignedByEmployeeId)
                    .OnDelete(DeleteBehavior.NoAction);
                
                entity.HasOne(e => e.AssignedToEmployee)
                    .WithMany()
                    .HasForeignKey(e => e.AssignedToEmployeeId)
                    .OnDelete(DeleteBehavior.NoAction);
                
                entity.HasOne(e => e.Incident)
                    .WithMany(i => i.IncidentAssignments)
                    .HasForeignKey(e => e.IncidentId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                // Configure indexes for better performance
                entity.HasIndex(e => e.IncidentId);
                entity.HasIndex(e => e.AssignedToEmployeeId);
                entity.HasIndex(e => e.AssignedByEmployeeId);
                entity.HasIndex(e => e.AssignmentStatus);
                entity.HasIndex(e => e.AssignmentType);
                entity.HasIndex(e => e.AssignedDateTime);
            });
            
            // Configure AssignmentResolution entity
            modelBuilder.Entity<AssignmentResolution>(entity =>
            {
                entity.HasKey(e => e.ResolutionId);
                entity.Property(e => e.ResolutionId)
                    .ValueGeneratedOnAdd();
                
                // Configure foreign keys with NO ACTION to prevent cascade cycles
                entity.HasOne(e => e.Assignment)
                    .WithMany()
                    .HasForeignKey(e => e.AssignmentId)
                    .OnDelete(DeleteBehavior.NoAction);
                
                entity.HasOne(e => e.Incident)
                    .WithMany()
                    .HasForeignKey(e => e.IncidentId)
                    .OnDelete(DeleteBehavior.NoAction);
                
                entity.HasOne(e => e.Employee)
                    .WithMany()
                    .HasForeignKey(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.NoAction);
                
                // Configure indexes
                entity.HasIndex(e => e.AssignmentId);
                entity.HasIndex(e => e.IncidentId);
                entity.HasIndex(e => e.EmployeeId);
                entity.HasIndex(e => e.ResolutionStatus);
                entity.HasIndex(e => e.EstimatedStartDate);
                entity.HasIndex(e => e.EstimatedEndDate);
            });
            
            // Configure AssignmentResolutionAttachment entity
            modelBuilder.Entity<AssignmentResolutionAttachment>(entity =>
            {
                entity.HasKey(e => e.AttachmentId);
                entity.Property(e => e.AttachmentId)
                    .ValueGeneratedOnAdd();
                
                entity.HasOne(e => e.Resolution)
                    .WithMany(e => e.Attachments)
                    .HasForeignKey(e => e.ResolutionId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasIndex(e => e.ResolutionId);
                entity.HasIndex(e => e.UploadedAt);
            });
            
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
                new Employee { EmployeeID = 2, Name = "มานี รักดี", SupportLevel = 1 },
                new Employee { EmployeeID = 3, Name = "วิชัย รักงาน", SupportLevel = 2 },
                new Employee { EmployeeID = 4, Name = "สมศรี มุ่งมั่น", SupportLevel = 2 },
                new Employee { EmployeeID = 5, Name = "ประเสริฐ ตั้งใจ", SupportLevel = 2 }
            );
        }
    }
}
