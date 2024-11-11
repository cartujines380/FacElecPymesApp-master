using System;

namespace Sipecom.FactElec.Pymes.Infraestructura.Transversal.Cache
{
    public interface ICacheManager
    {
        TItem Get<TItem>(object key);

        TItem Get<TItem>(object key, Func<TItem> factory);

        TItem Get<TItem>(object key, double cacheTime, Func<TItem> factory);

        void Set<TItem>(object key, TItem value, double cacheTime);

        void Set<TItem>(object key, TItem value, double cacheTime, TimeSpan slidingTime);

        void Remove(object key);

        void Clear();
    }
}
