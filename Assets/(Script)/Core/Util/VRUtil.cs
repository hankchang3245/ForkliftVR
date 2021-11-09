using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace edu.tnu.dgd.util
{
    public class VRUtil
    {
        public static Transform CopyTransform(Transform transform)
        {
            if (transform == null)
            {
                return null;
            }

            Transform tr2 = new GameObject().transform;

            tr2.position = transform.position;
            tr2.localPosition = transform.localPosition;

            tr2.rotation = transform.rotation;
            tr2.localRotation = transform.localRotation;

            tr2.localScale = transform.localScale;

            return tr2;

        }

        public static void PrintDictionary(Dictionary<object, object> dict)
        {
            foreach (KeyValuePair<object, object> kvp in dict)
            {
               Debug.LogFormat("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            }
        }

}

}