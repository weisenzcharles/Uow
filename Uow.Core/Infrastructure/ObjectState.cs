namespace Uow.Core.Infrastructure
{
    /// <summary>
    ///     定义一组描述实体对象状态的枚举。
    /// </summary>
    public enum ObjectState
    {
        /// <summary>
        ///     未更改。
        /// </summary>
        Unchanged,

        /// <summary>
        ///     添加。
        /// </summary>
        Added,

        /// <summary>
        ///     更新。
        /// </summary>
        Modified,

        /// <summary>
        ///     删除。
        /// </summary>
        Deleted
    }
}