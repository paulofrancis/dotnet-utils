using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_utils.Repository
{
    public class SampleContext : BaseContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(_configuration.GetConnectionString("HostedPayment"));
            base.OnConfiguring(options);

            options.UseLoggerFactory(_loggerFactory);
        }
    }

    public class BaseContext : DbContext
    {
        protected readonly IConfiguration _configuration;
        protected readonly ILoggerFactory _loggerFactory;
        protected readonly ILogger _log;

        public BaseContext() : base()
        {
            //_loggerFactory = AppContainer.Resolve<ILoggerFactory>();
            //_configuration = AppContainer.Resolve<IConfiguration>();

            _log = _loggerFactory.CreateLogger<BaseContext>();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return Database.BeginTransaction();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await Database.BeginTransactionAsync();
        }

        public void Rollback()
        {
            Dispose();
        }

        public async Task RollbackAsync()
        {
            await DisposeAsync();
        }

        public void Commit()
        {
            try
            {
                SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {

                Rollback();
            }
            catch (DbUpdateException ex)
            {

                Rollback();
            }
            catch (Exception ex)
            {
                var msg = "Error trying to commit";
                _log.LogError(msg, ex);

                Rollback();
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                await SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {

                await RollbackAsync();
            }
            catch (DbUpdateException ex)
            {

                await RollbackAsync();
            }
            catch (Exception ex)
            {
                var msg = "Error trying to commit";
                _log.LogError(msg, ex);

                await RollbackAsync();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
