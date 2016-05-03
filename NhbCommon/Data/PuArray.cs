using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhbCommon.Data
{
    public interface PuArray : PuElement, IList<PuValue>
    {
        List<Object> ToList();

        void AddFrom(params Object[] data);

        byte[] GetRaw(int index);

        bool GetBoolean(int index);

        byte GetByte(int index);

        short GetShort(int index);

        int GetInteger(int index);

        float GetFloat(int index);

        long GetLong(int index);

        double GetDouble(int index);

        string GetString(int index);

        PuObject GetPuObject(int index);

        PuArray GetPuArray(int index);

        PuValue Evict(int index);
    }
}
