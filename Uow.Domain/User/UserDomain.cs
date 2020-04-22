using System;
using System.Collections.Generic;
using System.Text;
using Uow.Core.Domain.Entities;

namespace Uow.Domain.User
{
    public class UserDomain : Entity<int>, IEntity
    {

        //public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
