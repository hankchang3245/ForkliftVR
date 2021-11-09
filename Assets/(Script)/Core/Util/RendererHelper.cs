using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace edu.tnu.dgd.util
{
    public class RendererHelper
    {
        public static void ChangeColor(GameObject obj, Color color)
        {
            if (obj == null)
            {
                return;
            }
            Renderer[] renders = obj.GetComponentsInChildren<Renderer>();

            for (int i = 0; renders != null && i < renders.Length; i++)
            {
                Material[] mats = renders[i].materials;
                foreach (Material mat in mats)
                {
                    mat.color = color;
                }
            }
        }
    }

}

