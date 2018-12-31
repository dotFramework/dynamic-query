using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotFramework.DynamicQuery
{
    [DataContract]
    public class FilterList<TFilter> : List<TFilter>
        where TFilter : AbstractFilter
    {
        public new void Add(TFilter item)
        {
            if (!String.IsNullOrEmpty(item.ToString()))
            {
                base.Add(item);
            }
        }
    }
}
