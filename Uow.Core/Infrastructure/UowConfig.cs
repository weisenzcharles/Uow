using System.Configuration;
using System.Xml;

namespace Uow.Core.Infrastructure
{
    /// <summary>
    ///     Represents a UowConfig.
    /// </summary>
    public class UowConfig : IConfigurationSectionHandler
    {
        /// <summary>
        ///     Indicates whether we should use Redis server for caching (instead of default in-memory caching)
        /// </summary>
        public bool RedisCachingEnabled { get; private set; }

        /// <summary>
        ///     Redis connection string. Used when Redis caching is enabled
        /// </summary>
        public string RedisCachingConnectionString { get; private set; }

        /// <summary>
        ///     创建一个配置节处理程序。
        /// </summary>
        /// <param name="parent">父对象。</param>
        /// <param name="configContext">配置上下文对象。</param>
        /// <param name="section">部分的 XML 节点。</param>
        /// <returns>创建的节处理程序对象。</returns>
        public object Create(object parent, object configContext, XmlNode section)
        {
            var config = new UowConfig();

            return config;
        }
    }
}