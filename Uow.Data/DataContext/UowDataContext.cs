using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyModel;
using Uow.Core.Domain.DataContext;
using Uow.Core.Domain.Entities;
using Open.Data.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Open.Data.DataContext
{
    public class UowDataContext : UowDbContext, IDbContext
    {
        #region Fields...

        #endregion

        #region Constructors...

        /// <summary>
        /// Initializes a new instance of the <seealso cref="UowDataContext"/> class.
        /// </summary>
        protected UowDataContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <seealso cref="UowDataContext"/> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public UowDataContext(DbContextOptions options) : base(options)
        {
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Further configuration the model.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //dynamically load all entity and query type configurations
            var typeConfigurations = Assembly.GetExecutingAssembly().GetTypes().Where(type =>
                (type.BaseType?.IsGenericType ?? false)
                    && (type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>)));

            foreach (var typeConfiguration in typeConfigurations)
            {
                var configuration = (IMappingConfiguration)Activator.CreateInstance(typeConfiguration);
                configuration.ApplyConfiguration(modelBuilder);
            }

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Modify the input SQL query by adding passed parameters
        /// </summary>
        /// <param name="sql">The raw SQL query</param>
        /// <param name="parameters">The values to be assigned to parameters</param>
        /// <returns>Modified raw SQL query</returns>
        protected virtual string CreateSqlWithParameters(string sql, params object[] parameters)
        {
            //add parameters to sql
            for (var i = 0; i <= (parameters?.Length ?? 0) - 1; i++)
            {
                if (!(parameters[i] is DbParameter parameter))
                    continue;

                sql = $"{sql}{(i > 0 ? "," : string.Empty)} @{parameter.ParameterName}";

                //whether parameter is output
                if (parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Output)
                    sql = $"{sql} output";
            }

            return sql;
        }

        #endregion

    }
}
