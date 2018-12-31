using System;

namespace DotFramework.DynamicQuery
{

    public static class DBValueExtensions
    {
        public static object GetDbValue(this object obj)
        {
            object retlVal = DBNull.Value;

            if (obj is DateTime)
            {
                if ((DateTime)obj != new DateTime())
                {
                    retlVal = obj;
                }
            }
            else if (obj is Guid)
            {
                if ((Guid)obj != new Guid())
                {
                    retlVal = obj;
                }
            }
            else if (obj is AbstractFilter)
            {
                var temp = obj as AbstractFilter;

                if (temp != null && !String.IsNullOrWhiteSpace(temp.ToString()))
                {
                    retlVal = temp.ToString();
                }
            }
            else if (obj is OrderByList)
            {
                var temp = obj as OrderByList;

                if (temp.Count != 0)
                {
                    retlVal = temp.ToString();
                }
            }
            else if (obj is SelectFieldList)
            {
                var temp = obj as SelectFieldList;

                if (temp.Count != 0)
                {
                    retlVal = temp.ToString();
                }
            }
            else
            {
                if (obj != null)
                {
                    retlVal = obj;
                }
            }

            return retlVal;
        }

        public static object GetDbValue(this object obj, bool isForeignKey)
        {
            object retlVal = DBNull.Value;

            if (isForeignKey)
            {
                if (obj is Guid)
                {
                    if ((Guid)obj != new Guid())
                    {
                        retlVal = obj;
                    }
                }
                else
                {
                    try
                    {
                        long value = Convert.ToInt64(obj);

                        if (value != 0)
                        {
                            retlVal = obj;
                        }
                    }
                    catch
                    {
                        throw new NotSupportedException(String.Format("{0} is not supported as a foreign key", obj.GetType().FullName));
                    }
                }
            }
            else
            {
                retlVal = obj.GetDbValue();
            }

            return retlVal;
        }
    }

}
