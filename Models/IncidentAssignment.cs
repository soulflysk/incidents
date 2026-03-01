using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DOTNETCORE_DEV.Models
{
    public class IncidentAssignment
    {
        [Key]
        public int AssignmentId { get; set; }
        
        [Required]
        public int IncidentId { get; set; }
        
        [ForeignKey("IncidentId")]
        public virtual Incident? Incident { get; set; }
        
        [Required]
        public int AssignedByEmployeeId { get; set; }
        
        [ForeignKey("AssignedByEmployeeId")]
        public virtual Employee? AssignedByEmployee { get; set; }
        
        [Required]
        public int AssignedToEmployeeId { get; set; }
        
        [ForeignKey("AssignedToEmployeeId")]
        public virtual Employee? AssignedToEmployee { get; set; }
        
        [Required]
        public DateTime AssignedDateTime { get; set; }
        
        [Required]
        public string AssignmentStatus { get; set; } = "Assigned"; // Assigned, Accepted, Rejected, Completed
        
        [StringLength(500)]
        public string? AssignmentNotes { get; set; }
        
        [StringLength(500)]
        public string? RejectionReason { get; set; }
        
        public DateTime? AcceptedDateTime { get; set; }
        
        public DateTime? CompletedDateTime { get; set; }
        
        // สำหรับ tracking การส่งต่อ
        public int? ParentAssignmentId { get; set; }
        
        [ForeignKey("ParentAssignmentId")]
        public virtual IncidentAssignment? ParentAssignment { get; set; }
        
        // สำหรับ tracking ประเภทการส่งต่อ
        [Required]
        public string AssignmentType { get; set; } // "1stTo1st", "1stTo2nd", "2ndTo2nd"
        
        // สำหรับ priority
        public int Priority { get; set; } = 1; // 1 = สูงสุด, 2 = สูง, 3 = ปานกลาง, 4 = ต่ำ
        
        // สำหรับ tracking สถานะการทำงาน
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
