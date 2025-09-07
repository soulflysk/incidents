using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using NuGet.Protocol;

namespace DOTNETCORE_DEV.Models
{
    public class serviceTypes
    {
        [Key]
        public int serviceTypeId { get; set; }
        public string serviceTypesName { get; set; }
    }
}