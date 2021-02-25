using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace dotnet_utils.Repository
{
    /// <summary>  
    /// Generic Repository class for Entity Operations  
    /// </summary>  
    /// <typeparam name="TEntity"></typeparam>  
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly Test_Entities context;
        private readonly DbSet<TEntity> dbSet;

        /// <summary>  
        /// Public Constructor,initializes privately declared local variables.  
        /// </summary>  
        /// <param name="context"></param>  
        public GenericRepository(Test_Entities _context)
        {
            this.context = _context;
            this.dbSet = context.Set<TEntity>();
        }

        /// <summary>
        /// Get all entities from db
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public virtual List<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = dbSet;

            foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return query.ToList();
        }

        /// <summary>  
        /// generic Get method for Entities  
        /// </summary>  
        /// <returns></returns>  
        public virtual IEnumerable<TEntity> Get()
        {
            IQueryable<TEntity> query = dbSet;
            return query.ToList();
        }

        /// <summary>  
        /// generic get method , fetches data for the entities on the basis of condition.  
        /// </summary>  
        /// <param name="where"></param>  
        /// <returns></returns>  
        public virtual TEntity Get(Expression<Func<TEntity, bool>> where)
        {
            IQueryable<TEntity> query = dbSet;

            return query.FirstOrDefault(where);
        }

        /// <summary>  
        /// Generic get method on the basis of id for Entities.  
        /// </summary>  
        /// <param name="id"></param>  
        /// <returns></returns>  
        public virtual TEntity GetById(object id)
        {
            return dbSet.Find(id);
        }

        /// <summary>  
        /// generic method to fetch all the records from db  
        /// </summary>  
        /// <returns></returns>  
        public IEnumerable<TEntity> GetAll()
        {
            return dbSet.AsEnumerable();
        }

        /// <summary>
        /// Get first or default entity by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public virtual TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = dbSet;

            foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);

            return query.FirstOrDefault(filter);
        }

        /// <summary>  
        /// generic method to get many record on the basis of a condition.  
        /// </summary>  
        /// <param name="where"></param>  
        /// <returns></returns>  
        public virtual IEnumerable<TEntity> GetMany(Func<TEntity, bool> where)
        {
            return dbSet.Where(where).ToList();
        }

        /// <summary>  
        /// generic method to get many record on the basis of a condition but query able.  
        /// </summary>  
        /// <param name="where"></param>  
        /// <returns></returns>  
        public virtual IQueryable<TEntity> GetManyQueryable(Expression<Func<TEntity, bool>> where)
        {
            return dbSet.Where(where).AsQueryable();
        }

        /// <summary>  
        /// Include multiple  
        /// </summary>  
        /// <param name="predicate"></param>  
        /// <param name="include"></param>  
        /// <returns></returns>  
        public IQueryable<TEntity> GetWithInclude(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate, params string[] include)
        {
            IQueryable<TEntity> query = this.dbSet;
            query = include.Aggregate(query, (current, inc) => current.Include(inc));
            return query.Where(predicate);
        }

        /// <summary>
        /// Get query for entity
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return query;
        }

        /// <summary>
        /// Insert entity to db
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        /// <summary>
        /// Update entity in db
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(TEntity entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Delete entity from db by primary key
        /// </summary>
        /// <param name="id"></param>
        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        /// <summary>  
        /// generic delete method , deletes data for the entities on the basis of condition.  
        /// </summary>  
        /// <param name="where"></param>  
        /// <returns></returns>  
        public void Delete(Func<TEntity, Boolean> where)
        {
            IQueryable<TEntity> objects = dbSet.Where<TEntity>(where).AsQueryable();
            foreach (TEntity obj in objects)
                dbSet.Remove(obj);
        }

        /// <summary>  
        /// Generic Delete method for the entities  
        /// </summary>  
        /// <param name="entityToDelete"></param>  
        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        /// <summary>
        /// Executes a stored procedure
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> ExecWithStoreProcedure(string query, params object[] parameters)
        {
            return context.Database.SqlQuery<TEntity>(query, parameters);
        }

        /// <summary>  
        /// Generic method to check if entity exists  
        /// </summary>  
        /// <param name="primaryKey"></param>  
        /// <returns></returns>  
        public bool Exists(object primaryKey)
        {
            return dbSet.Find(primaryKey) != null;
        }
    }
}
