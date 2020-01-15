using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KryptonitenBlog.DataAccessLayer.EntityFramework;
using KryptonitenBlog.Entities;
using KryptonitenBlog.BusinessLayer.Abstract;

namespace KryptonitenBlog.BusinessLayer
{
    public class CategoryManager : ManagerBase<Category>
    {
        public override int Delete(Category category)
        {
            NoteManager noteManager = new NoteManager();
            LikedManager likedManager = new LikedManager();
            CommentManager commentManager = new CommentManager();
            //Kategori ile bağlantılı notlar silinecek
            foreach (Note note in category.Notes.ToList())
            {
                //Not ile ilişkili beğeniler silinecek
                foreach (Liked like in note.Likes.ToList())
                {
                    likedManager.Delete(like);
                }

                foreach (Comment comment in note.Comments.ToList())
                {
                    commentManager.Delete(comment);
                }


                noteManager.Delete(note);
            }

            return base.Delete(category);
        }
        
    }
}
