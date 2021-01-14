﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Uow.Core.Domain.DapperAdapter
{
    public class MySqlAdapter : ISqlAdapter
    {
        public virtual string PagingBuild(ref PartedSql partedSql, object args, long skip, long take)
        {
            var pageSql = $"{partedSql.Raw} LIMIT {take} OFFSET {skip}";
            return pageSql;
        }

    }
}
