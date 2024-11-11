using System;
using System.Collections.Generic;
using Sipecom.FactElec.Pymes.Entidades.Base;

namespace Sipecom.FactElec.Pymes.AccesoDatos.Base
{
    public interface IRepository<TEntity, TPrimaryKey> : IDisposable where TEntity : Entity<TPrimaryKey>
    {
        TEntity Get(TPrimaryKey id);

        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> GetPaged(int pageIndex, int pageCount, bool ascending);

        TEntity Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);
    }
}
