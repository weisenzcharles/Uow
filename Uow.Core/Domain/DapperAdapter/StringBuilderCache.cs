using System;
using System.Collections.Generic;
using System.Text;

namespace Uow.Core.Domain.DapperAdapter
{
    public static class StringBuilderCache
    {
        [ThreadStatic]
        private static StringBuilder _cache;

        public static StringBuilder Allocate()
        {
            var stringBuilder = _cache;
            if (stringBuilder == null)
                return new StringBuilder();

            stringBuilder.Length = 0;
            _cache = null;
            return stringBuilder;
        }

        public static void Free(StringBuilder stringBuilder)
        {
            _cache = stringBuilder;
        }

        public static string ReturnAndFree(StringBuilder stringBuilder)
        {
            var str = stringBuilder.ToString();
            _cache = stringBuilder;
            return str;
        }
    }
}
