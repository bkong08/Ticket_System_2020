using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ticket_System_2020.Models;

namespace Ticket_System_2020.Controllers
{
    public class TicketsController : Controller
    {
        private Entities db = new Entities();

        // GET: Tickets
        public ActionResult Index(string option, string search)
        {
            
            if (option == "ProjectName")
            {
                return View(db.Tickets.Where(x=> x.ProjetName == search || search == null).ToList());
            }
            else if (option == "Department")
            {
                return View(db.Tickets.Where(x=> x.DepartmentName == search || search == null).ToList());
            }
            else if (option == "RequestReceived")
            {
                return View(db.Tickets.Where(x => x.TimeRequested.ToString() == search || search == null).ToList());
            }
            else if (option == "EmployeeName")
            {
                return View(db.Tickets.Where(x => x.RequestName == search || search == null).ToList());
            }
            else
            {
                return View(db.Tickets.Where(x=> x.ProblemDescription.StartsWith(search) || search == null).ToList());
            }
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TicketID,ProjetName,DepartmentName,RequestName,ProblemDescription,TimeRequested")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                List<Department> lstDepartment = db.Departments.ToList();
                lstDepartment.Insert(0, new Department { DepartmentID = 0, DepartmentName = "--Select Category--" });

                List<Employee> lstEmployee = new List<Employee>();
                ViewBag.DepartmentId = new SelectList(lstDepartment, "Id", "Name");
                ViewBag.EmployeeId = new SelectList(lstEmployee, "Id", "Name");

                ticket.TimeRequested = DateTime.Now;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TicketID,ProjetName,DepartmentName,RequestName,ProblemDescription,TimeRequested")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult GetEmployeeByDepartmentId(int id)
        {
            List<Employee> employees = new List<Employee>();
            if (id > 0)
            {
                employees = db.Employees.Where(p => p.DepartmentID == id).ToList();

            }
            else
            {
           
            }
            var result = (from r in employees
                          select new
                          {
                              id = r.EmployeeID,
                              name = r.FirstName
                          }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
