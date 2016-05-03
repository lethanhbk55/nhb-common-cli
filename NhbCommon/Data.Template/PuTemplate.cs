using MsgPack.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhbCommon.Data.Template
{
    public abstract class PuTemplate<T> : MessagePackSerializer<T>
    {
        public PuTemplate()
            : base(SerializationContext.Default)
        {

        }

        private GenericTypeTemplate _genericTypeTemplate = new PuGenericTypeTemplate();

        public GenericTypeTemplate GenericTypeTemplate
        {
            get { return _genericTypeTemplate; }
            set { _genericTypeTemplate = value; }
        }
    }
}
