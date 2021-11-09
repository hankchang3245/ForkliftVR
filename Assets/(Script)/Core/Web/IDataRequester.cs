using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace edu.tnu.dgd.web
{
    interface IDataRequester<T>
    {

        void SetCallbackMessage(string msg);
        void SetResponseData(T obj);
    }

}
