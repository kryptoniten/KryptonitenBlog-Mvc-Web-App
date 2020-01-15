using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KryptonitenBlog.Entities
{

    [Table("Categories")]
    public class Category : BlogEntityBase
    {
 
        [DisplayName("Kategori"),Required(ErrorMessage ="{0} Alanı Gereklidir"),StringLength(50,ErrorMessage ="{0} Alanı Max. {1} Karakter Olmalıdır")]
        public string Title { get; set; }

        [DisplayName("Açıklama"),StringLength(140,ErrorMessage ="{0} Alanı Max. {1} Karakter olmalıdır")]
        public string Description { get; set; }


        public virtual List<Note> Notes { get; set; }
        public Category()
        {
            Notes = new List<Note>();
        }
        




    }
}
