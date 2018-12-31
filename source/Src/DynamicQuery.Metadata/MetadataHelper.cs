using System;
using System.Linq;

namespace DotFramework.DynamicQuery.Metadata
{
    public static class MetadataHelper
    {
        public static bool HasSchemaName(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            var attributes = type.GetCustomAttributes(typeof(SchemaNameAttribute), false);
            return attributes != null && attributes.Length != 0;
        }

        //public static GeneralObject GetGeneralObject(object obj)
        //{
        //    if (obj is Type)
        //    {
        //        return GetGeneralObjectByType(obj as Type);
        //    }
        //    else if (obj is GeneralObject)
        //    {
        //        return obj as GeneralObject;
        //    }
        //    else
        //    {
        //        return GetGeneralObjectByType(obj.GetType());
        //    }
        //}

        public static GeneralObject GetGeneralObject(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            var attributes = type.GetCustomAttributes(typeof(SchemaNameAttribute), false);

            if (attributes != null && attributes.Length != 0)
            {
                return new GeneralObject((attributes.First() as SchemaNameAttribute).Schema, type.Name);
            }
            else
            {
                return new GeneralObject(type.Name);
            }
        }
    }
}
