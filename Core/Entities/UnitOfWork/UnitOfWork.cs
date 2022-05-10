using MicroOrm.Dapper.Repositories;
using MicroOrm.Dapper.Repositories.DbContext;
using System;
using System.Collections.Concurrent;
using System.Data;
using System.Data.SqlClient;

namespace Entities.UnitOfWork
{
    public class UnitOfWork : DapperDbContext, IUnitOfWork
    {
        public static string ConnectionString;

        private ConcurrentDictionary<Type, object> Repositories;

        private IDbTransaction Transaction { get; set; }

        private bool IsLocked { get; set; }

        public UnitOfWork()
           : base(new SqlConnection(ConnectionString))
        {

        }

        public IDapperRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (Repositories == null)
            {
                Repositories = new ConcurrentDictionary<Type, object>();
            }

            var type = typeof(TEntity);
            if (!Repositories.ContainsKey(type))
            {
                Repositories[type] = new DapperRepository<TEntity>(Connection);
            }
            return (IDapperRepository<TEntity>)Repositories[type];
        }

        public override IDbTransaction BeginTransaction()
        {
            if (Transaction?.Connection == null)
                IsLocked = false;
            while (IsLocked) ;
            Transaction = Connection.BeginTransaction();
            IsLocked = true;
            return Transaction;
        }

        public void Commit()
        {
            Transaction.Commit();
            IsLocked = false;
            Transaction = null;
        }

        public void Rollback()
        {
            Transaction.Rollback();
            IsLocked = false;
            Transaction = null;
        }

        public void Dispose()
        {
            if (Transaction != null)
                Transaction.Rollback();

            if (InnerConnection != null && InnerConnection.State != ConnectionState.Closed)
                InnerConnection.Close();

            IsLocked = false;
        }
    }
}
