using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace edu.tnu.dgd.value
{
    public class PartDataController : MonoBehaviour
    {
        public Text nameChText;
        public Text nameEnText;
        public Text descText;
        public Text descTitleText;
        public Text specText;
        public Text specTitleText;

        private PartData partData;


        void Update()
        {
            if (partData != null)
            {
                if (nameChText != null) nameChText.text = partData.nameCh;
                if (nameEnText != null) nameEnText.text = partData.nameEn;
                if (descText != null) descText.text = partData.desc;
                if (specText != null) specText.text = partData.spec;
                if (specTitleText != null) specTitleText.text = "規格：";
                if (descTitleText != null) descTitleText.text = "功能描述：";
            }
            else
            {
                if (nameChText != null) nameChText.text = "";
                if (nameEnText != null) nameEnText.text = "";
                if (descText != null) descText.text = "";
                if (specText != null) specText.text = "";
                if (specTitleText != null) specTitleText.text = "";
                if (descTitleText != null) descTitleText.text = "";
            }
        }

        public void ShowPartData(PartData partData)
        {
            this.partData = partData;
        }
    }
}