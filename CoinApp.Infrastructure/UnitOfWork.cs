using CoinApp.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoinApp.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CoinAppContext _auditContext;

        public UnitOfWork(
            CoinAppContext auditContext)
        {
            this._auditContext = auditContext;

        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {

            return await this._auditContext.SaveChangesAsync(cancellationToken);
        }
    }
}
