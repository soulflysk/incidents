using System.ComponentModel.DataAnnotations;

public class AssignIncident
{
    [Key]
    public int AssignID { get; set; }
    public int IncidentId { get; set; }
    public int AssignedByEmployeeID { get; set; }
    public int AssignedToEmployeeID { get; set; }
    public DateTime AssignDateTime { get; set; } = DateTime.Now;
}