using System;
using AutoMapper;

namespace Uow.Core.AutoMapper
{
    /// <summary>
    ///     Mapper configuration registrar interface
    /// </summary>
    public interface IMapperConfiguration
    {
        /// <summary>
        ///     Order of this mapper implementation
        /// </summary>
        int Order { get; }

        /// <summary>
        ///     Get configuration
        /// </summary>
        /// <returns>Mapper configuration action</returns>
        Action<IMapperConfigurationExpression> GetConfiguration();
    }
}