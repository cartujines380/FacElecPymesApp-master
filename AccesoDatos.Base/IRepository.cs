using System;
using Sipecom.FactElec.Pymes.Entidades.Base;

namespace Sipecom.FactElec.Pymes.AccesoDatos.Base
{
    public interface IRepository<TEntity> : IRepository<TEntity, int> where TEntity : Entity<int>
    {

    }
}
