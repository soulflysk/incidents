using DOTNETCORE_DEV.Data;
using DOTNETCORE_DEV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

public class AssignmentResolutionController : Controller
{
    private readonly ApplicationDbContext _context;
    public AssignmentResolutionController(ApplicationDbContext context)
    {
        _context = context;
    }

    // แสดงรายการการแก้ไขปัญหาของ assignment
    public async Task<IActionResult> Index(int assignmentId)
    {
        var assignment = await _context.IncidentAssignments
            .Include(a => a.Incident)
            .Include(a => a.AssignedToEmployee)
            .Include(a => a.AssignedByEmployee)
            .FirstOrDefaultAsync(a => a.AssignmentId == assignmentId);

        if (assignment == null)
        {
            return NotFound();
        }

        ViewBag.Assignment = assignment;
        
        var resolutions = await _context.AssignmentResolutions
            .Include(r => r.Employee)
            .Where(r => r.AssignmentId == assignmentId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        return View(resolutions);
    }

    // แสดงฟอร์มบันทึกการแก้ไขปัญหา
    public async Task<IActionResult> Create(int assignmentId)
    {
        var assignment = await _context.IncidentAssignments
            .Include(a => a.Incident)
            .Include(a => a.AssignedToEmployee)
            .Include(a => a.AssignedByEmployee)
            .FirstOrDefaultAsync(a => a.AssignmentId == assignmentId);

        if (assignment == null)
        {
            return NotFound();
        }

        // ตรวจสอบว่า assignment นี้ถูก accept แล้วหรือไม่
        if (assignment.AssignmentStatus != "Accepted")
        {
            ViewBag.Error = "กรุณายอมรับการมอบหมายก่อนบันทึกการแก้ไขปัญหา";
            return RedirectToAction("MyAssignments", "IncidentAssignment", new { employeeId = assignment.AssignedToEmployeeId });
        }

        ViewBag.Assignment = assignment;
        ViewBag.Components = GetComponentOptions();
        
        return View(new AssignmentResolution 
        { 
            AssignmentId = assignmentId,
            IncidentId = assignment.IncidentId,
            EmployeeId = assignment.AssignedToEmployeeId,
            EstimatedStartDate = DateTime.Today,
            EstimatedEndDate = DateTime.Today.AddDays(1)
        });
    }

    // บันทึกการแก้ไขปัญหา
    [HttpPost]
    public async Task<IActionResult> Create(AssignmentResolution model)
    {
        Console.WriteLine("=== CREATE ASSIGNMENT RESOLUTION DEBUG ===");
        Console.WriteLine($"AssignmentId: {model.AssignmentId}");
        Console.WriteLine($"IncidentId: {model.IncidentId}");
        Console.WriteLine($"EmployeeId: {model.EmployeeId}");
        Console.WriteLine($"Component: {model.Component}");
        Console.WriteLine($"ResolutionDetails: {model.ResolutionDetails}");
        Console.WriteLine($"EstimatedStartDate: {model.EstimatedStartDate}");
        Console.WriteLine($"EstimatedEndDate: {model.EstimatedEndDate}");
        Console.WriteLine($"ActualCompletionDate: {model.ActualCompletionDate}");

        // ตรวจสอบว่า AssignmentId มีอยู่จริงหรือไม่
        var assignment = await _context.IncidentAssignments
            .Include(a => a.Incident)
            .Include(a => a.AssignedToEmployee)
            .FirstOrDefaultAsync(a => a.AssignmentId == model.AssignmentId);

        if (assignment == null)
        {
            ModelState.AddModelError("", "ไม่พบการมอบหมายที่ระบุ");
            return View(model);
        }

        // ตรวจสอบว่า assignment ถูก accept แล้วหรือไม่
        if (assignment.AssignmentStatus != "Accepted")
        {
            ModelState.AddModelError("", "กรุณายอมรับการมอบหมายก่อนบันทึกการแก้ไขปัญหา");
            return View(model);
        }

        // ตรวจสอบวันที่
        if (model.EstimatedStartDate == DateTime.MinValue)
        {
            ModelState.AddModelError("EstimatedStartDate", "กรุณาเลือกวันที่เริ่มต้น");
        }

        if (model.EstimatedEndDate == DateTime.MinValue)
        {
            ModelState.AddModelError("EstimatedEndDate", "กรุณาเลือกวันที่สิ้นสุด");
        }

        if (model.EstimatedStartDate > model.EstimatedEndDate)
        {
            ModelState.AddModelError("", "วันที่เริ่มต้นต้องน้อยกว่าวันที่สิ้นสุด");
        }

        // คำนวณจำนวนชั่วโมงที่ใช้
        if (model.ActualCompletionDate.HasValue)
        {
            model.HoursSpent = (decimal)(model.ActualCompletionDate.Value - model.EstimatedStartDate).TotalHours;
            model.ResolutionStatus = "Completed";
        }
        else
        {
            model.ResolutionStatus = "In Progress";
        }

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                Console.WriteLine($"Model Error: {error.ErrorMessage}");
            }
            
            ViewBag.Assignment = assignment;
            ViewBag.Components = GetComponentOptions();
            return View(model);
        }

