using NhbCommon.Data.Template;
using NhbCommon.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NhbCommon.Data
{
    public class PuObject : PuObjectRW
    {
        private Dictionary<string, PuValue> _values = new Dictionary<string, PuValue>();

        public static PuObject FromObject(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            if (obj is byte[])
            {
                using (MemoryStream stream = new MemoryStream(obj as byte[]))
                {
                    return PuObjectTemplate.Instance.Unpack(stream);
                }
            }
            else if (obj is PuObjectRO)
            {
                PuObject result = new PuObject();
                result.AddAll(obj as PuObjectRO);
                return result;
            }
            else if (!PrimitiveTypeUtils.IsPrimitiveTypeOrWrapperType(obj.GetType()) && !ArrayUtils.IsArrayOrCollection(obj.GetType()))
            {
                Dictionary<string, object> map = null;
                if (obj is Dictionary<string, object>)
                {
                    map = obj as Dictionary<string, object>;
                }
                if (map != null)
                {
                    PuObject result = new PuObject();
                    foreach (KeyValuePair<string, object> entry in map)
                    {
                        result.Set(entry.Key, entry.Value);
                    }

                    return result;
                }
            }

            throw new InvalidOperationException();
        }

        public object Remove(string fieldName)
        {
            object objectToRemove = null;
            if (this._values.ContainsKey(fieldName))
            {
                objectToRemove = _values[fieldName];
            }
            this._values.Remove(fieldName);
            return objectToRemove;
        }

        public void RemoveAll()
        {
            this._values.Clear();
        }

        public void AddAll(PuObjectRO source)
        {
            foreach (KeyValuePair<string, PuValue> entry in source)
            {
                this.Set(entry.Key, entry.Value);
            }
        }

        public void DecodeBase64(string fieldName)
        {
            if (this._values.ContainsKey(fieldName))
            {
                this._values[fieldName].DecodeBase64();
            }
        }

        public void SetType(string fieldName, PuDataType type)
        {
            if (this._values.ContainsKey(fieldName))
            {
                this._values[fieldName].Type = type;
            }
        }

        public void Set(string fieldName, object value)
        {
            this._values.Add(fieldName, PuValue.FromObject(value));
        }

        public void SetRaw(string fieldName, byte[] value)
        {
            this._values.Add(fieldName, new PuValue(value, PuDataType.RAW));
        }

        public void SetBoolean(string fieldName, bool value)
        {
            this._values.Add(fieldName, new PuValue(value, PuDataType.BOOLEAN));
        }

        public void SetShort(string fieldName, short value)
        {
            this._values.Add(fieldName, new PuValue(value, PuDataType.SHORT));
        }

        public void SetInteger(string fieldName, int value)
        {
            this._values.Add(fieldName, new PuValue(value, PuDataType.INTEGER));
        }

        public void SetLong(string fieldName, long value)
        {
            this._values.Add(fieldName, new PuValue(value, PuDataType.LONG));
        }

        public void SetFloat(string fieldName, float value)
        {
            this._values.Add(fieldName, new PuValue(value, PuDataType.FLOAT));
        }

        public void SetDouble(string fieldName, double value)
        {
            this._values.Add(fieldName, new PuValue(value, PuDataType.DOUBLE));
        }

        public void SetString(string fieldName, string value)
        {
            this._values.Add(fieldName, new PuValue(value, PuDataType.STRING));
        }

        public void SetPuObject(string fieldName, PuObject value)
        {
            this._values.Add(fieldName, new PuValue(value, PuDataType.PUOBJECT));
        }

        public void SetPuArray(string fieldName, PuArray value)
        {
            this._values.Add(fieldName, new PuValue(value, PuDataType.PUARRAY));
        }

        public int Size()
        {
            return _values.Count;
        }

        public Dictionary<string, object> ToMap()
        {
            Dictionary<string, object> map = new Dictionary<string, object>();
            foreach (KeyValuePair<string, PuValue> entry in this._values)
            {
                map.Add(entry.Key, entry.Value.Data);
            }
            return map;
        }

        public object Get(string fieldName)
        {
            return this._values[fieldName].Data;
        }

        public bool GetBoolean(string fieldName)
        {
            return this._values[fieldName].GetBoolean();
        }

        public bool GetBoolean(string fieldName, bool defaultValue)
        {
            if (this._values.ContainsKey(fieldName))
            {
                return GetBoolean(fieldName);
            }
            return defaultValue;
        }

        public double GetDouble(string fieldName)
        {
            return this._values[fieldName].GetDouble();
        }

        public double GetDouble(string fieldName, double defaultValue)
        {
            if (this._values.ContainsKey(fieldName))
            {
                return GetDouble(fieldName);
            }
            return defaultValue;
        }

        public float GetFloat(string fieldName)
        {
            return this._values[fieldName].GetFloat();
        }

        public float GetFloat(string fieldName, float defaultValue)
        {
            if (this._values.ContainsKey(fieldName))
            {
                return GetFloat(fieldName);
            }
            return defaultValue;
        }

        public int GetInteger(string fieldName)
        {
            return this._values[fieldName].GetInteger();
        }

        public int GetInteger(string fieldName, int defaultValue)
        {
            if (this._values.ContainsKey(fieldName))
            {
                return GetInteger(fieldName);
            }
            return defaultValue;
        }

        public long GetLong(string fieldName)
        {
            return this._values[fieldName].GetLong();
        }

        public long GetLong(string fieldName, long defaultValue)
        {
            if (this._values.ContainsKey(fieldName))
            {
                return GetLong(fieldName);
            }
            return defaultValue;
        }

        public byte[] GetRaw(string fieldName)
        {
            return this._values[fieldName].GetRaw();
        }

        public byte[] GetRaw(string fieldName, byte[] defaultValue)
        {
            if (this._values.ContainsKey(fieldName))
            {
                return GetRaw(fieldName);
            }
            return defaultValue;
        }

        public byte GetByte(string fieldName)
        {
            return _values[fieldName].GetByte();
        }

        public byte GetByte(string fieldName, byte defaultValue)
        {
            if (this._values.ContainsKey(fieldName))
            {
                return GetByte(fieldName);
            }
            return defaultValue;
        }

        public PuObject GetPuObject(string fieldName)
        {
            return _values[fieldName].GetPuObject();
        }

        public PuObject GetPuObject(string fieldName, PuObject defaultValue)
        {
            if (this._values.ContainsKey(fieldName))
            {
                return GetPuObject(fieldName);
            }
            return defaultValue;
        }

        public PuArray GetPuArray(string fieldName)
        {
            return this._values[fieldName].GetPuArray();
        }

        public PuArray GetPuArray(string fieldName, PuArray defaultValue)
        {
            if (this._values.ContainsKey(fieldName))
            {
                return GetPuArray(fieldName);
            }
            return defaultValue;
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
                    jsonWriter.WriteStartObject();
                    WriteJSON(jsonWriter);
                    jsonWriter.WriteEndObject();
                }
            }
            return builder.ToString();
        }

        public void WriteTo(System.IO.Stream writer)
        {
            PuObjectTemplate.Instance.Pack(writer, this);
        }

        public void WriteJSON(JsonWriter writer)
        {
            foreach (KeyValuePair<string, PuValue> entry in this)
            {
                writer.WritePropertyName(entry.Key);
                if (entry.Value.Data is PuObject)
                {
                    writer.WriteStartObject();
                    entry.Value.WriteJSON(writer);
                    writer.WriteEndObject();
                }
                else if (entry.Value.Data is PuArray)
                {
                    writer.WriteStartArray();
                    entry.Value.WriteJSON(writer);
                    writer.WriteEndArray();
                }
                else
                {
                    entry.Value.WriteJSON(writer);
                }
            }
        }

        public short GetShort(string fieldName)
        {
            return this._values[fieldName].GetShort();
        }

        public short GetShort(string fieldName, short defaultValue)
        {
            if (this._values.ContainsKey(fieldName))
            {
                return _values[fieldName].GetShort();
            }
            return defaultValue;
        }

        public Dictionary<string, PuValue>.Enumerator GetEnumerator()
        {
            return this._values.GetEnumerator();
        }


        public string GetString(string fieldName)
        {
            return this._values[fieldName].GetString();
        }

        public string GetString(string fieldName, string defaultValue)
        {
            if (this._values.ContainsKey(fieldName))
            {
                return GetString(fieldName);
            }
            return defaultValue;
        }

        public static PuObject FromJson(string json)
        {
            using (StringReader stringReader = new StringReader(json))
            {
                using (JsonReader reader = new JsonTextReader(stringReader))
                {
                    if (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.StartObject)
                        {
                            PuObject result = new PuObject();
                            result.ReadJSON(reader);
                            return result;
                        }
                        else
                        {
                            throw new InvalidOperationException("json object not start { token");
                        }
                    }
                }
            }
            return null;
        }

        public void ReadJSON(JsonReader reader)
        {
            while (reader.Read())
            {
                JsonToken token = reader.TokenType;
                switch (token)
                {
                    case JsonToken.StartObject:
                        {
                            PuObject puo = new PuObject();
                            reader.Read();
                            String key = reader.Value.ToString();
                            puo.ReadJSON(reader);
                            this.SetPuObject(key, puo);
                            break;
                        }
                    case JsonToken.StartArray:
                        PuArray array = new PuArrayList();
                        array.ReadJSON(reader);
                        break;
                    case JsonToken.PropertyName:
                        {
                            string key = reader.Value.ToString();
                            if (reader.Read())
                            {
                                object value = reader.Value;
                                this.Set(key, value);
                            }
                            break;
                        }
                }
            }
        }
    }
}
