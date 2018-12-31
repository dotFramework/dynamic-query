namespace DotFramework.DynamicQuery.Metadata
{
    public class GeneralObject
    {
        public GeneralObject(string tableName)
            : this(null, tableName)
        {

        }

        public GeneralObject(string schemaName, string tableName)
        {
            SchemaName = schemaName;
            TableName = tableName;
        }

        public string SchemaName { get; set; }

        public string TableName { get; set; }
    }
}
