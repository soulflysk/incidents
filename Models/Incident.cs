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
        public int IncidentId { get; set; }
        
        [Required(ErrorMessage = "กรุณากรอกปัญหา/ความต้องการ")]
        public string Problem { get; set; }
        
        [Required(ErrorMessage = "กรุณาเลือกวันที่รับแจ้ง")]
        public string Date { get; set; }
        
        [Required(ErrorMessage = "กรุณาเลือกสถานะ")]
        public string Result { get; set; }
        
        [Required(ErrorMessage = "กรุณาเลือกประเภทผู้แจ้ง")]
        public string InsideOrOutside { get; set; }
        
        public int EmployeeId { get; set; }
        
        public string? Email { get; set; }
        
        [Required(ErrorMessage = "กรุณากรอกชื่อผู้แจ้ง")]
        public string Name { get; set; }
        
        public string? CompanyName { get; set; }
        
        public int? PhoneNumber { get; set; }
        
        [Required(ErrorMessage = "กรุณาเลือกประเภทบริการ")]
        public int serviceTypeId { get; set; }

        // Navigation properties
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }
        
        [ForeignKey("serviceTypeId")]
        public virtual serviceTypes? ServiceType { get; set; }
    }
}