using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;

namespace NhbCommon.Utils
{
    public class ArrayUtils
    {
        public static bool IsArrayOrCollection(object obj)
        {
            Type type = obj.GetType();
            if (type.IsArray)
            {
                return true;
            }


            if (obj is IList)
            {
                return true;
            }


            return false;
        }

        public static int Length(object obj)
        {
            if (obj != null)
            {
                if (obj.GetType().IsArray)
                {
                    Array array = obj as Array;
                    return array.Length;
                }

                if (obj is IList)
                {
                    IList<object> list = obj as IList<object>;
                    return list.Count;
                }
            }
            return 0;
        }

        public static void ForEach(object obj, Action<object> action)
        {
            if (obj != null)
            {
                if (obj.GetType().IsArray)
                {
                    Array array = obj as Array;
                    for (int i = 0; i < array.Length; i++)
                    {
                        action.Invoke(array.GetValue(i));
                    }
                }

                if (obj is IList)
                {
                    IList<object> list = obj as IList<object>;
                    foreach (var value in list)
                    {
                        action.Invoke(value);
                    }
                }
            }
        }
    }
}
