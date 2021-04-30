using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_utils.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly SampleContext _context = null;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public UnitOfWork()
        {
            _context = new SampleContext();
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            if (_repositories.Keys.Contains(typeof(T)))
            {
                return _repositories[typeof(T)] as IGenericRepository<T>;
            }

            IGenericRepository<T> repo = new GenericRepository<T>(_context);
            _repositories.Add(typeof(T), repo);
            return repo;
        }

        /// <summary>  
        /// Save method.  
        /// </summary>  
        public void Commit()
        {
            _context.Commit();
        }

        /// <summary>  
        /// Save method.  
        /// </summary>  
        public async Task CommitAsync()
        {
            await _context.CommitAsync();
        }

        /// <summary>
        /// Rollback method.
        /// </summary>
        public void Rollback()
        {
            _context.Rollback();
        }

        /// <summary>
        /// Rollback method.
        /// </summary>
        public async Task RollbackAsync()
        {
            await _context.RollbackAsync();
        }

        private bool disposed = false;

        /// <summary>  
        /// Protected Virtual Dispose method  
        /// </summary>  
        /// <param name="disposing"></param>  
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                System.Diagnostics.Debug.WriteLine("UnitOfWork is being disposed");
                _context.Dispose();
            }
            disposed = true;
        }

        /// <summary>  
        /// Dispose method  
        /// </summary>  
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
