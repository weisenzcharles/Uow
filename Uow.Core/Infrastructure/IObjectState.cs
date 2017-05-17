using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uow.Core.Infrastructure
{
    /// <summary>
    /// 描述实体对象状态的接口。
    /// </summary>
    public interface IObjectState
    {
        /// <summary>
        /// 实体对象的状态。
        /// </summary>
        [NotMapped]
        EntityState ObjectState { get; set; }
    }
}
