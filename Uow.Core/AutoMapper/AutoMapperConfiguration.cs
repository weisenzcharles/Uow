using System;
using System.Collections.Generic;
using AutoMapper;

namespace Uow.Core.AutoMapper
{
    /// <summary>
    ///     AutoMapper configuration
    /// </summary>
    public static class AutoMapperConfiguration
    {
        /// <summary>
        ///     Mapper
        /// </summary>
        public static IMapper Mapper { get; private set; }

        /// <summary>
        ///     Mapper configuration
        /// </summary>
        public static MapperConfiguration MapperConfiguration { get; private set; }

        /// <summary>
        ///     Initialize mapper
        /// </summary>
        /// <param name="configurationActions">Configuration actions</param>
        public static void Init(List<Action<IMapperConfigurationExpression>> configurationActions)
        {
            if (configurationActions == null)
                throw new ArgumentNullException("configurationActions");

            MapperConfiguration = new MapperConfiguration(cfg =>
            {
                foreach (var ca in configurationActions)
                    ca(cfg);
            });

            Mapper = MapperConfiguration.CreateMapper();
        }
    }
}