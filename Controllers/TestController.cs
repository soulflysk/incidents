using DOTNETCORE_DEV.Data;
using DOTNETCORE_DEV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class TestController : Controller
{
    private readonly ApplicationDbContext _context;
    
    public TestController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IActionResult> Index()
    {
        // ตรวจสอบข้อมูลในระบบ
        var incidents = await _context.Incidents.ToListAsync();
        var assignments = await _context.IncidentAssignments
            .Include(a => a.Incident)
            .Include(a => a.AssignedToEmployee)
            .ToListAsync();
        var employees = await _context.Employees.ToListAsync();
        
        ViewBag.Incidents = incidents;
        ViewBag.Assignments = assignments;
        ViewBag.Employees = employees;
        
        return View();
    }
    
    public async Task<IActionResult> CreateTestAssignment()
    {
        // สร้างข้อมูลทดสอบ
        var incident = await _context.Incidents.FirstOrDefaultAsync();
        var employee = await _context.Employees.FirstOrDefaultAsync();
        
        if (incident != null && employee != null)
        {
            var assignment = new IncidentAssignment
            {
                IncidentId = incident.IncidentId,
                AssignedByEmployeeId = employee.EmployeeID,
                AssignedToEmployeeId = employee.EmployeeID,
                AssignedDateTime = DateTime.Now,
                AssignmentStatus = "Accepted",
                AssignmentType = "Direct",
                Priority = 2,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                AcceptedDateTime = DateTime.Now
            };
            
            _context.IncidentAssignments.Add(assignment);
            await _context.SaveChangesAsync();
            
            ViewBag.Message = $"สร้าง assignment ID: {assignment.AssignmentId} เรียบร้อย";
            ViewBag.AssignmentId = assignment.AssignmentId;
        }
        else
        {
            ViewBag.Message = "ไม่พบข้อมูล incident หรือ employee";
        }
        
        return View();
    }
}
