using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KryptonitenBlog.BusinessLayer;
using KryptonitenBlog.BusinessLayer.Results;
using KryptonitenBlog.Entities;

namespace Blogger.Controllers
{
    public class BlogUserController : Controller
    {
        private BlogUserManager BlogUserManager = new BlogUserManager();

        public ActionResult Index()
        {
            return View(BlogUserManager.List());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogUser blogUser = BlogUserManager.Find(x => x.Id == id.Value);
            if (blogUser == null)
            {
                return HttpNotFound();
            }
            return View(blogUser);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BlogUser blogUser)
        {

            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifierUsername");


            if (ModelState.IsValid)
            {
                BusinessLayerResult<BlogUser> res = BlogUserManager.Insert(blogUser);
                if(res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("",x.Message));
                    return View(blogUser);
                }

                return RedirectToAction("Index");
            }

            return View(blogUser);
        }

       
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogUser blogUser = BlogUserManager.Find(x => x.Id == id.Value);
            if (blogUser == null)
            {
                return HttpNotFound();
            }
            return View(blogUser);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BlogUser blogUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifierUsername");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<BlogUser> res = BlogUserManager.Update(blogUser); 
                if(res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(blogUser);
                }

                
                return RedirectToAction("Index");
            }
            return View(blogUser);
        }

        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogUser blogUser = BlogUserManager.Find(x => x.Id == id.Value);
            if (blogUser == null)
            {
                return HttpNotFound();
            }
            return View(blogUser);
        }

        


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogUser blogUser = BlogUserManager.Find(x => x.Id == id);
           BlogUserManager.Delete(blogUser);
            BlogUserManager.Save();
            return RedirectToAction("Index");
        }

      
    }
}
