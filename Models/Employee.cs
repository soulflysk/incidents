using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNETCORE_DEV.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int SupportLevel { get; set; } // 1 = 1st Level, 2 = 2nd Level
    }
}