using System.ComponentModel.DataAnnotations;

namespace DOTNETCORE_DEV.Models
{
    public class serviceTypes
    {
        [Key]
        public int serviceTypeId { get; set; }
        public string serviceTypesName { get; set; }
    }
}