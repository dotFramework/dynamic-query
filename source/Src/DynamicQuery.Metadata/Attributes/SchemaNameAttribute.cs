using System;

namespace DotFramework.DynamicQuery.Metadata
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SchemaNameAttribute : Attribute
    {
        public SchemaNameAttribute(string schema)
        {
            Schema = schema;

        }

        public string Schema { get; private set; }
    }
}
