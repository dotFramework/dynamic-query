using DotFramework.DynamicQuery.Metadata;
using System.Runtime.Serialization;

namespace DotFramework.DynamicQuery
{
    [DataContract]
    public abstract class AbstractQuery
    {
        #region Properties

        [DataMember]
        public GeneralObject ObjectType { get; set; }

        private AbstractFilter _Filter;
        [DataMember]
        public AbstractFilter Filter
        {
            get
            {
                return _Filter;
            }
            set
            {
                _Filter = value;
            }
        }

        #endregion

        #region Abstract Methods

        

        #endregion

        #region Public Methods

        

        #endregion

        #region Virtual Methods

        public virtual bool ValidateQuery()
        {
            return true;
        }

        #endregion

        #region Properties



        #endregion
    }
}
