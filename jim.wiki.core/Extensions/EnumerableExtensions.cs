using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.core.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool ContainElements<T>(this IEnumerable<T> enumerable)
        {
            return enumerable != null && enumerable.Any();
        }

        public static bool NoContainElements<T>(this IEnumerable<T> enumerable)
        {
            return !enumerable.ContainElements();
        }
    }
}
