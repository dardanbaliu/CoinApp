using System;
using System.Collections.Generic;
using System.Text;

namespace CoinApp.Infrastructure.Caching
{
    public interface ICacheKey<TItem>
    {
        string CacheKey { get; }
    }
}
