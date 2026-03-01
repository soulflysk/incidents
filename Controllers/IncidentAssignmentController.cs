using DOTNETCORE_DEV.Data;
using DOTNETCORE_DEV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

public class IncidentAssignmentController : Controller
{
    private readonly ApplicationDbContext _db;
    public IncidentAssignmentController(ApplicationDbContext db)
    {
        _db = db;
    }

    // แสดงหน้าหลักสำหรับ Assign Incident
    public async Task<IActionResult> Index()
    {
        ViewBag.Incidents = await _db.Incidents
            .Include(i => i.Employee)
            .Include(i => i.ServiceType)
            .ToListAsync();
        
        ViewBag.Employees = await _db.Employees.ToListAsync();
        
        // แสดงประวัติการ assign ล่าสุด
        ViewBag.RecentAssignments = await _db.IncidentAssignments
            .Include(a => a.Incident)
            .Include(a => a.AssignedByEmployee)
            .Include(a => a.AssignedToEmployee)
            .OrderByDescending(a => a.AssignedDateTime)
            .Take(10)
            .ToListAsync();
        
        return View();
    }

    // แสดงฟอร์ม Assign Incident
    public async Task<IActionResult> Create(int incidentId)
    {
        var incident = await _db.Incidents
            .Include(i => i.Employee)
            .Include(i => i.ServiceType)
            .FirstOrDefaultAsync(i => i.IncidentId == incidentId);
        
        if (incident == null)
        {
            return NotFound();
        }

        ViewBag.Incident = incident;
        ViewBag.Employees = await _db.Employees.ToListAsync();
        
        return View();
    }

    // บันทึกการ Assign Incident
    [HttpPost]
    public async Task<IActionResult> Assign(int incidentId, int assignedByEmployeeId, int assignedToEmployeeId, string assignmentNotes)
    {
        try
        {
            var assignedBy = await _db.Employees.FindAsync(assignedByEmployeeId);
            var assignedTo = await _db.Employees.FindAsync(assignedToEmployeeId);
            var incident = await _db.Incidents.FindAsync(incidentId);

            if (assignedBy == null || assignedTo == null || incident == null)
            {
                ViewBag.Error = "ข้อมูลไม่ถูกต้อง";
                return RedirectToAction("Index");
            }

            // ตรวจสอบกฎการ assign
            string assignmentType = GetAssignmentType(assignedBy.SupportLevel, assignedTo.SupportLevel);
            
            if (!IsValidAssignment(assignedBy.SupportLevel, assignedTo.SupportLevel))
            {
                ViewBag.Error = $"ไม่สามารถ Assign Incident จาก {GetSupportLevelName(assignedBy.SupportLevel)} ไปยัง {GetSupportLevelName(assignedTo.SupportLevel)} ได้";
                return RedirectToAction("Index");
            }

            // สร้างการ assign ใหม่
            var assignment = new IncidentAssignment
            {
                IncidentId = incidentId,
                AssignedByEmployeeId = assignedByEmployeeId,
                AssignedToEmployeeId = assignedToEmployeeId,
                AssignedDateTime = DateTime.Now,
                AssignmentStatus = "Assigned",
                AssignmentNotes = assignmentNotes,
                AssignmentType = assignmentType,
                Priority = 1,
                IsActive = true
            };

            _db.IncidentAssignments.Add(assignment);
            await _db.SaveChangesAsync();

            ViewBag.Success = $"Assign Incident สำเร็จ! ส่งจาก {assignedBy.Name} ไปยัง {assignedTo.Name}";
        }
        catch (Exception ex)
        {
            ViewBag.Error = $"เกิดข้อผิดพลาด: {ex.Message}";
        }

        return RedirectToAction("Index");
    }

    // แสดงประวัติการ Assign ของ Incident
    public async Task<IActionResult> History(int incidentId)
    {
        var incident = await _db.Incidents
            .Include(i => i.Employee)
            .Include(i => i.ServiceType)
            .FirstOrDefaultAsync(i => i.IncidentId == incidentId);
        
        if (incident == null)
        {
            return NotFound();
        }

        var assignments = await _db.IncidentAssignments
            .Include(a => a.AssignedByEmployee)
            .Include(a => a.AssignedToEmployee)
            .Where(a => a.IncidentId == incidentId)
            .OrderBy(a => a.AssignedDateTime)
            .ToListAsync();

        ViewBag.Incident = incident;
        return View(assignments);
    }

