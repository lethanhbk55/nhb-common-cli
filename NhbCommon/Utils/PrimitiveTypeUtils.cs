using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhbCommon.Utils
{
    public class PrimitiveTypeUtils
    {
        public static bool IsPrimitiveTypeOrWrapperType(Type type)
        {
            return (type.IsPrimitive || type == typeof(String) ||
                type == typeof(Double) || type == typeof(Char) ||
                type == typeof(Int16) || type == typeof(Int32) ||
                type == typeof(Int64) || type == typeof(Boolean)) && !type.IsArray;
        }
    }
}
