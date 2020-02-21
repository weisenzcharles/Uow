using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uow.Core.Domain.Entities;

namespace Uow.Domain
{
    public class UserDomain: EntityBase
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
