using System;
using System.Runtime.Serialization;

namespace DotFramework.DynamicQuery
{
    [DataContract]
    public class UpdateQuery : AbstractQuery
    {
        #region Constructor

        public UpdateQuery()
        {
            UpdateFieldList = new UpdateFieldList();
        }

        #endregion

        #region Properties

        [DataMember]
        public UpdateFieldList UpdateFieldList { get; set; }

        #endregion

        #region Overrided Methods

        public override bool ValidateQuery()
        {
            if (base.ValidateQuery())
            {
                if (UpdateFieldList.Count == 0)
                {
                    throw new Exception("Update Fields cannot be empty.");
                }
            }

            return true;
        }

        #endregion
    }
}
