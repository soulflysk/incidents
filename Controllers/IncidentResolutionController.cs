using DOTNETCORE_DEV.Data;
using DOTNETCORE_DEV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

public class IncidentResolutionController : Controller
{
    private readonly ApplicationDbContext _context;

    public IncidentResolutionController(ApplicationDbContext context)
    {
        _context = context;
    }

    // แสดงประวัติการแก้ไข
    public async Task<IActionResult> Index(int incidentId)
    {
        var resolutions = await _context.IncidentResolutions
            .Where(r => r.IncidentId == incidentId)
            .ToListAsync();

        return View(resolutions);
    }

    // แสดงฟอร์มบันทึกการแก้ไข
    public IActionResult Create(int incidentId)
    {
        Console.WriteLine($"GET Create - IncidentId: {incidentId}");
        return View(new IncidentResolution { IncidentId = incidentId });
    }

    // บันทึกข้อมูลการแก้ไขลงฐานข้อมูล
    [HttpPost]
    public async Task<IActionResult> Create(IncidentResolution model)
    {
        Console.WriteLine("=== CREATE INCIDENT RESOLUTION DEBUG ===");
        Console.WriteLine($"IncidentId: {model.IncidentId}");
        Console.WriteLine($"Component: {model.Component}");
        Console.WriteLine($"ResolutionDetails: {model.ResolutionDetails}");
        Console.WriteLine($"EstimatedStartDate: {model.EstimatedStartDate}");
        Console.WriteLine($"EstimatedEndDate: {model.EstimatedEndDate}");
        Console.WriteLine($"ActualCompletionDate: {model.ActualCompletionDate}");
        
        // ตรวจสอบว่า IncidentId มีอยู่จริงหรือไม่
        var incidentExists = await _context.Incidents.AnyAsync(i => i.IncidentId == model.IncidentId);
        Console.WriteLine($"Incident exists: {incidentExists}");
        
        if (!incidentExists)
        {
            ModelState.AddModelError("", "ไม่พบเหตุการณ์ที่ระบุ กรุณาตรวจสอบอีกครั้ง");
            return View(model);
        }

        // ตรวจสอบและแก้ไขวันที่ที่ไม่ถูกต้อง
        if (model.EstimatedStartDate == DateTime.MinValue)
        {
            ModelState.AddModelError("EstimatedStartDate", "กรุณาเลือกวันที่คาดว่าจะเริ่มแก้ไข");
        }
        
        if (model.EstimatedEndDate == DateTime.MinValue)
        {
            ModelState.AddModelError("EstimatedEndDate", "กรุณาเลือกวันที่คาดว่าจะเสร็จ");
        }

        // ตรวจสอบวันที่เริ่มต้องน้อยกว่าวันที่สิ้นสุด
        if (model.EstimatedStartDate != DateTime.MinValue && model.EstimatedEndDate != DateTime.MinValue)
        {
            if (model.EstimatedStartDate > model.EstimatedEndDate)
            {
                ModelState.AddModelError("", "วันที่คาดว่าจะเริ่มแก้ไขต้องน้อยกว่าวันที่คาดว่าจะเสร็จ");
            }
        }

        // ตรวจสอบวันที่แก้ไขแล้วต้องไม่น้อยกว่าวันที่เริ่ม
        if (model.ActualCompletionDate.HasValue && model.ActualCompletionDate != DateTime.MinValue)
        {
            if (model.ActualCompletionDate < model.EstimatedStartDate)
            {
                ModelState.AddModelError("", "วันที่แก้ไขแล้วเสร็จต้องไม่น้อยกว่าวันที่คาดว่าจะเริ่มแก้ไข");
            }
        }

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                Console.WriteLine($"Model Error: {error.ErrorMessage}");
            }
            return View(model);
        }

        try
        {
            model.HoursSpent = (model.ActualCompletionDate - model.EstimatedStartDate)?.TotalHours ?? 0;
            Console.WriteLine($"HoursSpent calculated: {model.HoursSpent}");
            
            _context.IncidentResolutions.Add(model);
            await _context.SaveChangesAsync();
            Console.WriteLine("Successfully saved incident resolution!");
            
            return RedirectToAction("Index", new { incidentId = model.IncidentId });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving incident resolution: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            ModelState.AddModelError("", "ไม่สามารถบันทึกข้อมูลได้ กรุณาลองใหม่");
            return View(model);
        }
    }
}