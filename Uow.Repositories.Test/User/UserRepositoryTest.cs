using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Open.Data.DataContext;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uow.Core.Domain.Repositories;
using Uow.Domain.User;
using Uow.Repositories.Implementations.User;
using Uow.Repositories.Interface.User;
using Xunit;

namespace Uow.Repositories.Test.User
{
    public class UserRepositoryTest
    {
        private static readonly ServiceProvider _serviceProvider;
        static UserRepositoryTest()
        {
            var services = new ServiceCollection();
            services.AddDbContext<UowDataContext>(
             opt => opt.UseMySql("SERVER=192.168.0.199;UserID=root;Password=123456;Database=uow;Port=3306;SslMode=none;AllowUserVariables=True;Pooling=True;DefaultCommandTimeout=60;ConnectionTimeout=60;")
            );
            services.AddUnitOfWork<UowDataContext>();
            services.AddCustomRepository<UserDomain, UserRepository>();
            services.AddCustomRepositoryAsync<UserDomain, UserRepository>();

            //services.AddScoped<IDbContext, UowDataContext>();
            services.AddScoped<IUserRepository, UserRepository>();

            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public void RepositoryLongCountTest()
        {
            var userRepository = _serviceProvider.GetService<IUserRepository>();

            var user = userRepository.LongCount(c => c.CreatedUser == 164);

            Assert.Equal(8, user);
        }

        [Fact]
        public async Task RepositoryQueryAsyncTest()
        {
            var userRepository = _serviceProvider.GetService<IUserRepository>();

            var userIds = "1,3,5";

            var user = await userRepository.QueryAsync(userIds);

            Assert.Equal(3, user.Count);

            Assert.True(user.Any());
        }

        [Fact]
        public async Task RepositoryGetByIdAsyncTest()
        {
            var repositoryAsync = _serviceProvider.GetService<IRepositoryAsync<UserDomain>>();

            var c = await repositoryAsync.GetByIdAsync(10);

            Assert.Equal(10, c.Id);
        }

        [Fact]
        public void RepositoryFindTest()
        {
            var repository = _serviceProvider.GetService<IRepository<UserDomain>>();
            var user = repository.Find(100);

            Assert.Equal(100, user.Id);
        }

    }
}
