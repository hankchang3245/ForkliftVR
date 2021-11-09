using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace edu.tnu.dgd.value
{
    public class PartDataEventArgs : EventArgs
    {
        private PartData _source;

        public PartDataEventArgs(PartData _source)
        {
            this._source = _source;
        }

        public PartData Source
        {
            get { return _source; }
        }
    }

}