    // แสดงงานที่รับผิดชอบของพนักงาน
    public async Task<IActionResult> MyAssignments(int employeeId)
    {
        var employee = await _db.Employees.FindAsync(employeeId);
        if (employee == null)
        {
            return NotFound();
        }

        var assignments = await _db.IncidentAssignments
            .Include(a => a.Incident)
            .Include(a => a.AssignedByEmployee)
            .Where(a => a.AssignedToEmployeeId == employeeId && a.IsActive)
            .OrderBy(a => a.Priority)
            .ThenBy(a => a.AssignedDateTime)
            .ToListAsync();

        ViewBag.Employee = employee;
        return View(assignments);
    }

    // ยอมรับการ Assign
    [HttpPost]
    public async Task<IActionResult> AcceptAssignment(int assignmentId)
    {
        var assignment = await _db.IncidentAssignments.FindAsync(assignmentId);
        if (assignment != null)
        {
            assignment.AssignmentStatus = "Accepted";
            assignment.AcceptedDateTime = DateTime.Now;
            assignment.UpdatedAt = DateTime.Now;
            await _db.SaveChangesAsync();
        }
        
        return RedirectToAction("MyAssignments", new { employeeId = assignment?.AssignedToEmployeeId });
    }

    // ปฏิเสธการ Assign
    [HttpPost]
    public async Task<IActionResult> RejectAssignment(int assignmentId, string rejectionReason)
    {
        var assignment = await _db.IncidentAssignments.FindAsync(assignmentId);
        if (assignment != null)
        {
            assignment.AssignmentStatus = "Rejected";
            assignment.RejectionReason = rejectionReason;
            assignment.UpdatedAt = DateTime.Now;
            await _db.SaveChangesAsync();
        }
        
        return RedirectToAction("MyAssignments", new { employeeId = assignment?.AssignedToEmployeeId });
    }

    // ทำเครื่องหมายว่าเสร็จสิ้น
    [HttpPost]
    public async Task<IActionResult> CompleteAssignment(int assignmentId)
    {
        var assignment = await _db.IncidentAssignments.FindAsync(assignmentId);
        if (assignment != null)
        {
            assignment.AssignmentStatus = "Completed";
            assignment.CompletedDateTime = DateTime.Now;
            assignment.IsActive = false;
            assignment.UpdatedAt = DateTime.Now;
            await _db.SaveChangesAsync();
        }
        
        return RedirectToAction("MyAssignments", new { employeeId = assignment?.AssignedToEmployeeId });
    }

    // Helper methods
    private string GetAssignmentType(int fromLevel, int toLevel)
    {
        if (fromLevel == 1 && toLevel == 1) return "1stTo1st";
        if (fromLevel == 1 && toLevel == 2) return "1stTo2nd";
        if (fromLevel == 2 && toLevel == 2) return "2ndTo2nd";
        return "Unknown";
    }

    private bool IsValidAssignment(int fromLevel, int toLevel)
    {
        // 1st Level Support สามารถส่งต่อให้ 1st Level Support อื่นได้
        if (fromLevel == 1 && toLevel == 1) return true;
        
        // 1st Level Support สามารถส่งต่อให้ 2nd Level Support ได้
        if (fromLevel == 1 && toLevel == 2) return true;
        
        // 2nd Level Support สามารถส่งต่อให้ 2nd Level Support อื่นได้
        if (fromLevel == 2 && toLevel == 2) return true;
        
        // 2nd Level Support ไม่สามารถส่งต่อให้ 1st Level Support ได้
        return false;
    }

    private string GetSupportLevelName(int level)
    {
        return level switch
        {
            1 => "1st Level Support",
            2 => "2nd Level Support",
            _ => "Unknown Level"
        };
    }
}
