using MicroOrm.Dapper.Repositories;
using System;
using System.Data;

namespace Entities.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IDbConnection Connection { get; }

        IDapperRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        IDbTransaction BeginTransaction();
        
        void Commit();

        void Rollback();
    }
}
