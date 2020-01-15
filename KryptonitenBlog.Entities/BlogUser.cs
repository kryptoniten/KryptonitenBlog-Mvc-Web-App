using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KryptonitenBlog.Entities
{
    [Table("BlogUsers")]
    public class BlogUser : BlogEntityBase
    {
        [DisplayName("isim"), StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır")]
        public string Name { get; set; }

        [DisplayName("Soyisim"), StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır")]
        public string Surname { get; set; }

        [DisplayName("Kullanıcı Adı"), Required(ErrorMessage = "{0} Alanı Gereklidir"), StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır")]
        public string Username { get; set; }

        [DisplayName("E-Mail Adresi"), Required(ErrorMessage = "{0} Alanı Gereklidir"), StringLength(80, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır")]
        public string Email { get; set; }

        [DisplayName("Şifre"),
            Required(ErrorMessage = "{0} Alanı Gereklidir"), StringLength(100, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır")]
        public string Password { get; set; }

        [StringLength(45),ScaffoldColumn(false)]
        public string ProfileImageName { get; set; }
        [DisplayName("Is Active")]
        public bool IsActive { get; set; }
    [DisplayName("Is Admin?")]
        public bool IsAdmin { get; set; }

        [Required,ScaffoldColumn(false)]
        public Guid ActiveGuid { get; set; }

        public virtual List<Note> Notes { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }
    }
}