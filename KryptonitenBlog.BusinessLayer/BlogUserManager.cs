using KryptonitenBlog.DataAccessLayer.EntityFramework;
using KryptonitenBlog.Entities;
using KryptonitenBlog.Entities.ValueObjects;
using KryptonitenBlog.Entities.Messages;
using KryptonitenBlog.Common.Helpers;
using System;
using KryptonitenBlog.BusinessLayer.Results;
using KryptonitenBlog.BusinessLayer.Abstract;

namespace KryptonitenBlog.BusinessLayer
{
    public class BlogUserManager : ManagerBase<BlogUser>
    {

        public BusinessLayerResult<BlogUser> RegisterUser(RegisterViewModel data)
        {
            

            BlogUser user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<BlogUser> res = new BusinessLayerResult<BlogUser>();

            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists,"Kullanıcı Adı Kayıtlıdır");
                }
                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "Email Adresi Kayıtlıdır");
                }
            }
            else
            {
                int dbResult = base.Insert(new BlogUser()
                {
                    Username = data.Username,
                    Email = data.Email,
                    ProfileImageName = "user.png",
                    Password = data.Password,
                    ActiveGuid = Guid.NewGuid(),
                    IsActive = false,
                    IsAdmin = false
                });
                if (dbResult > 0)
                {
                    res.Result = Find(x => x.Email == data.Email && x.Username == data.Username);
                    //TODO : AKTİVASYON MAİLİ ATILACAK

                    string SiteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{SiteUri}/Home/UserActive/{res.Result.ActiveGuid}";
                    string body = $"Merhaba  {res.Result.Username};<br><br> Hesabınızı aktif hale getirmek için <a href='{activateUri}' target='_blank' >tıklayınız</a>.";
                    MailHelper.SendMail(body,res.Result.Email,"Kryptoniten Blog Hesap Aktivasyonu");
                  
                }
            }

            return res;
        }

        public BusinessLayerResult<BlogUser > GetUserById(int id)
        {
            BusinessLayerResult<BlogUser> res = new BusinessLayerResult<BlogUser>();
            res.Result = Find(x => x.Id == id);
            if(res.Result == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı Bulunamadı");
            }


            return res;
         }

        public BusinessLayerResult<BlogUser> LoginUser(LoginViewModel data)
        {
            BusinessLayerResult<BlogUser> res = new BusinessLayerResult<BlogUser>();
            res.Result = Find(x => x.Username == data.Username && x.Password == data.Password);

            if (res.Result != null)
            {
                if (!res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserIsNotActive, "Kullanıcı Aktif Edilmemiştir");
                    res.AddError(ErrorMessageCode.CheckYourEmail, "Lütfen E-Posta Adresinizi Kontrol Ediniz");
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserNameOrPassWrong, "Kullanıcı Adı ve Şifre Eşleşmiyor ");
            }

            return res;
        }

        public BusinessLayerResult<BlogUser> UpdateProfile(BlogUser data)
        {

                
            BlogUser db_user = Find(x => x.Id != data.Id && (x.Username == data.Username || x.Email == data.Email));
            BusinessLayerResult<BlogUser> res = new BusinessLayerResult<BlogUser>();
            if(db_user !=null && db_user.Id != data.Id)
            {
                if(db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı Adı Kayıtlı");
                }
                if(db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "Email adresi kayıtlı");

                }
                return res;

            }
            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;
            if(String.IsNullOrEmpty(data.ProfileImageName)==false)
            {
                res.Result.ProfileImageName = data.ProfileImageName;
            }
            if(base.Update(res.Result)==0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Profil Güncellenemedi");

            }
            return res;
        
        
        
        }

        public BusinessLayerResult<BlogUser> RemoveUserById(int id)
        {
            BusinessLayerResult<BlogUser> res = new BusinessLayerResult<BlogUser>();
            BlogUser user = Find(x => x.Id == id);
            if(user != null)
            {
                if (Delete(user) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı Silinemedi");
                    return res;
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFound, "Kullanıcı Bulunamadı");
            }
            return res;
        }

        public BusinessLayerResult<BlogUser> ActivateUser(Guid activateId)
        {
            BusinessLayerResult<BlogUser> res = new BusinessLayerResult<BlogUser>();
            res.Result = Find(x =>x.ActiveGuid == activateId);
            if(res.Result !=null)
            {
                if(res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı zaten aktif edilmiştir");
                    return res;
                }

                res.Result.IsActive = true;
                Update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExists, "Aktifleştirilecek kullanıcı bulunamadı");

            }

            return res;
        }




        public new BusinessLayerResult<BlogUser> Insert(BlogUser data)
        {
            
            BlogUser user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<BlogUser> res = new BusinessLayerResult<BlogUser>();
            res.Result = data;

            if (user != null)
            {
                
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı Adı Kayıtlıdır");
                }
                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "Email Adresi Kayıtlıdır");
                }
            }
            else
            {
                res.Result.ProfileImageName = "user.png";
                res.Result.ActiveGuid = Guid.NewGuid();
               
                
                if(base.Insert(res.Result)== 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotInserted, "Kullanıcı Eklenemedi");
                }
                
                
            }
            
            return res;

        }

        public new BusinessLayerResult<BlogUser> Update(BlogUser data) 
        {
            BlogUser db_user = Find(x => x.Id != data.Id && (x.Username == data.Username || x.Email == data.Email));
            BusinessLayerResult<BlogUser> res = new BusinessLayerResult<BlogUser>();
            res.Result = data;


            if (db_user != null && db_user.Id != data.Id)
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı Adı Kayıtlı");
                }
                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "Email adresi kayıtlı");

                }
                return res;

            }
            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;
            res.Result.IsActive = data.IsActive;
            res.Result.IsAdmin = data.IsAdmin;

           
            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdated, "Kullanıcı Güncellenemedi");

            }
            return res;



            

        }
    }
}