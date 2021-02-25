namespace dotnet_utils.Repository
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> Repository<T>() where T : class;
        void Save();
    }
}
