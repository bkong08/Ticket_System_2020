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
        private Entities1 db = new Entities1();

        public object EmployeeNames { get; private set; }

        // GET: Tickets
        public ActionResult Index(string option, string search)
        {

            if (option == "projectName")
            {
                //Index action method will return a view with a student records based on what a user specify the value in textbox  
                return View(db.Tickets.Where(x => x.ProjetName.StartsWith(search) || search == null).ToList());
            }
            else if (option == "department")
            {
                return View(db.Tickets.Where(x => x.DepartmentName.StartsWith(search) || search == null).ToList());
            }
            else if (option == "submittedBy")
            {
                return View(db.Tickets.Where(x => x.RequestName.StartsWith(search) || search == null).ToList());
            }
            else if (option == "problemDescription")
            {
                return View(db.Tickets.Where(x => x.ProblemDescription.StartsWith(search) || search == null).ToList());
            }
            else
            {
                return View(db.Tickets.Where(x => x.TimeRequested.ToString().StartsWith(search) || search == null).ToList());
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
    }
}
