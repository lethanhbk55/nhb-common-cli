using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhbCommon.Data
{
    public interface PuElement
    {
        byte[] ToBytes();

        String ToJSON();

        void WriterJSON(JsonWriter writer);
        
        void WriteTo(Stream writer);
    }
}
