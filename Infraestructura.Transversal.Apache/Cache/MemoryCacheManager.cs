using System;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;

using Sipecom.FactElec.Pymes.Infraestructura.Transversal.Cache;


namespace Sipecom.FactElec.Pymes.Infraestructura.Transversal.Apache.Cache
{
    public class MemoryCacheManager : ICacheManager
    {
        #region Constantes

        public const double CACHE_TIME_DEFAULT = 300000D;//5 minutos

        #endregion

        #region Campos

        private readonly IMemoryCache m_cache;
        private readonly ConcurrentDictionary<object, object> m_cacheEntries;

        #endregion

        #region Constructores

        public MemoryCacheManager(IMemoryCache cache)
        {
            if (cache == null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            m_cache = cache;
            m_cacheEntries = new ConcurrentDictionary<object, object>();
        }

        #endregion

        #region Metodos privados

        private void SetInternal<TItem>(object key, TItem value, MemoryCacheEntryOptions options)
        {
            options.RegisterPostEvictionCallback((k, v, r, s) =>
            {
                m_cacheEntries.TryRemove(k, out var _);
            });

            m_cache.Set(key, value, options);

            m_cacheEntries.AddOrUpdate(key, value, ((k, ov) =>
            {
                return value;
            }));
        }

        #endregion

        #region ICacheManager

        public TItem Get<TItem>(object key)
        {
            return m_cache.Get<TItem>(key);
        }

        public TItem Get<TItem>(object key, Func<TItem> factory)
        {
            return Get(key, CACHE_TIME_DEFAULT, factory);
        }

        public TItem Get<TItem>(object key, double cacheTime, Func<TItem> factory)
        {
            TItem aux;

            if (m_cache.TryGetValue(key, out aux))
            {
                return aux;
            }

            var result = factory();

            Set(key, result, cacheTime);

            return result;
        }

        public void Set<TItem>(object key, TItem value, double cacheTime)
        {
            var options = new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddMilliseconds(cacheTime)
            };

            SetInternal(key, value, options);
        }

        public void Set<TItem>(object key, TItem value, double cacheTime, TimeSpan slidingTime)
        {
            var options = new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddMilliseconds(cacheTime),
                SlidingExpiration = slidingTime
            };

            SetInternal(key, value, options);
        }

        public void Remove(object key)
        {
            m_cache.Remove(key);
        }

        public void Clear()
        {
            foreach (var key in m_cacheEntries.Keys.ToList())
            {
                Remove(key);
            }
        }

        #endregion
    }
}
