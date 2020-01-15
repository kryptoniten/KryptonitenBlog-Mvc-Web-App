using KryptonitenBlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;
using KryptonitenBlog.Common;
using KryptonitenBlog.Core.DataAccess;

namespace KryptonitenBlog.DataAccessLayer.EntityFramework
{   //REPOSITORY PATTERN 
   public class Repository<T>: RepositoryBase,IDataAccess<T> where T : class
    {

       
        private DbSet<T> _objectSet;
        public Repository()
        {
          

            _objectSet = context.Set<T>();

        }
        public List<T> List()
        {
         
            return _objectSet.ToList();
    
        }
        public IQueryable<T> ListQueryable()
        {

            return _objectSet.AsQueryable<T>();

        }

        public List<T> List(Expression<Func<T,bool>>where )
        {
            return _objectSet.Where(where).ToList();
        }
            
        public int Insert(T obj)
        {
             _objectSet.Add(obj);
            if(obj is BlogEntityBase)
            {
                BlogEntityBase o = obj as BlogEntityBase;
                DateTime now = DateTime.Now;
                o.CreateadOn = now;
                o.ModifiedOn = now;
                o.ModifierUserName = App.Common.GetCurrentUsername(); //TODO : İŞLEM YAPAN KULLANICI ADI YAZILMALI 
            }


            return Save();
        } 

        public int Update(T obj)
        {
            if (obj is BlogEntityBase)
            {
                BlogEntityBase o = obj as BlogEntityBase;
                
                
                o.ModifiedOn = DateTime.Now;
                o.ModifierUserName = App.Common.GetCurrentUsername();//TODO : İŞLEM YAPAN KULLANICI ADI YAZILMALI 
            }

            return Save();
        }
        public int Delete(T obj)
        {

           _objectSet.Remove(obj);
            return Save(); 

        }

        public int Save()
        {
            return context.SaveChanges();
        }

        public T Find(Expression<Func<T,bool>>where)
        {
            return _objectSet.FirstOrDefault(where);
        }

        
            

            




    }
}
