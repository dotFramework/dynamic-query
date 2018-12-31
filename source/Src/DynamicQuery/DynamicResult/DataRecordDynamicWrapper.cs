using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Dynamic;

namespace DotFramework.DynamicQuery
{
    public class DataRecordDynamicWrapper : DynamicObject, ICustomTypeDescriptor
    {
        private readonly IDataRecord _dataRecord;
        private PropertyDescriptorCollection _properties;

        private List<String> _Members;
        private List<String> Members
        {
            get
            {
                if (_Members == null)
                {
                    _Members = new List<String>();

                    if (_dataRecord != null && _dataRecord.FieldCount != 0)
                    {
                        for (var i = 0; i < _dataRecord.FieldCount; i++)
                        {
                            _Members.Add(_dataRecord.GetName(i));
                        }
                    }
                }

                return _Members;
            }
        }

        public DataRecordDynamicWrapper(IDataRecord dataRecord)
        {
            _dataRecord = dataRecord;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = _dataRecord[binder.Name];

            if (result is DBNull)
            {
                result = null;
            }

            return true;
        }

        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            return AttributeCollection.Empty;
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            return _dataRecord.GetType().Name;
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            return _dataRecord.GetType().Name;
        }

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            return new TypeConverter();
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            return null;
        }

        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            return null;
        }

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            throw new NotSupportedException();
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return EventDescriptorCollection.Empty;
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            return EventDescriptorCollection.Empty;
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            if (_properties == null)
                _properties = GenerateProperties();
            return _properties;
        }

        private PropertyDescriptorCollection GenerateProperties()
        {
            var count = _dataRecord.FieldCount;
            var properties = new PropertyDescriptor[count];

            for (var i = 0; i < count; i++)
            {
                properties[i] = new DataRecordProperty(i, _dataRecord.GetName(i), _dataRecord.GetFieldType(i));
            }

            return new PropertyDescriptorCollection(properties);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            if (attributes != null && attributes.Length == 0)
                return ((ICustomTypeDescriptor)this).GetProperties();
            return PropertyDescriptorCollection.Empty;
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            return _dataRecord;
        }

        public override IEnumerable<String> GetDynamicMemberNames()
        {
            if (Members.Count != 0)
            {
                return Members;
            }
            else
            {
                return base.GetDynamicMemberNames();
            }
        }

        private sealed class DataRecordProperty : PropertyDescriptor
        {
            private static readonly Attribute[] NoAttributes = new Attribute[0];

            private readonly int _ordinal;
            private readonly Type _type;

            public DataRecordProperty(int ordinal, string name, Type type)
                : base(name, NoAttributes)
            {
                _ordinal = ordinal;
                _type = type;
            }

            public override bool CanResetValue(object component)
            {
                return false;
            }

            public override object GetValue(object component)
            {
                var wrapper = ((DataRecordDynamicWrapper)component);
                return wrapper._dataRecord.GetValue(_ordinal);
            }

            public override void ResetValue(object component)
            {
                throw new NotSupportedException();
            }

            public override void SetValue(object component, object value)
            {
                throw new NotSupportedException();
            }

            public override bool ShouldSerializeValue(object component)
            {
                return true;
            }

            public override Type ComponentType
            {
                get { return typeof(IDataRecord); }
            }

            public override bool IsReadOnly
            {
                get { return true; }
            }

            public override Type PropertyType
            {
                get { return _type; }
            }
        }
    }
}
