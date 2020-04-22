using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Uow.Core.Domain.Collections;
using Uow.Core.Domain.DataContext;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Uow.Core.Domain.UnitOfWork
{
    public interface IUnitOfWorkAsync : IUnitOfWork 
    {

    }
}
