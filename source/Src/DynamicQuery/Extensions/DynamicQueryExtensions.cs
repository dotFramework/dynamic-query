using System.Collections.Generic;

namespace DotFramework.DynamicQuery
{
    public static class DynamicQueryExtensions
    {
        public static bool In<T>(this T obj, IList<T> list)
        {
            return true;
        }
    }
}
