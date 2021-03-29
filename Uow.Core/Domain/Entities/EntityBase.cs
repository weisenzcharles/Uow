using System;
using System.Data.Entity;
using Uow.Core.Infrastructure;

namespace Uow.Core.Domain.Entities
{
    /// <summary>
    ///     实体对象基础类。
    /// </summary>
    public class EntityBase : IObjectState
    {
        #region Fields...

        #endregion

        #region Properties...

        /// <summary>
        ///     获取或设置实体对象唯一标识符。
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     获取或设置实体对象的状态。
        /// </summary>
        //[NotMapped]
        public EntityState ObjectState { get; set; }

        #endregion

        #region Methods...

        #region Public Method...

        /// <summary>
        ///     确定指定的实体对象是否相同。
        /// </summary>
        /// <param name="obj">指定的对象实例。</param>
        /// <returns>如果是相同的实例，或者如果二者都为 null，则为 true；否则为 false。</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as EntityBase);
        }

        /// <summary>
        ///     获取当前实体对象的类型。
        /// </summary>
        /// <returns>当前实体对象的类型。</returns>
        private Type GetUnproxiedType()
        {
            return GetType();
        }

        /// <summary>
        ///     确定指定的对象实例是否是相同的实例。
        /// </summary>
        /// <param name="other">指定的对象实例。</param>
        /// <returns>如果是相同的实例，或者如果二者都为 null，则为 true；否则为 false。</returns>
        public virtual bool Equals(EntityBase other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (!IsTransient(this) &&
                !IsTransient(other) &&
                Equals(Id, other.Id))
            {
                var otherType = other.GetUnproxiedType();
                var thisType = GetUnproxiedType();
                return thisType.IsAssignableFrom(otherType) ||
                       otherType.IsAssignableFrom(thisType);
            }

            return false;
        }

        /// <summary>
        ///     获取对象的哈希编码。
        /// </summary>
        /// <returns>哈希编码。</returns>
        public override int GetHashCode()
        {
            if (Equals(Id, default(int)))
                return base.GetHashCode();
            return Id.GetHashCode();
        }

        /// <summary>
        ///     确定两个对象实例是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个对象实例。</param>
        /// <param name="y">要比较的第二个对象实例。</param>
        /// <returns>如果对象相等，则为 true；否则为 false。</returns>
        public static bool operator ==(EntityBase x, EntityBase y)
        {
            return Equals(x, y);
        }

        /// <summary>
        ///     确定两个对象实例是否不相等。
        /// </summary>
        /// <param name="x">要比较的第一个对象实例。</param>
        /// <param name="y">要比较的第二个对象实例。</param>
        /// <returns>如果对象不相等，则为 true；否则为 false。</returns>
        public static bool operator !=(EntityBase x, EntityBase y)
        {
            return !(x == y);
        }

        #endregion

        #region Private Method...

        /// <summary>
        ///     判断指定的对象是否是瞬时实例。
        /// </summary>
        /// <param name="obj">指定的对象。</param>
        /// <returns>如果是，则为 true；否则为 false。</returns>
        private static bool IsTransient(EntityBase obj)
        {
            return obj != null && Equals(obj.Id, default(int));
        }

        #endregion

        #endregion
    }
}