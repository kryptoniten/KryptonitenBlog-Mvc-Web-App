using KryptonitenBlog.BusinessLayer;
using KryptonitenBlog.Entities;
using KryptonitenBlog.Entities.ValueObjects;
using System;
using System.Linq;
using System.Net;
using KryptonitenBlog.Entities.Messages;
using System.Web.Mvc;
using System.Collections.Generic;
using Blogger.ViewModels;
using System.Web;
using KryptonitenBlog.BusinessLayer.Results;
using Blogger.Models;

namespace Blogger.Controllers
{
    public class HomeController : Controller
    {
       
        public ActionResult Index()
        {
            NoteManager nm = new NoteManager();

            return View(nm.ListQueryable().OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CategoryManager cm = new CategoryManager();
            Category cat = cm.Find(x=> x.Id == id.Value);
            if (cat == null)
            {
                return new HttpNotFoundResult();
            }

            return View("Index", cat.Notes.OrderByDescending(x => x.ModifiedOn).ToList());
        } 

        public ActionResult MostLiked()
        {
            NoteManager nm = new NoteManager();

            return View("Index", nm.ListQueryable().OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult ShowProfile()
        {
            
            BlogUserManager eum = new BlogUserManager();

            BusinessLayerResult<BlogUser> res = eum.GetUserById(SessionManager.User.Id);
            if(res.Errors.Count > 0)
            {
                ErrorViewModel errornotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };
                return View("Error", errornotifyObj);
            }


            return View(res.Result);
        }
        public ActionResult EditProfile()
        {
            
            BlogUserManager eum = new BlogUserManager();

            BusinessLayerResult<BlogUser> res = eum.GetUserById(SessionManager.User.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errornotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };
                return View("Error", errornotifyObj);
            }


            return View(res.Result);
        }

        [HttpPost]
        public ActionResult EditProfile(BlogUser model, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifierUserName");

           if(ModelState.IsValid)
            {
                if (ProfileImage != null &&
               (ProfileImage.ContentType == "image/jpeg"
               || ProfileImage.ContentType == "image/jpg" ||
               ProfileImage.ContentType == "image/png"))
                {

                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    model.ProfileImageName = filename;

                }



                BlogUserManager eum = new BlogUserManager();
                BusinessLayerResult<BlogUser> res = eum.UpdateProfile(model);


                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellenemedi",
                        RedirecingtUrl = "/Home/EditProfile"
                    };
                    return View("Error", errorNotifyObj);
                }


                SessionManager.Set<BlogUser>("login", res.Result);
                return RedirectToAction("ShowProfile");

            }
            return View(model);


        }
             
        public ActionResult DeleteProfile()
        {
           
            BlogUserManager eum = new BlogUserManager();
            BusinessLayerResult<BlogUser> res = eum.RemoveUserById(SessionManager.User.Id);
            if(res.Errors.Count > 0)
            {
                ErrorViewModel ErrorNotifyObj = new ErrorViewModel()
                {
                    Items = res.Errors,
                    Title = "Profil Silinemedi",
                    RedirecingtUrl = "/Home/ShowProfile"
                };
                return View("Error", ErrorNotifyObj);
            }
            Session.Clear();
            return RedirectToAction("Index");
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)  
        {
            if (ModelState.IsValid)
            {
                BlogUserManager eum = new BlogUserManager();
                BusinessLayerResult<BlogUser> res = eum.LoginUser(model);

                if (res.Errors.Count > 0)
                {

                    if (res.Errors.Find(x => x.Code == ErrorMessageCode.UserIsNotActive) != null)
                    {
                        ViewBag.SetLink  = "https://Home/Activate/1234-4567-7890";
                    }

                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                SessionManager.Set<BlogUser>("login", res.Result);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)   
        {
            if (ModelState.IsValid)
            {
                
                BlogUserManager eum = new BlogUserManager();
                BusinessLayerResult<BlogUser> res = eum.RegisterUser(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirecingtUrl = "/Home/Login",
                    


                };
                notifyObj.Items.Add("Lütfen E-posta Adresinize gönderdiğimiz aktivasyon linkine tıklayarak hesabınızı aktif hale getiriniz");
                return  View("Ok",notifyObj);
            }
            

            return View(model);
        }

        public ActionResult UserActive(Guid id)    
        {
            BlogUserManager eum = new BlogUserManager();
           BusinessLayerResult<BlogUser> res = eum.ActivateUser(id);
            if(res.Errors.Count > 0)
            {
                ErrorViewModel errornotifyObj = new ErrorViewModel()
                {
                    Title = "Geçersiz İşlem",
                    Items = res.Errors
                };
                return View("Error", errornotifyObj);
            }
            OkViewModel OknotifyObj = new OkViewModel()
            {
                Title = "Hesap Aktifleştirildi",
                 RedirecingtUrl = "/Home/Login",
                 
            };
            OknotifyObj.Items.Add("Hesabınız aktifleştirildi artık not paylaşabilir ve beğenme yapabilirsiniz");

            return View("Ok",OknotifyObj);
        }
        
        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction("Index");
        }



        public ActionResult Testnotify()
        {

            OkViewModel model = new OkViewModel()
            {
                Header = "Yönlendirme",
                Title = "Ok test",
                RedirectingTimeout = 3000,
              
            };
            return View("Ok",model );
        }

    }
}