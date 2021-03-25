using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SparePartRequest.Models;
using SparePartRequest.ViewModels;
using Microsoft.AspNet.Identity;

namespace SparePartRequest.Controllers
{
    public class RequestsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Requests
        [Authorize]
        public async Task<ActionResult> Index()
        {
           
            var requests = db.Requests.Include(r => r.RequestType);
            var userId = User.Identity.GetUserId();
            if (!string.IsNullOrEmpty(userId))
            {
                requests = db.Requests.Where(x => x.ApplicationUserID == userId && x.IsCanceled == false);
            }
            return View(await requests.ToListAsync());
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Requests()
        {
            var requests = db.Requests.Include(r => r.RequestType);
            requests = db.Requests.Where(x => x.IsCanceled == false);
            return View(await requests.ToListAsync());

        }

        // GET: Requests/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = await db.Requests.FindAsync(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // GET: Requests/Create
        public ActionResult Create()
        {
            ViewBag.RequestTypeId = new SelectList(db.RequestTypes, "RequestTypeId", "Name");
            return View();
        }

        // POST: Requests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "RequestId,Title,Desc,RequestTypeId,ApplicationUser")] Request request)
        {
            if (ModelState.IsValid)
            {
                request.ApplicationUserID = User.Identity.GetUserId();
                db.Requests.Add(request);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.RequestTypeId = new SelectList(db.RequestTypes, "RequestTypeId", "Name", request.RequestTypeId);
            return View(request);
        }

        // GET: Requests/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = await db.Requests.FindAsync(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            ViewBag.RequestTypeId = new SelectList(db.RequestTypes, "RequestTypeId", "Name", request.RequestTypeId);
            return View(request);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "RequestId,Title,Desc,IsApproved,IsRejected,IsCanceled,RequestTypeId")] Request request)
        {
            if (ModelState.IsValid)
            {
                db.Entry(request).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.RequestTypeId = new SelectList(db.RequestTypes, "RequestTypeId", "Name", request.RequestTypeId);
            return View(request);
        }

        public async Task<ActionResult> Approval(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ApproveRequest  = await db.Requests.FindAsync(id);
            if (ApproveRequest == null)
            {
                return HttpNotFound();
            }
            ApproveRequest.IsApproved = true; // to check this @index and it is approved without creating a page 
            ApproveRequest.IsRejected = false;
            db.Entry(ApproveRequest).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Requests");
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Approval([Bind(Include = "RequestId,Desc")] ApprovalViewModel requestVM)
        {
            if (ModelState.IsValid)
            {
                var request = await db.Requests.FindAsync(requestVM.RequestId);
                request.IsApproved = true;
                request.IsRejected = false;
                db.Entry(request).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Requests");
            }
         //   ViewBag.RequestTypeId = new SelectList(db.RequestTypes, "RequestTypeId", "Name", request.RequestTypeId);
            return View(requestVM);
        }


        public async Task<ActionResult> Reject(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ApproveRequest = await db.Requests.FindAsync(id);
            if (ApproveRequest == null)
            {
                return HttpNotFound();
            }
            ApproveRequest.IsApproved = false; // to check this @index and it is approved without creating a page 
            ApproveRequest.IsRejected = true;
            db.Entry(ApproveRequest).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Requests");
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Reject([Bind(Include = "RequestId,Desc")] ApprovalViewModel requestVM)
        {
            if (ModelState.IsValid)
            {
                var request = await db.Requests.FindAsync(requestVM.RequestId);
                request.IsApproved = false;
                request.IsRejected = true;
                db.Entry(request).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Requests");
            }
            //   ViewBag.RequestTypeId = new SelectList(db.RequestTypes, "RequestTypeId", "Name", request.RequestTypeId);
            return View(requestVM);
        }


        public async Task<ActionResult> Cancel(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var CancelRequest = await db.Requests.FindAsync(id);
            if (CancelRequest == null)
            {
                return HttpNotFound();
            }
            CancelRequest.IsCanceled = true; // to check this @index and it is canceled without creating a page 
            db.Entry(CancelRequest).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("index");
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Cancel([Bind(Include = "RequestId,Desc")] CancelViewModel requestVM)
        {
            if (ModelState.IsValid)
            {
                var request = await db.Requests.FindAsync(requestVM.RequestId);
                request.IsCanceled = true;
                db.Entry(request).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            //   ViewBag.RequestTypeId = new SelectList(db.RequestTypes, "RequestTypeId", "Name", request.RequestTypeId);
            return View(requestVM);
        }



        // GET: Requests/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = await db.Requests.FindAsync(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Request request = await db.Requests.FindAsync(id);
            db.Requests.Remove(request);
            await db.SaveChangesAsync();
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
