using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace pasteBin.Repositories
{
    public class EntityFrameworkRepository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;
        private DbSet<T> _objectset;

        public EntityFrameworkRepository(DbContext context)
        {
            _context = context;
        }

        protected DbSet<T> ObjectSet
        {
            get
            {
                if (_objectset == null)
                {
                    _objectset = Context.Set<T>();
                }
                return _objectset;
            }
        }

        protected virtual DbContext Context
        {
            get { return _context; }
        }

        public virtual IQueryable<T> GetAll()
        {
            return DbSet;
        }

        public virtual DbSet<T> DbSet { get { return Context.Set<T>(); } }

        public virtual T Add(T entity)
        {
            try
            {
                if (Context.Entry(entity).State != EntityState.Detached && Context.Entry(entity).State != EntityState.Deleted)
                    return entity;

                T res = DbSet.Add(entity);
                _context.SaveChanges();
                return res;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    var title = String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    
                    foreach (var ve in eve.ValidationErrors)
                    {
                        var error = String.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public virtual void Update(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            if (entity == null) return;
            DbSet.Remove(entity);
            _context.SaveChanges();
        }
    }
}