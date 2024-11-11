using System;
using System.Collections.Generic;

namespace Sipecom.FactElec.Pymes.Entidades.Base
{
    public abstract class Entity<TPrimaryKey>
    {
        #region Campos

        private int? m_requestedHashCode;
        private TPrimaryKey m_Id;

        #endregion

        #region Propiedades

        public virtual TPrimaryKey Id
        {
            get
            {
                return m_Id;
            }
            protected set
            {
                m_Id = value;
            }
        }

        #endregion

        #region Metodos protegidos

        protected virtual bool IsTransient(TPrimaryKey identity)
        {
            if (typeof(TPrimaryKey) == typeof(int))
            {
                return Convert.ToInt32(identity) == 0;
            }

            if (typeof(TPrimaryKey) == typeof(long))
            {
                return Convert.ToInt64(identity) == 0L;
            }

            return EqualityComparer<TPrimaryKey>.Default.Equals(identity, default(TPrimaryKey));
        }

        #endregion

        #region Metodos publicos

        public bool IsTransient()
        {
            return IsTransient(m_Id);
        }

        public void ChangeCurrentIdentity(TPrimaryKey identity)
        {
            if (!IsTransient(identity))
            {
                m_Id = identity;
            }
        }

        #endregion

        #region Object overrides

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity<TPrimaryKey>))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            var item = (Entity<TPrimaryKey>)obj;

            if (item.IsTransient() || IsTransient())
            {
                return false;
            }
            else
            {
                return m_Id.Equals(item.Id);
            }
        }

        public override int GetHashCode()
        {
            if (IsTransient())
            {
                return base.GetHashCode();
            }
            else
            {
                if (!m_requestedHashCode.HasValue)
                {
                    m_requestedHashCode = this.Id.GetHashCode() ^ 31;
                }

                return m_requestedHashCode.Value;
            }
        }

        public static bool operator ==(Entity<TPrimaryKey> left, Entity<TPrimaryKey> right)
        {
            if (Equals(left, null))
            {
                return Equals(right, null);
            }
            else
            {
                return left.Equals(right);
            }
        }

        public static bool operator !=(Entity<TPrimaryKey> left, Entity<TPrimaryKey> right)
        {
            return !(left == right);
        }

        #endregion
    }
}
