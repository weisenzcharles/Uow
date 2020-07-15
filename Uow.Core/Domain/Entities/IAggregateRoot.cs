using Uow.Core.Domain.Entities;
using Uow.Core.Events.Bus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Uow.Core.Domain.Entities
{
    public interface IAggregateRoot : IAggregateRoot<int>, IEntity
    {

    }

    public interface IAggregateRoot<TPrimaryKey> : IEntity<TPrimaryKey>, IGeneratesDomainEvents
    {

    }

    public interface IGeneratesDomainEvents
    {
        ICollection<IEventData> DomainEvents { get; }
    }
}
