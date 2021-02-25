using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace dotnet_utils.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Get all entities from db
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        List<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Get all entities from db
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> Get();

        /// <summary>
        /// Get one entities from db
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        TEntity Get(Expression<Func<TEntity, bool>> where);

        /// <summary>
        /// Get single entity by primary key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity GetById(object id);

        /// <summary>
        /// Get all entities from db
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// Get first or default entity by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        TEntity GetFirstOrDefault(
            Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Get many entities from db with parameters
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        IEnumerable<TEntity> GetMany(Func<TEntity, bool> where);

        /// <summary>
        /// Get many queryable entities from db with parameters
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        IQueryable<TEntity> GetManyQueryable(Expression<Func<TEntity, bool>> where);

        /// <summary>
        /// Include multiple  
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        IQueryable<TEntity> GetWithInclude(Expression<Func<TEntity, bool>> predicate, params string[] include);

        /// <summary>
        /// Get query for entity
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        /// <summary>
        /// Insert entity to db
        /// </summary>
        /// <param name="entity"></param>
        void Insert(TEntity entity);

        /// <summary>
        /// Update entity in db
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);

        /// <summary>
        /// Delete entity from db by primary key
        /// </summary>
        /// <param name="id"></param>
        void Delete(object id);

        /// <summary>
        /// generic delete method , deletes data for the entities on the basis of condition.  
        /// </summary>
        /// <param name="where"></param>
        void Delete(Func<TEntity, bool> where);

        /// <summary>
        /// Generic Delete method for the entities  
        /// </summary>
        /// <param name="entityToDelete"></param>
        void Delete(TEntity entityToDelete);

        /// <summary>
        /// Executes a stored procedure
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<TEntity> ExecWithStoreProcedure(string query, params object[] parameters);

        /// <summary>
        /// Generic method to check if entity exists 
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        bool Exists(object primaryKey);
    }
}
