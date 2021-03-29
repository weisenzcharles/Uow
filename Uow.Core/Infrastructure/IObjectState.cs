using System.Data.Entity;

namespace Uow.Core.Infrastructure
{
    /// <summary>
    ///     描述实体对象状态的接口。
    /// </summary>
    public interface IObjectState
    {
        /// <summary>
        ///     实体对象的状态。
        /// </summary>
        // [NotMapped]
        EntityState ObjectState { get; set; }
    }
}