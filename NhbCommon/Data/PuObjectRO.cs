using System;
using System.Collections.Generic;

namespace NhbCommon.Data
{
    public interface PuObjectRO : PuElement
    {
        int Size();

        Dictionary<String, Object> ToMap();

        Object Get(String fieldName);

        bool GetBoolean(String fieldName);

        bool GetBoolean(String fieldName, bool defaultValue); 

        double GetDouble(String fieldName);

        double GetDouble(String fieldName, double defaultValue);

        float GetFloat(String fieldName);

        float GetFloat(String fieldName, float defaultValue);

        int GetInteger(String fieldName);

        int GetInteger(String fieldName, int defaultValue);

        long GetLong(String fieldName);

        long GetLong(String fieldName, long defaultValue);

        byte[] GetRaw(String fieldName);

        byte[] GetRaw(String fieldName, byte[] defaultValue);

        byte GetByte(String fieldName);

        byte GetByte(String fieldName, byte defaultValue);

        short GetShort(String fieldName);

        short GetShort(String fieldName, short defaultValue);

        string GetString(String fieldName);

        string GetString(String fieldName, string defaultValue);

        PuObject GetPuObject(String fieldName);

        PuObject GetPuObject(String fieldName, PuObject defaultValue);

        PuArray GetPuArray(String fieldName);

        PuArray GetPuArray(String fieldName, PuArray defaultValue);

        Dictionary<string, PuValue>.Enumerator GetEnumerator();
    }
}
