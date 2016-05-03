using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhbCommon.Data.Template
{
    public class PuArrayTemplate : PuTemplate<PuArray>
    {
        private static readonly PuArrayTemplate _instance = new PuArrayTemplate();

        public static PuArrayTemplate Instance
        {
            get { return _instance; }
        }

        protected override void PackToCore(MsgPack.Packer packer, PuArray puArray)
        {
            packer.PackArrayHeader(puArray.Count);

            foreach (PuValue value in puArray)
            {
                if (value.Data is PuObject)
                {
                    PuObjectTemplate.Instance.PackTo(packer, (PuObject)value.Data);
                }
                else if (value.Data is PuArray)
                {
                    PackToCore(packer, (PuArray)value.Data);
                }
                else
                {
                    GenericTypeTemplate.PackTo(packer, value.Data);
                }
            }
        }

        protected override PuArray UnpackFromCore(MsgPack.Unpacker unpacker)
        {
            PuArray array = new PuArrayList();
            long size = unpacker.ItemsCount;
            for (int i = 0; i < size; i++)
            {
                unpacker.Read();
                array.Add(new PuValue(GenericTypeTemplate.UnpackFrom(unpacker)));
            }
            return array;
        }
    }
}
