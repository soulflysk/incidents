using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DOTNETCORE_DEV.Models
{
    public class IncidentResolution
    {
        [Key]
        public int ResolutionId { get; set; }

        [ForeignKey("Incident")]
        public int IncidentId { get; set; } // เชื่อมโยงกับ Incident หลัก
        public string Component { get; set; } = string.Empty; // เช่น Server/Hardware/Software/Application
        public string ResolutionDetails { get; set; } = string.Empty; // รายละเอียดการแก้ไข
        public DateTime EstimatedStartDate { get; set; } // วันที่คาดว่าจะเริ่มแก้ไข
        public DateTime EstimatedEndDate { get; set; } // วันที่คาดว่าจะเสร็จ
        public DateTime? ActualCompletionDate { get; set; } // วันที่แก้เสร็จจริง (nullable)
        public double HoursSpent { get; set; } // จำนวนชั่วโมงที่ใช้

        public virtual Incident? Incident { get; set; } // ความสัมพันธ์กับ Incident
    }
}
