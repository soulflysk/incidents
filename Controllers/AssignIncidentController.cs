using DOTNETCORE_DEV.Data;
using DOTNETCORE_DEV.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

public class AssignIncidentController : Controller
{
    private readonly ApplicationDbContext _db;
        public AssignIncidentController(ApplicationDbContext db)
        {
            _db = db;
        }
    private static List<Employee> employees = new List<Employee>
    {
        new Employee { EmployeeID = 1, Name = "Alice", SupportLevel = 1 },
        new Employee { EmployeeID = 2, Name = "Bob", SupportLevel = 1 },
        new Employee { EmployeeID = 3, Name = "Charlie", SupportLevel = 2 }
    };

    private static List<AssignIncident> assignIncidents = new List<AssignIncident>();

    public IActionResult Index()
    {
        ViewBag.Incidents = _db.Incidents.ToList();;
        ViewBag.Employees = employees;
        return View();
    }

    [HttpPost]
    public IActionResult Assign(int incidentId, int assignedById, int assignedToId)
    {
        var assignedBy = employees.FirstOrDefault(e => e.EmployeeID == assignedById);
        var assignedTo = employees.FirstOrDefault(e => e.EmployeeID == assignedToId);

        if (assignedBy == null || assignedTo == null)
        {
            ViewBag.Error = "พนักงานไม่ถูกต้อง";
            return RedirectToAction("Index");
        }

        if (assignedBy.SupportLevel == 1 && assignedTo.SupportLevel == 1 ||
            assignedBy.SupportLevel == 1 && assignedTo.SupportLevel == 2 ||
            assignedBy.SupportLevel == 2 && assignedTo.SupportLevel == 2)
        {
            _db.AssignIncidents.Add(new AssignIncident
            {
                IncidentId = incidentId,
                AssignedByEmployeeID = assignedById,
                AssignedToEmployeeID = assignedToId,
                AssignDateTime = DateTime.Now
            });
            _db.SaveChanges();

            ViewBag.Success = "Assign สำเร็จ!";
        }
        else
        {
            ViewBag.Error = "ไม่สามารถ Assign Incident ตามกฎที่กำหนด";
        }

        return RedirectToAction("Index");
    }

    public IActionResult History(int incidentId)
    {
        var history = _db.AssignIncidents.Where(a => a.IncidentId == incidentId).ToList();
        ViewBag.History = history;
        return View();
    }
}
