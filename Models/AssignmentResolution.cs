using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DOTNETCORE_DEV.Models
{
    public class AssignmentResolution
    {
        [Key]
        public int ResolutionId { get; set; }
        
        [Required]
        public int AssignmentId { get; set; }
        
        [ForeignKey("AssignmentId")]
        public virtual IncidentAssignment? Assignment { get; set; }
        
        [Required]
        public int IncidentId { get; set; }
        
        [ForeignKey("IncidentId")]
        public virtual Incident? Incident { get; set; }
        
        [Required]
        public int EmployeeId { get; set; }
        
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Component { get; set; } = string.Empty;
        
        [Required]
        [StringLength(1000)]
        public string ResolutionDetails { get; set; } = string.Empty;
        
        [Required]
        public DateTime EstimatedStartDate { get; set; }
        
        [Required]
        public DateTime EstimatedEndDate { get; set; }
        
        public DateTime? ActualCompletionDate { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal HoursSpent { get; set; } = 0;
        
        [Required]
        public string ResolutionStatus { get; set; } = "In Progress"; // "In Progress", "Completed", "On Hold", "Cancelled"
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        [StringLength(500)]
        public string? CompletionNotes { get; set; }
        
        public int Priority { get; set; } = 1; // 1 = สูงสุด, 2 = สูง, 3 = ปานกลาง, 4 = ต่ำ
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        
        // Navigation properties
        public virtual ICollection<AssignmentResolutionAttachment> Attachments { get; set; } = new List<AssignmentResolutionAttachment>();
    }
    
    public class AssignmentResolutionAttachment
    {
        [Key]
        public int AttachmentId { get; set; }
        
        [Required]
        public int ResolutionId { get; set; }
        
        [ForeignKey("ResolutionId")]
        public virtual AssignmentResolution? Resolution { get; set; }
        
        [Required]
        [StringLength(255)]
        public string FileName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(500)]
        public string FilePath { get; set; } = string.Empty;
        
        [StringLength(255)]
        public string? OriginalFileName { get; set; }
        
        [StringLength(50)]
        public string? ContentType { get; set; }
        
        public long FileSize { get; set; } = 0;
        
        public DateTime UploadedAt { get; set; } = DateTime.Now;
        
        public string? UploadedBy { get; set; }
    }
}
