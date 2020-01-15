using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KryptonitenBlog.Entities
{
    [Table("Comments")]
    public class Comment : BlogEntityBase
    {
        [Required,StringLength(255)]
        public string Text { get; set; }
         
        public virtual Note Note { get; set; }
        public virtual BlogUser Owner { get; set; }


    }
}
