using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KGA.Models;


namespace KGA.Controllers
{
    [Authorize]
    public class ProblemsDataController : Controller
    {
        private KnowledgeGarden_AEntities db = new KnowledgeGarden_AEntities();

        // GET: ProblemsData
        public ActionResult Index()
        {
            return View(db.ProblemsDatas.ToList());
        }

        // GET: ProblemsData/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProblemsData problemsData = db.ProblemsDatas.Find(id);
            if (problemsData == null)
            {
                return HttpNotFound();
            }
            return View(problemsData);
        }

        public ActionResult Remove(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attachment attachment = db.Attachments.Find(id);
            ProblemsData problemsData = db.ProblemsDatas.Find(id);
            if (attachment == null)
            {
                return HttpNotFound();
            }
            int? path = attachment.ProblemID;
            db.Attachments.Remove(attachment);
            db.SaveChanges();
            return RedirectToAction("Edit",new { id = path});
        }

        public ActionResult Download(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Attachment attachment = db.Attachments.Find(id);
            AttachmentsBusinessModel fileDownload = new AttachmentsBusinessModel();
            if (attachment == null)
            {
                return HttpNotFound();
            }

            fileDownload.DownloadFile(attachment.FileID);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = fileDownload.attachment.ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileDownload.attachment.FileName);
            Response.BinaryWrite(fileDownload.attachment.Data);
            Response.Flush();
            Response.End();
            return View();
        }
  
        // GET: ProblemsData/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProblemsData/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProblemID,Title,ProblemDescription")] ProblemsData problemsData, HttpPostedFileBase filePosted)
        {
            //AttachmentsBusinessModel fileDownload = new AttachmentsBusinessModel();
            AttachmentsBusinessModel fileUpload = new AttachmentsBusinessModel();

            if (ModelState.IsValid)
            {
                problemsData.UserName = User.Identity.Name;
                problemsData.DateAndTime = DateTime.Now;
                if (problemsData.Title == null)
                {
                    problemsData.Title = "Empty";
                }
                if (problemsData.ProblemDescription == null)
                {
                    problemsData.ProblemDescription = "Empty";
                }
                db.ProblemsDatas.Add(problemsData);
                db.SaveChanges();

                fileUpload.attachment.UserName = User.Identity.Name;
                fileUpload.attachment.ProblemID = problemsData.ProblemID;
                fileUpload.uploadData(filePosted);

                return RedirectToAction("Index");
            }
            return View(problemsData);
        }

        // GET: ProblemsData/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProblemsData problemsData = db.ProblemsDatas.Find(id);
            if (problemsData == null)
            {
                return HttpNotFound();
            }
            return View(problemsData);
        }

        // POST: ProblemsData/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProblemID,Title,ProblemDescription")] ProblemsData problemsData, HttpPostedFileBase filePosted)
        {
            AttachmentsBusinessModel fileUpload = new AttachmentsBusinessModel();

            if (ModelState.IsValid)
            {
                problemsData.UserName = User.Identity.Name;
                problemsData.DateAndTime = DateTime.Now;
                if (problemsData.Title == null)
                {
                    problemsData.Title = "Empty";
                }
                if (problemsData.ProblemDescription == null)
                {
                    problemsData.ProblemDescription = "Empty";
                }

                db.Entry(problemsData).State = EntityState.Modified;
                db.SaveChanges();

                fileUpload.attachment.UserName = User.Identity.Name;
                fileUpload.attachment.ProblemID = problemsData.ProblemID;
                fileUpload.uploadData(filePosted);

                return RedirectToAction("Index");
            }
            return View(problemsData);
        }

        // GET: ProblemsData/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProblemsData problemsData = db.ProblemsDatas.Find(id);
            if (problemsData == null)
            {
                return HttpNotFound();
            }
            return View(problemsData);
        }

        // POST: ProblemsData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProblemsData problemsData = db.ProblemsDatas.Find(id);
            db.ProblemsDatas.Remove(problemsData);
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
