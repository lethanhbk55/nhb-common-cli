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

        void WriteJSON(JsonWriter writer);

        void ReadJSON(JsonReader reader);
        
        void WriteTo(Stream writer);
    }
}
