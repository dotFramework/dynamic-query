using System.Runtime.Serialization;

namespace DotFramework.DynamicQuery
{
    [DataContract]
    public class AndFilter : AbstractFilter
    {
        public AndFilter()
        {

        }

        public AndFilter(AbstractFilter filterExpressionLeft, AbstractFilter filterExpressionRight)
        {
            FilterList.Add(filterExpressionLeft);
            FilterList.Add(filterExpressionRight);
        }

        public AndFilter(FilterList<AbstractFilter> filterList)
        {
            FilterList = filterList;
        }

        private FilterList<AbstractFilter> _FilterList;
        [DataMember]
        public FilterList<AbstractFilter> FilterList
        {
            get
            {
                if (_FilterList == null)
                    _FilterList = new FilterList<AbstractFilter>();
                return _FilterList;
            }
            set
            {
                _FilterList = value;
            }
        }
    }
}
