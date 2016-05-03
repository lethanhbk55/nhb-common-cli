using MsgPack.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhbCommon.Data.Template
{
    class PuGenericTypeTemplate : GenericTypeTemplate
    {
        protected override object ReadList(MsgPack.Unpacker unpacker)
        {
            PuArrayList list = new PuArrayList();
            long length = unpacker.ItemsCount;
            for (int i = 0; i < length; i++)
            {
                unpacker.Read();
                list.AddFrom(UnpackFromCore(unpacker));
            }
            return list;
        }

        protected override object ReadMap(MsgPack.Unpacker unpacker)
        {
            PuObject obj = new PuObject();
            long size = unpacker.ItemsCount;
            for (int i = 0; i < size; i++)
            {
                string key = "";
                unpacker.ReadString(out key);
                unpacker.Read();
                object value = UnpackFromCore(unpacker);
                obj.Set(key, value);
            }
            return obj;
        }
    }
}
