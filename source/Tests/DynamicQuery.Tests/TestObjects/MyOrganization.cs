using System;

namespace DotFramework.DynamicQuery.Tests
{
    public class Organization
    {
        public long iD { get; set; }
        public string ORG_NODE_CODE { get; set; }
        public string ORG_NODE_NAME { get; set; }
        public string ORG_PARENT_CODE { get; set; }
        public string ORG_TYPE_CODE { get; set; }
        public long COST_CENTER_ID { get; set; }
        public short ORGANIZATION_STATUS { get; set; }
        public long CALENDAR_ID { get; set; }
        public DateTime OPERATION_START_DATE { get; set; }
        public DateTime APPROVAL_DATE { get; set; }
        public int USED { get; set; }
        public DateTime LASTUPDATEDDATE { get; set; }
        public string LAST_UPDATED_BY { get; set; }
        public short? STATUS { get; set; }
        public string SUBSYSTEM_CODE { get; set; }
    }

    public class CostCenter
    {
        public long iD { get; set; }
        public string COST_CENTERE_CODE { get; set; }
        public string COST_CENTER_NAME { get; set; }
    }
}
