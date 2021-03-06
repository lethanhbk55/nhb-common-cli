﻿using Newtonsoft.Json;
using NhbCommon.Data.Template;
using NhbCommon.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhbCommon.Data
{
    public class PuArrayList : List<PuValue>, PuArray
    {
        public static PuArray FromObject(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            if (obj is byte[])
            {
                byte[] buffer = obj as byte[];
                using (MemoryStream stream = new MemoryStream(buffer))
                {
                    return PuArrayTemplate.Instance.Unpack(stream);
                }
            }
            else if (obj is PuArray)
            {
                PuArray result = new PuArrayList();
                PuArray array = obj as PuArray;
                foreach (PuValue value in array)
                {
                    result.Add(value);
                }
                return result;
            }
            else if (ArrayUtils.IsArrayOrCollection(obj))
            {
                PuArray result = new PuArrayList();
                ArrayUtils.ForEach(obj, (x) =>
                {
                    result.Add(new PuValue(x));
                });
                return result;
            }
            throw new InvalidOperationException();
        }

        public List<object> ToList()
        {
            List<object> result = new List<object>();
            for (int i = 0; i < this.Count; i++)
            {
                result.Add(this[i].Data);
            }
            return result;
        }

        public void AddFrom(params object[] data)
        {
            if (data == null)
            {
                this.Add(PuValue.FromObject(null));
            }
            else
            {
                foreach (object element in data)
                {
                    this.Add(PuValue.FromObject(element));
                }
            }
        }

        public byte[] GetRaw(int index)
        {
            return this[index].GetRaw();
        }

        public bool GetBoolean(int index)
        {
            return this[index].GetBoolean();
        }

        public byte GetByte(int index)
        {
            return this[index].GetByte();
        }

        public short GetShort(int index)
        {
            return this[index].GetShort();
        }

        public int GetInteger(int index)
        {
            return this[index].GetInteger();
        }

        public float GetFloat(int index)
        {
            return this[index].GetFloat();
        }

        public long GetLong(int index)
        {
            return this[index].GetLong();
        }

        public double GetDouble(int index)
        {
            return this[index].GetDouble();
        }

        public string GetString(int index)
        {
            return this[index].GetString();
        }

        public PuObject GetPuObject(int index)
        {
            return this[index].GetPuObject();
        }

        public PuArray GetPuArray(int index)
        {
            return this[index].GetPuArray();
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
            using (StringWriter stringWriter = new StringWriter(builder))
            {
                using (JsonWriter jsonWriter = new JsonTextWriter(stringWriter))
                {
                    jsonWriter.WriteStartArray();
                    WriteJSON(jsonWriter);
                    jsonWriter.WriteEndArray();
                }
            }
            return builder.ToString();
        }

        public void WriteTo(System.IO.Stream writer)
        {
            PuArrayTemplate.Instance.Pack(writer, this);
        }

        public PuValue Evict(int index)
        {
            PuValue objectToRemove = this[index];
            RemoveAt(index);
            return objectToRemove;
        }


        public void WriteJSON(Newtonsoft.Json.JsonWriter writer)
        {
            foreach (PuValue value in this)
            {
                if (value.Data is PuObject)
                {
                    writer.WriteStartObject();
                    value.WriteJSON(writer);
                    writer.WriteEndObject();
                }
                else if (value.Data is PuArray)
                {
                    writer.WriteStartArray();
                    value.WriteJSON(writer);
                    writer.WriteEndArray();
                }
                else
                {
                    value.WriteJSON(writer);
                }
            }
        }


        public void ReadJSON(JsonReader reader)
        {
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.StartArray:
                        PuArray array = new PuArrayList();
                        array.ReadJSON(reader);
                        this.Add(PuValue.FromObject(array));
                        break;
                    case JsonToken.StartObject:
                        PuObject puo = new PuObject();
                        puo.ReadJSON(reader);
                        this.Add(PuValue.FromObject(puo));
                        break;
                    case JsonToken.PropertyName:
                    case JsonToken.EndArray:
                    case JsonToken.EndObject:
                    case JsonToken.EndConstructor:
                    case JsonToken.StartConstructor:
                        break;
                    default:
                        PuValue value = new PuValue();
                        value.ReadJSON(reader);
                        this.Add(value);
                        break;
                }
            }
        }

        public static PuArray FromJson(String json)
        {
            using (StringReader stringReader = new StringReader(json))
            {
                using (JsonReader reader = new JsonTextReader(stringReader))
                {
                    if (reader.Read())
                    {
                        switch (reader.TokenType)
                        {
                            case JsonToken.StartArray:
                                PuArray array = new PuArrayList();
                                array.ReadJSON(reader);
                                return array;
                            default:
                                throw new InvalidOperationException("array not start with [ token");
                        }
                    }
                }
            }
            return null;
        }
    }
}
