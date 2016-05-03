using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhbCommon.Data.Template
{
    public class PuObjectTemplate : PuTemplate<PuObject>
    {
        private static readonly PuObjectTemplate _instance = new PuObjectTemplate();

        public static PuObjectTemplate Instance
        {
            get { return _instance; }
        }

        protected override void PackToCore(MsgPack.Packer packer, PuObject puObject)
        {
            packer.PackMapHeader(puObject.Size());

            foreach (KeyValuePair<string, PuValue> entry in puObject)
            {
                packer.PackString(entry.Key);
                if (entry.Value.Data is PuObject)
                {
                    PackToCore(packer, (PuObject) entry.Value.Data);
                }
                else if (entry.Value.Data is PuArray)
                {
                    PuArrayTemplate.Instance.PackTo(packer, (PuArray)entry.Value.Data);
                }
                else
                {
                    GenericTypeTemplate.PackTo(packer, entry.Value.Data);
                }
            }
        }

        protected override PuObject UnpackFromCore(MsgPack.Unpacker unpacker)
        {
            PuObject puo = new PuObject();
            long size = unpacker.ItemsCount;
            for (int i = 0; i < size; i++)
            {
                string key;
                unpacker.ReadString(out key);
                unpacker.Read();
                puo.Set(key, this.GenericTypeTemplate.UnpackFrom(unpacker));
            }
            return puo;
        }
    }
}
