using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KryptonitenBlog.Entities;
using KryptonitenBlog.BusinessLayer;
using KryptonitenBlog.BusinessLayer.Results;
using Blogger.Models;

namespace Blogger.Controllers
{
    public class NoteController : Controller
    {
         NoteManager NoteManager = new NoteManager();
        CategoryManager CategoryManager = new CategoryManager();
        LikedManager LikedManager = new LikedManager();
        
        public ActionResult Index()
        {

            var notes = NoteManager.ListQueryable().Include("Category").Include("Owner").Where(
                x => x.Owner.Id == SessionManager.User.Id).OrderByDescending(
                x => x.ModifiedOn);
            
            return View(notes.ToList());
        }

        public ActionResult MyLikedNotes()
        {
            var notes = LikedManager.ListQueryable().Include("LikedUser").Include("Note").Where(
                x => x.LikedUser.Id == SessionManager.User.Id).Select(
                x => x.Note).Include("Category").Include("Owner").OrderByDescending(
                x => x.ModifiedOn);

          

            return View("Index",notes.ToList());

        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = NoteManager.Find(x => x.Id == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Note note)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifierUsername");
            if (ModelState.IsValid)
            {
                note.Owner = SessionManager.User;
                NoteManager.Insert(note);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = NoteManager.Find(x => x.Id == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Note note)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifierUsername"); 
            if (ModelState.IsValid)
            {
                Note db_note = NoteManager.Find(x => x.Id == note.Id);
                db_note.IsDraft = note.IsDraft;
                db_note.Category = note.Category;
                db_note.Text = note.Text;
                db_note.Title = note.Title;
                NoteManager.Update(db_note);

                return RedirectToAction("Index");

            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = NoteManager.Find(x => x.Id == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }
         
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Note note = NoteManager.Find(x => x.Id == id);
            NoteManager.Delete(note);
            return RedirectToAction("Index");
        }

       
    }
}
