using MsgPack;
using MsgPack.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NhbCommon.Utils;
using Newtonsoft.Json;

namespace NhbCommon.Data
{
    public class PuValue : PuElement
    {
        private object _data = null;
        private PuDataType _type = PuDataType.NULL;

        public PuValue()
        {

        }

        public PuValue(object data)
        {
            Data = data;
        }

        public PuValue(object data, PuDataType type)
        {
            Data = data;
            Type = type;
        }

        public object Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }

        public PuDataType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                switch (_type)
                {
                    case PuDataType.BOOLEAN:
                        this._data = Convert.ToBoolean(this._data);
                        break;
                    case PuDataType.BYTE:
                        this._data = Convert.ToByte(this._data);
                        break;
                    case PuDataType.CHARACTER:
                        this._data = Convert.ToChar(this._data);
                        break;
                    case PuDataType.DOUBLE:
                        this._data = Convert.ToDouble(this._data);
                        break;
                    case PuDataType.FLOAT:
                        this._data = Convert.ToSingle(this._data);
                        break;
                    case PuDataType.LONG:
                        this._data = Convert.ToInt64(this._data);
                        break;
                    case PuDataType.NULL:
                        this._data = null;
                        break;
                    case PuDataType.PUARRAY:
                        this._data = PuArrayList.FromObject(this._data);
                        break;
                    case PuDataType.PUOBJECT:
                        this._data = PuObject.FromObject(this._data);
                        break;
                    case PuDataType.RAW:
                        if (this._data is string)
                        {
                            this._data = Encoding.UTF8.GetBytes((string)this._data);
                        }
                        break;
                    case PuDataType.SHORT:
                        this._data = Convert.ToInt16(this._data);
                        break;
                    case PuDataType.STRING:
                        this._data = Convert.ToString(this._data);
                        break;
                }
            }
        }

        public void DecodeBase64()
        {
            if (this.Data is string)
            {
                this.Data = Convert.FromBase64String((string)this.Data);
                this.Type = PuDataType.RAW;
            }
        }

        public byte[] ToBytes()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                WriteTo(stream);
                return stream.ToArray();
            }
        }

        public string ToJSON()
        {
            StringBuilder builder = new StringBuilder();
            using (StringWriter writer = new StringWriter())
            {
                using (JsonWriter jsonWriter = new JsonTextWriter(writer))
                {
                    if (this.Data is PuObject)
                    {
                        jsonWriter.WriteStartObject();
                        WriteJSON(jsonWriter);
                        jsonWriter.WriteEndObject();
                    }
                    else if (this.Data is PuArray)
                    {
                        jsonWriter.WriteStartArray();
                        WriteJSON(jsonWriter);
                        jsonWriter.WriteEndArray();
                    }
                    else
                    {
                        WriteJSON(jsonWriter);
                    }
                }
            }
            return builder.ToString();
        }

        public void WriteTo(Stream writer)
        {
            var serializer = MessagePackSerializer.Get<Boolean>();
            serializer.Pack(writer, _data);
        }

        public bool GetBoolean()
        {
            return Convert.ToBoolean(this._data);
        }

        public byte GetByte()
        {
            return Convert.ToByte(this._data);
        }

        public int GetInteger()
        {
            return Convert.ToInt32(this._data);
        }

        public long GetLong()
        {
            return Convert.ToInt64(this._data);
        }

        public float GetFloat()
        {
            return Convert.ToSingle(this._data);
        }

        public double GetDouble()
        {
            return Convert.ToDouble(this._data);
        }

        public char GetCharacter()
        {
            return Convert.ToChar(this._data);
        }

        public byte[] GetRaw()
        {
            if (this._type == PuDataType.STRING)
            {
                this._type = PuDataType.RAW;
            }
            if (this._data is string)
            {
                this._data = Encoding.UTF8.GetBytes((string)this._data);
            }
            return (byte[])this._data;
        }

        public PuObject GetPuObject()
        {
            if (this._data is PuObject)
            {
                return this._data as PuObject;
            }
            return PuObject.FromObject(this._data);
        }

        public PuArray GetPuArray()
        {
            if (this._data is PuArray)
            {
                return this._data as PuArray;
            }
            return PuArrayList.FromObject(this._data);
        }

        public short GetShort()
        {
            return Convert.ToInt16(this._data);
        }

        public string GetString()
        {
            if (this._type == PuDataType.RAW)
            {
                this._type = PuDataType.STRING;
            }
            if (this._data is byte[])
            {
                return Encoding.UTF8.GetString(this._data as byte[]);
            }
            return Convert.ToString(this._data);
        }

        public static PuValue FromObject(object value)
        {
            if (value == null)
            {
                return new PuValue(null, PuDataType.NULL);
            }
            else if (value is byte[])
            {
                return new PuValue(value as byte[], PuDataType.RAW);
            }
            else if (value is PuValue)
            {
                return new PuValue((value as PuValue).Data, (value as PuValue).Type);
            }
            else if (value is PuObject || value is PuArray || PrimitiveTypeUtils.IsPrimitiveTypeOrWrapperType(value.GetType()))
            {
                return new PuValue(value);
            }
            else if (ArrayUtils.IsArrayOrCollection(value))
            {
                return new PuValue(PuArrayList.FromObject(value));
            }
            else if (value is MessagePackObject)
            {
                MessagePackObject obj = (MessagePackObject)value;
                if (obj.IsNil)
                {
                    return new PuValue(null, PuDataType.NULL);
                }
                else if (obj.IsRaw)
                {
                    return new PuValue(obj.AsBinary(), PuDataType.RAW);
                }
                else
                {
                    return new PuValue(obj.ToObject());
                }
            }
            else
            {
                return new PuValue(PuObject.FromObject(value));
            }
        }

        public override string ToString()
        {
            if (this._data == null)
            {
                return null;
            }
            if (this._data is byte[])
            {
                return Encoding.UTF8.GetString(this._data as byte[]);
            }
            return this._data.ToString();
        }


        public void WriteJSON(Newtonsoft.Json.JsonWriter writer)
        {
            if (this.Data == null)
            {
                writer.WriteNull();
            }
            else if (this.Data is PuElement)
            {
                (this.Data as PuElement).WriteJSON(writer);
            }
            else if (this.Data is byte[])
            {
                writer.WriteValue(Encoding.UTF8.GetString(this.Data as byte[]));
            }
            else
            {
                writer.WriteValue(this.Data);
            }
        }

        public void ReadJSON(JsonReader reader)
        {
            switch (reader.TokenType)
            {
                case JsonToken.StartArray:
                    PuArray array = new PuArrayList();
                    array.ReadJSON(reader);
                    this._data = array;
                    this._type = PuDataType.PUARRAY;
                    break;
                case JsonToken.StartObject:
                    PuObject puo = new PuObject();
                    puo.ReadJSON(reader);
                    this._data = puo;
                    this._type = PuDataType.PUOBJECT;
                    break;
                case JsonToken.PropertyName:
                    break;
                default:
                    this.Data = reader.Value;
                    break;
            }
        }
    }
}
