using LibraryManagement.Application;
using LibraryManagement.Persistence.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace LibraryManagement.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryManagementContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(LibraryManagementContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default)
        {
            _transaction = await _context.Database.BeginTransactionAsync(ct);
            return _transaction;
        }

        public async Task CommitTransactionAsync(CancellationToken ct = default)
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync(ct);
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken ct = default)
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync(ct);
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }
}
