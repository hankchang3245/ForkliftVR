using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace edu.tnu.dgd.value
{
    public class AssembleProcessController : MonoBehaviour
    {
        public Text nameChText;
        public Text nameEnText;
        public Text[] stepsText;

        private AssembleProcess processData;

        void Start()
        {

        }


        void Update()
        {
            if (processData != null)
            {
                nameChText.text = processData.nameCh;
                nameEnText.text = processData.nameEn;

                for (int i = 0; i < stepsText.Length; i++)
                {
                    if (i < processData.steps.Length)
                    {
                        stepsText[i].text = "<color=yellow>步驟" + (i + 1) + "：</color>" + processData.steps[i];
                    } else
                    {
                        stepsText[i].text = "";
                    }
                    
                }
            }
            else
            {
                for (int i = 0; i < stepsText.Length; i++)
                {
                    stepsText[i].text = "";
                }
            }
        }

        public void ShowAssembleProcess(AssembleProcess data)
        {
            this.processData = data;
        }
    }
}