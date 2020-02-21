using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Uow.Core.Dependency;
using Uow.Core.Domain.DataContext;
using Uow.Core.Domain.Repositories;
using Uow.Core.Domain.Uow;
using Uow.Core.Fakes;
using Uow.Core.Infrastructure;
using Uow.Data.DataContext;
using Uow.Data.Repositories;
using Uow.Data.Uow;
using Uow.Entities;
using Uow.Repositories;
using Uow.Services;

namespace Uow.Web.Framework
{
    /// <summary>
    /// Dependency registrar
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {


        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            //    System.Diagnostics.Debugger.Break();

            //HTTP context and other related stuff
            builder.Register(c =>
                //register FakeHttpContext when HttpContext is not available
                HttpContext.Current != null ?
                (new HttpContextWrapper(HttpContext.Current) as HttpContextBase) :
                (new FakeHttpContext("~/") as HttpContextBase)).As<HttpContextBase>().InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Request).As<HttpRequestBase>().InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Response).As<HttpResponseBase>().InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Server).As<HttpServerUtilityBase>().InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Session).As<HttpSessionStateBase>().InstancePerLifetimeScope();

            //controllers
            builder.RegisterControllers(typeFinder.GetAssemblies().ToArray());

            // DbContext
            builder.RegisterType<UowDbContext>().As<IDataContextAsync>().InstancePerLifetimeScope();

            // UnitOfWork
            builder.RegisterType<UnitOfWork>().As<IUnitOfWorkAsync>().InstancePerLifetimeScope();

            // repository
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepositoryAsync<>)).InstancePerLifetimeScope();

            // repositories
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();

            // services
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
        }

        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        public int Order
        {
            get { return 0; }
        }
    }
}
