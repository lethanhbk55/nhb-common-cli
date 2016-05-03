using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhbCommon.Data
{
    public interface PuObjectRW : PuObjectRO
    {
        object Remove(String fieldName);

        void RemoveAll();

        void AddAll(PuObjectRO source);

        void DecodeBase64(String fieldName);

        void SetType(string fieldName, PuDataType type);

        void Set(string fieldName, Object value);

        void SetRaw(string fieldName, byte[] value);

        void SetBoolean(string fieldName, Boolean value);

        void SetShort(string fieldName, short value);

        void SetInteger(string fieldName, int value);

        void SetLong(string fieldName, long value);

        void SetFloat(string fieldName, float value);

        void SetDouble(string fieldName, double value);

        void SetString(string fieldName, string value);

        void SetPuObject(string fieldName, PuObject value);

        void SetPuArray(string fieldName, PuArray value);
    }
}
