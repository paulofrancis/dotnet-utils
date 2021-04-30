using System.Threading.Tasks;

namespace dotnet_utils.Repository
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> Repository<T>() where T : class;
        void Commit();
        Task CommitAsync();
        void Rollback();
        Task RollbackAsync();
    }
}
