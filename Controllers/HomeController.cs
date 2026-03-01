using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DOTNETCORE_DEV.Models;
using DOTNETCORE_DEV.Data;

namespace DOTNETCORE_DEV.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Home
        public async Task<IActionResult> Index(string search, string statusFilter, string serviceTypeFilter, int page = 1)
        {
            var incidents = _context.Incidents
                .Include(i => i.Employee)
                .Include(i => i.ServiceType)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(search))
            {
                incidents = incidents.Where(i => 
                    i.Problem.Contains(search) || 
                    i.Email.Contains(search) || 
                    i.Name.Contains(search));
            }

            if (!string.IsNullOrEmpty(statusFilter))
            {
                incidents = incidents.Where(i => i.Result == statusFilter);
            }

            if (!string.IsNullOrEmpty(serviceTypeFilter))
            {
                incidents = incidents.Where(i => i.ServiceType != null && i.ServiceType.serviceTypesName.Contains(serviceTypeFilter));
            }

            // Pagination
            int pageSize = 10;
            int totalItems = await incidents.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var pagedIncidents = await incidents
                .OrderByDescending(i => i.IncidentId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Load dropdown data
            ViewBag.ServiceTypes = await _context.ServiceTypes.ToListAsync();
            ViewBag.Employees = await _context.Employees.ToListAsync();

            // Pagination data
            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = totalPages;
            ViewData["TotalItems"] = totalItems;

            return View(pagedIncidents);
        }

        // GET: Home/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.ServiceTypes = await _context.ServiceTypes.ToListAsync();
            ViewBag.Employees = await _context.Employees.ToListAsync();
            return View();
        }

        // GET: Home/GetEmployee/{id}
        [HttpGet]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Json(new { 
                employeeId = employee.EmployeeID, 
                name = employee.Name 
            });
        }

        // POST: Home/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IncidentId,Problem,Date,Result,InsideOrOutside,EmployeeId,Email,Name,CompanyName,PhoneNumber,serviceTypeId")] Incident incident)
        {
            Console.WriteLine("=== CREATE INCIDENT DEBUG ===");
            Console.WriteLine($"Reporter Type: {incident.InsideOrOutside}");
            Console.WriteLine($"Employee ID: {incident.EmployeeId}");
            Console.WriteLine($"Email: {incident.Email}");
            Console.WriteLine($"Name: {incident.Name}");
            Console.WriteLine($"Service Type ID: {incident.serviceTypeId}");
            Console.WriteLine($"Problem: {incident.Problem}");
            Console.WriteLine($"Date: {incident.Date}");
            Console.WriteLine($"Result: {incident.Result}");
            
            // Debug: Print all model state errors
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine($"Model Error: {error.ErrorMessage}");
                    Console.WriteLine($"Model Error Exception: {error.Exception}");
                }
            }
            else
            {
                Console.WriteLine("Model is valid, attempting to save...");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Debug: แสดงข้อมูลที่จะบันทึก
                    Console.WriteLine("=== SAVE INCIDENT DEBUG ===");
                    Console.WriteLine($"IncidentId: {incident.IncidentId}");
                    Console.WriteLine($"Problem: {incident.Problem}");
                    Console.WriteLine($"Date: {incident.Date} (Type: {incident.Date.GetType().Name})");
                    Console.WriteLine($"Result: {incident.Result}");
                    Console.WriteLine($"InsideOrOutside: {incident.InsideOrOutside}");
                    Console.WriteLine($"EmployeeId: {incident.EmployeeId}");
                    Console.WriteLine($"Email: {incident.Email}");
                    Console.WriteLine($"Name: {incident.Name}");
                    Console.WriteLine($"CompanyName: {incident.CompanyName}");
                    Console.WriteLine($"PhoneNumber: {incident.PhoneNumber}");
                    Console.WriteLine($"serviceTypeId: {incident.serviceTypeId}");
                    
                    // ตรวจสอบว่ามีข้อมูลที่จำเป็น
                    if (incident.IncidentId == 0)
                    {
                        Console.WriteLine("WARNING: IncidentId is 0, this might indicate a problem with entity creation");
                    }
                    
                    _context.Add(incident);
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"Successfully saved incident with ID: {incident.IncidentId}");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving incident: {ex.Message}");
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }

            ViewBag.ServiceTypes = await _context.ServiceTypes.ToListAsync();
            ViewBag.Employees = await _context.Employees.ToListAsync();
            return View(incident);
        }

        // GET: Home/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incident = await _context.Incidents
                .Include(i => i.Employee)
                .Include(i => i.ServiceType)
                .FirstOrDefaultAsync(m => m.IncidentId == id);

            if (incident == null)
            {
                return NotFound();
            }

            return View(incident);
        }

        // GET: Home/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incident = await _context.Incidents.FindAsync(id);
            if (incident == null)
            {
                return NotFound();
            }

            ViewBag.ServiceTypes = await _context.ServiceTypes.ToListAsync();
            ViewBag.Employees = await _context.Employees.ToListAsync();
            return View(incident);
        }

        // POST: Home/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IncidentId,Problem,Date,Result,InsideOrOutside,EmployeeId,Email,Name,CompanyName,PhoneNumber,serviceTypeId")] Incident incident)
        {
            if (id != incident.IncidentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(incident);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncidentExists(incident.IncidentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ServiceTypes = await _context.ServiceTypes.ToListAsync();
            ViewBag.Employees = await _context.Employees.ToListAsync();
            return View(incident);
        }

        // GET: Home/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incident = await _context.Incidents
                .Include(i => i.Employee)
                .Include(i => i.ServiceType)
                .FirstOrDefaultAsync(m => m.IncidentId == id);

            if (incident == null)
            {
                return NotFound();
            }

            return View(incident);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);
            if (incident != null)
            {
                _context.Incidents.Remove(incident);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool IncidentExists(int id)
        {
            return _context.Incidents.Any(e => e.IncidentId == id);
        }
    }
}
