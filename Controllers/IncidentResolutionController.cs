using DOTNETCORE_DEV.Data;
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
        return View(new IncidentResolution { IncidentId = incidentId });
    }

    // บันทึกข้อมูลการแก้ไขลงฐานข้อมูล
    [HttpPost]
    public async Task<IActionResult> Create(IncidentResolution model)
    {
            model.HoursSpent = (model.ActualCompletionDate - model.EstimatedStartDate)?.TotalHours ?? 0;
            _context.IncidentResolutions.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { incidentId = model.IncidentId });
}
}