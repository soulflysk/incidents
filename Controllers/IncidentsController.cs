using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DOTNETCORE_DEV.Data;
using DOTNETCORE_DEV.Dtos;
using DOTNETCORE_DEV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace DOTNETCORE_DEV.Controllers
{
    public class IncidentsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public IncidentsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Incident> objList = _db.Incidents.ToList();
            return View(objList);
        }
        public IActionResult Create()
        {
            ViewData["serviceTypeId"] = new SelectList(_db.ServiceTypes, "serviceTypeId", "serviceTypesName");
            return View();
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Incident obj)
        {
            if (ModelState.IsValid)
            {
                _db.Incidents.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            ViewData["serviceTypeId"] = new SelectList(_db.ServiceTypes, "serviceTypeId", "serviceTypesName", obj.serviceTypeId);

            return View(obj);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var incidentFromDb = _db.Incidents.Find(id);
            //var categoryFromDbFirst = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u=>u.Id == id);
            if (incidentFromDb == null)
            {
                return NotFound();
            }
            ViewData["serviceTypeId"] = new SelectList(_db.ServiceTypes, "serviceTypeId", "serviceTypesName");
            return View(incidentFromDb);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, UpdateEmployeeDto employeeDto, Incident obj)
        {
            var existingIncident = _db.Incidents.FirstOrDefault(x => x.IncidentId == id);
            existingIncident.EmployeeId = employeeDto.EmployeeId;
            // existingIncident.serviceTypeId = obj.serviceTypeId;

            _db.SaveChanges();
            TempData["success"] = "Incident updated successfully";
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDb = _db.Incidents.Find(id);
            //var categoryFromDbFirst = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u=>u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.Incidents.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Incidents.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully";
            // return RedirectToAction("Index");
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Details(int id)
    {
        var incident = _db.Incidents.FirstOrDefault(i => i.IncidentId == id);
        if (incident == null)
        {
            return NotFound();
        }
        return View(incident);
    }
    }
}