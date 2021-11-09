using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace edu.tnu.dgd.outline
{
    public class ItemPackageOutline : BaseOutline
    {
        void Awake()
        {
            Transform tr = this.gameObject.transform.Find("ModelOutline");
            if (tr != null)
            {
                this.outline = tr.gameObject;
            }
        }

        private void Start()
        {
            ShowHintBlink(-1f);
        }
    }
}