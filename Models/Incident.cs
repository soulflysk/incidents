using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using NuGet.Protocol;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DOTNETCORE_DEV.Models
{
    public class Incident
    {
        [Key]
        public int IncidentId { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Problem { get; set; } = string.Empty;
        
        [Required]
        public DateTime Date { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Result { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string InsideOrOutside { get; set; } = string.Empty;
        
        public int? EmployeeId { get; set; }
        
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }
        
        [StringLength(100)]
        public string? Email { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string? CompanyName { get; set; }
        
        [StringLength(20)]
        public string? PhoneNumber { get; set; }
        
        public int? serviceTypeId { get; set; }
        
        [ForeignKey("serviceTypeId")]
        public virtual serviceTypes? ServiceType { get; set; }
        
        // Navigation properties
        public virtual ICollection<IncidentResolution> IncidentResolutions { get; set; } = new List<IncidentResolution>();
        public virtual ICollection<IncidentAssignment> IncidentAssignments { get; set; } = new List<IncidentAssignment>();
    }
}