using BankProject.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BankProject.Data.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        private readonly BaseContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(BaseContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext can not be null");

            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Find(predicate);
        }
        public T Max(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).Max();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
            if (entity == null)
                return;

            Delete(entity);
        }

        public void Delete(T entity)
        {
            DbEntityEntry dbEntityEntry = _dbContext.Entry(entity);

            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                _dbSet.Attach(entity);
                _dbSet.Remove(entity);

                //PropertyInfo[] props = typeof(T).GetProperties();
                //int queryID = (int)entity.GetType().GetProperty(props.FirstOrDefault(x => x.Name == "Id").Name).GetValue(entity, null);

                //var entry = (IModelBase)_dbContext.Set<T>().Find(queryID);
                //entry.Status = 0;
            }
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;

        }

        
    }
}
