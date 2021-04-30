using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace dotnet_utils.Repository
{
    /// <summary>  
    /// Generic Repository class for Entity Operations  
    /// </summary>  
    /// <typeparam name="TEntity"></typeparam>  
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly SampleContext context;
        private readonly DbSet<TEntity> dbSet;

        /// <summary>  
        /// Public Constructor,initializes privately declared local variables.  
        /// </summary>  
        /// <param name="context"></param>  
        public GenericRepository(SampleContext _context)
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
        public virtual List<TEntity> Get(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            params Expression<Func<TEntity, object>>[] includes)
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
        /// Get all entities from db
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = dbSet;

            foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return await query.ToListAsync();
        }

        /// <summary>  
        /// generic Get method for Entities  
        /// </summary>  
        /// <returns></returns>  
        public virtual async Task<IEnumerable<TEntity>> GetAsync()
        {
            IQueryable<TEntity> query = dbSet;
            return await query.ToListAsync();
        }

        /// <summary>  
        /// generic get method , fetches data for the entities on the basis of condition.  
        /// </summary>  
        /// <param name="where"></param>  
        /// <returns></returns>  
        public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> where)
        {
            IQueryable<TEntity> query = dbSet;

            return await query.FirstOrDefaultAsync(where);
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
        /// Generic get method on the basis of id for Entities.  
        /// </summary>  
        /// <param name="id"></param>  
        /// <returns></returns>  
        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }

        /// <summary>  
        /// generic method to fetch all the records from db  
        /// </summary>  
        /// <returns></returns>  
        public IEnumerable<TEntity> GetAll()
        {
            return dbSet.ToList();
        }

        /// <summary>  
        /// generic method to fetch all the records from db  
        /// </summary>  
        /// <returns></returns>  
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        /// <summary>
        /// Get first or default entity by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public virtual TEntity GetFirstOrDefault(
            Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = dbSet;

            foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);

            return query.FirstOrDefault(filter);
        }

        /// <summary>
        /// Get first or default entity by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetFirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = dbSet;

            foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);

            return await query.FirstOrDefaultAsync(filter);
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
        /// generic method to get many record on the basis of a condition.  
        /// </summary>  
        /// <param name="where"></param>  
        /// <returns></returns>  
        public virtual async Task<IEnumerable<TEntity>> GetManyAsync(Func<TEntity, bool> where)
        {
            IQueryable<TEntity> query = dbSet;

            query = (IQueryable<TEntity>)query.Where(where);

            return await query.ToListAsync();
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
        /// generic method to get many record on the basis of a condition but query able.  
        /// </summary>  
        /// <param name="where"></param>  
        /// <returns></returns>  
        public virtual async Task<IQueryable<TEntity>> GetManyQueryableAsync(Expression<Func<TEntity, bool>> where)
        {
            IQueryable<TEntity> query = dbSet;

            query = (IQueryable<TEntity>)await query.Where(where).ToListAsync();

            return query.AsQueryable();
        }

        /// <summary>  
        /// Include multiple  
        /// </summary>  
        /// <param name="predicate"></param>  
        /// <param name="include"></param>  
        /// <returns></returns>  
        public IQueryable<TEntity> GetWithInclude(
            Expression<Func<TEntity, bool>> predicate,
            params string[] include)
        {
            IQueryable<TEntity> query = dbSet;
            query = include.Aggregate(query, (current, inc) => current.Include(inc));
            return query.Where(predicate);
        }

        public async Task<IQueryable<TEntity>> GetWithIncludeAsync(
            Expression<Func<TEntity, bool>> predicate,
            params string[] include)
        {
            IQueryable<TEntity> query = dbSet;

            query = include.Aggregate(query, (current, inc) => current.Include(inc));
            query = (IQueryable<TEntity>)await query.Where(predicate).ToListAsync();

            return query.AsQueryable();
        }

        /// <summary>
        /// Get query for entity
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> Query(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return query;
        }

        /// <summary>
        /// Get query for entity
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public virtual async Task<IQueryable<TEntity>> QueryAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
                query = (IQueryable<TEntity>)await query.Where(filter).ToListAsync();

            if (orderBy != null)
                query = orderBy(query);

            return query.AsQueryable();
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
        /// Insert entity to db
        /// </summary>
        /// <param name="entity"></param>
        public virtual async Task InsertAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
            context.Entry(entity).State = EntityState.Added;
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
        /// Update entity in db
        /// </summary>
        /// <param name="entity"></param>
        public virtual async Task UpdateAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
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
        /// Delete entity from db by primary key
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task DeleteAsync(object id)
        {
            TEntity entityToDelete = await dbSet.FindAsync(id);
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
        public async Task DeleteAsync(Func<TEntity, bool> where)
        {
            IQueryable<TEntity> query = dbSet;

            query = (IQueryable<TEntity>)query.Where(where);

            foreach (TEntity obj in await query.ToListAsync())
            {
                dbSet.Remove(obj);
            }
        }

        /// <summary>
        /// Executes a stored procedure
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> ExecWithStoreProcedure(string query, params object[] parameters)
        {
            return dbSet.FromSqlRaw(query, parameters).ToList();
        }

        /// <summary>
        /// Executes a stored procedure
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> ExecWithStoreProcedureAsync(string query, params object[] parameters)
        {
            return await dbSet.FromSqlRaw(query, parameters).ToListAsync();
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

        /// <summary>  
        /// Generic method to check if entity exists  
        /// </summary>  
        /// <param name="primaryKey"></param>  
        /// <returns></returns>  
        public async Task<bool> ExistsAsync(object primaryKey)
        {
            return await dbSet.FindAsync(primaryKey) != null;
        }

    }
}
