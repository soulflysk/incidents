using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using NuGet.Protocol;

namespace DOTNETCORE_DEV.Models
{
    public class Incident
    {
        public int IncidentId { get; set; }
        public string Problem { get; set; }
        public string Date { get; set; }
        public string Result { get; set; }
        public string InsideOrOutside { get; set; }
        public int EmployeeId { get; set; }
        public string Email { get; set; }
        public string? Name { get; set; }
        public string? CompanyName { get; set; }
        public int? PhoneNumber { get; set; }
        public int serviceTypeId { get; set; }
    }
}