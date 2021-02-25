using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;

namespace dotnet_utils.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ILog _log = null;
        private Test_Entities _context = null;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public UnitOfWork()
        {
            _context = new Test_Entities();
            _context.Database.CommandTimeout = 180;

            if (_log == null)
            {
                log4net.Config.XmlConfigurator.Configure();
                _log = LogManager.GetLogger(typeof(UnitOfWork));
            }
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            if (_repositories.Keys.Contains(typeof(T)) == true)
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
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    _log.ErrorFormat("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State);

                    foreach (var ve in eve.ValidationErrors)
                    {
                        _log.ErrorFormat("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    }
                }

                throw e;
            }
        }

        private bool disposed = false;

        /// <summary>  
        /// Protected Virtual Dispose method  
        /// </summary>  
        /// <param name="disposing"></param>  
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    System.Diagnostics.Debug.WriteLine("UnitOfWork is being disposed");
                    _context.Dispose();
                }
            }
            this.disposed = true;
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
