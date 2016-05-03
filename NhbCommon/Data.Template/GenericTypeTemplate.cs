using System;
using System.Collections.Generic;
using System.Text;
using MsgPack.Serialization;
using MsgPack;
using NhbCommon.Utils;
using System.Collections;

namespace NhbCommon.Data.Template
{
    public class GenericTypeTemplate : MessagePackSerializer<object>
    {
        public GenericTypeTemplate()
            : base(SerializationContext.Default)
        {

        }

        protected override void PackToCore(Packer packer, object obj)
        {
            if (obj == null)
            {
                packer.PackNull();
            }
            else if (obj is byte[])
            {
                packer.PackRaw((byte[])obj);
            }
            else if (PrimitiveTypeUtils.IsPrimitiveTypeOrWrapperType(obj.GetType()))
            {
                packer.Pack(obj);
            }
            else if (ArrayUtils.IsArrayOrCollection(obj))
            {
                WriteList(packer, obj);
            }
            else if (obj is IDictionary)
            {
                IDictionary<string, object> map = (IDictionary<string, object>)obj;
                packer.PackMapHeader(map.Count);
                foreach (KeyValuePair<string, object> entry in map)
                {
                    packer.PackString(entry.Key);
                    PackToCore(packer, entry.Value);
                }
            }
        }

        protected void WriteList(Packer packer, object arrayOrCollection)
        {
            packer.PackArrayHeader(ArrayUtils.Length(arrayOrCollection));
            ArrayUtils.ForEach(arrayOrCollection, (obj) =>
            {
                PackToCore(packer, obj);
            });
        }

        protected override object UnpackFromCore(Unpacker unpacker)
        {
            if (unpacker.IsArrayHeader)
            {
                return ReadList(unpacker);
            }
            else if (unpacker.IsMapHeader)
            {
                return ReadMap(unpacker);
            }
            else
            {
                if (unpacker.LastReadData.IsNil)
                {
                    return null;
                }
                else if (unpacker.LastReadData.IsRaw)
                {
                    return unpacker.LastReadData.AsBinary();
                }
                return unpacker.LastReadData.ToObject();
            }
        }

        protected virtual object ReadList(Unpacker unpacker)
        {
            long length = unpacker.ItemsCount;
            IList<object> list = new List<object>();
            for (int i = 0; i < length; i++)
            {
                unpacker.Read();
                list.Add(UnpackFromCore(unpacker));
            }
            return list;
        }

        protected virtual object ReadMap(Unpacker unpacker)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            long size = unpacker.ItemsCount;
            for (int i = 0; i < size; i++)
            {
                string key = "";
                unpacker.ReadString(out key);
                unpacker.Read();
                object value = UnpackFromCore(unpacker);
                map[key] = value;
            }
            return map;
        }
    }
}
