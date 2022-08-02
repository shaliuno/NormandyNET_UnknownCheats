using System;
using System.Collections.Generic;
using System.Linq;

namespace NormandyNET.Helpers
{
    internal static class MathHelper
    {
        public static T Median<T>(this IEnumerable<T> items)
        {
            var i = (int)Math.Ceiling((double)(items.Count() - 1) / 2);
            if (i >= 0)
            {
                var values = items.ToList();
                values.Sort();
                return values[i];
            }

            return default(T);
        }
    }
}