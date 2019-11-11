
using BankProject.Data.Model;
using BankProject.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankProject.Data.Repository
{
    public class BaseUnitOfWork : IUnitOfWork
    {
        private bool disposed = false;
        private readonly BaseContext _dbContext;

        public BaseUnitOfWork(BaseContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException($"{nameof(dbContext)} can not be null");

            _dbContext = dbContext;
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            return new BaseRepository<T>(_dbContext);
        }

        public int SaveChanges()
        {
            try
            {
                return _dbContext.SaveChanges();
            }
            catch (OptimisticConcurrencyException ex)
            {
                //TODO : handle exceptions
                throw;
            }
            catch (CommitFailedException ex)
            {
                //TODO : handle exceptions
                return _dbContext.SaveChanges();
            }
            catch (EntityException ex)
            {
                //TODO : handle exceptions
                return _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                //TODO : handle exceptions
                throw;
            }
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
