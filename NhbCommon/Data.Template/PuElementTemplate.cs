using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhbCommon.Data.Template
{
    public class PuElementTemplate : PuTemplate<PuElement>
    {
        protected override void PackToCore(MsgPack.Packer packer, PuElement puElement)
        {
            if (puElement is PuArray)
            {
                PuArrayTemplate.Instance.PackTo(packer, puElement as PuArray);
            }
            else if (puElement is PuObject)
            {
                PuObjectTemplate.Instance.PackTo(packer, puElement as PuObject);
            }
            else if (puElement is PuValue)
            {
                this.GenericTypeTemplate.PackTo(packer, ((PuValue)puElement).Data);
            }
            else
            {
                throw new InvalidOperationException(puElement.GetType().Name + " is not supported");
            }
        }

        protected override PuElement UnpackFromCore(MsgPack.Unpacker unpacker)
        {
            if (unpacker.IsArrayHeader)
            {
                return PuArrayTemplate.Instance.UnpackFrom(unpacker);
            }
            else if (unpacker.IsMapHeader)
            {
                return PuObjectTemplate.Instance.UnpackFrom(unpacker);
            }
            else
            {
                return PuValue.FromObject(unpacker.LastReadData);
            }
        }
    }
}