        try
        {
            model.CreatedAt = DateTime.Now;
            model.UpdatedAt = DateTime.Now;
            
            _context.AssignmentResolutions.Add(model);
            await _context.SaveChangesAsync();

            Console.WriteLine("Successfully saved assignment resolution!");
            
            return RedirectToAction("Index", new { assignmentId = model.AssignmentId });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving assignment resolution: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            ModelState.AddModelError("", "ไม่สามารถบันทึกข้อมูลได้ กรุณาลองใหม่");
            
            ViewBag.Assignment = assignment;
            ViewBag.Components = GetComponentOptions();
            return View(model);
        }
    }

    // แก้ไขการแก้ไขปัญหา
    public async Task<IActionResult> Edit(int id)
    {
        var resolution = await _context.AssignmentResolutions
            .Include(r => r.Assignment)
            .ThenInclude(a => a.Incident)
            .Include(r => r.Employee)
            .FirstOrDefaultAsync(r => r.ResolutionId == id);

        if (resolution == null)
        {
            return NotFound();
        }

        ViewBag.Assignment = resolution.Assignment;
        ViewBag.Components = GetComponentOptions();
        
        return View(resolution);
    }

    // บันทึกการแก้ไข
    [HttpPost]
    public async Task<IActionResult> Edit(AssignmentResolution model)
    {
        var resolution = await _context.AssignmentResolutions.FindAsync(model.ResolutionId);
        if (resolution == null)
        {
            return NotFound();
        }

        // ตรวจสอบวันที่
        if (model.EstimatedStartDate > model.EstimatedEndDate)
        {
            ModelState.AddModelError("", "วันที่เริ่มต้นต้องน้อยกว่าวันที่สิ้นสุด");
            ViewBag.Assignment = await _context.IncidentAssignments.FindAsync(model.AssignmentId);
            ViewBag.Components = GetComponentOptions();
            return View(model);
        }

        try
        {
            resolution.Component = model.Component;
            resolution.ResolutionDetails = model.ResolutionDetails;
            resolution.EstimatedStartDate = model.EstimatedStartDate;
            resolution.EstimatedEndDate = model.EstimatedEndDate;
            resolution.ActualCompletionDate = model.ActualCompletionDate;
            resolution.Notes = model.Notes;
            resolution.CompletionNotes = model.CompletionNotes;
            resolution.Priority = model.Priority;
            resolution.ResolutionStatus = model.ResolutionStatus;
            resolution.UpdatedAt = DateTime.Now;

            // คำนวณจำนวนชั่วโมงที่ใช้
            if (model.ActualCompletionDate.HasValue)
            {
                resolution.HoursSpent = (decimal)(model.ActualCompletionDate.Value - resolution.EstimatedStartDate).TotalHours;
                resolution.ResolutionStatus = "Completed";
            }

            await _context.SaveChangesAsync();
            
            return RedirectToAction("Index", new { assignmentId = resolution.AssignmentId });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating assignment resolution: {ex.Message}");
            ModelState.AddModelError("", "ไม่สามารถแก้ไขข้อมูลได้ กรุณาลองใหม่");
            
            ViewBag.Assignment = await _context.IncidentAssignments.FindAsync(model.AssignmentId);
            ViewBag.Components = GetComponentOptions();
            return View(model);
        }
    }

    // ลบการแก้ไขปัญหา
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var resolution = await _context.AssignmentResolutions.FindAsync(id);
        if (resolution != null)
        {
            _context.AssignmentResolutions.Remove(resolution);
            await _context.SaveChangesAsync();
        }
        
        return RedirectToAction("Index", new { assignmentId = resolution?.AssignmentId });
    }

    // Helper methods
    private List<string> GetComponentOptions()
    {
        return new List<string>
        {
            "Server",
            "Hardware", 
            "Software สำเร็จรูป",
            "ระบบ Application",
            "Network",
            "Database",
            "Security",
            "Backup System",
            "Email System",
            "File Server",
            "Web Server",
            "Application Server",
            "Other"
        };
    }
}
