using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KryptonitenBlog.Entities.ValueObjects
{
    public class RegisterViewModel
    {
        [DisplayName("Kullanıcı Adı"),
            Required(ErrorMessage = "{0} Alanı Boş Geçilemez"),
            StringLength(25, ErrorMessage = ("{0}.max {1} Karakter Olmalı"))]
        public string Username { get; set; }



        [DisplayName("E-Posta"),
              Required(ErrorMessage = "{0} alanı boş geçilemez"),
              StringLength(70, ErrorMessage = "{0} max. {1} karakter olmalı"),
              EmailAddress(ErrorMessage = "{0} alanı için geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }






        [DisplayName("Şifre"), Required(ErrorMessage = "{0} Alanı Boş Geçilemez"),
            DataType(DataType.Password),
            StringLength(25, ErrorMessage = ("{0}.max {1} Karakter Olmalı"))]
        public string Password { get; set; }






        [DisplayName("Şifre Tekrar"),
            Required(ErrorMessage = "{0} Alanı Boş Geçilemez"), DataType(DataType.Password),
            StringLength(25, ErrorMessage = ("{0}.max {1} Karakter Olmalı")), Compare("Password", ErrorMessage = "{0} ile {1} uyuşmuyor")]
        public string RePassword { get; set; }




    }
}