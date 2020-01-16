using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using KryptonitenBlog.Entities;
using KryptonitenBlog.BusinessLayer;

namespace Blogger.Controllers
{
    public class CommentController : Controller
    {
        private NoteManager noteManager = new NoteManager();
        public ActionResult ShowNoteComments(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteManager.Find(x => x.Id == id);
            if(note == null)
            {
                return HttpNotFound();
            }

                 

            return PartialView("_PartialComments",note.Comments);
        }
    }
}