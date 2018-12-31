using System.Collections;

namespace System.Linq
{
    public static class LinqExtensions
    {
        public static bool HasItem(this IEnumerable list)
        {
            var found = false;

            if (list != null)
            {
                foreach (var item in list)
                {
                    found = true;
                }
            }

            return found;
        }
    }
}
