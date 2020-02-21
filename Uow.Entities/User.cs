using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uow.Core.Domain.Entities;

namespace Uow.Entities
{
    public class User: EntityBase
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
