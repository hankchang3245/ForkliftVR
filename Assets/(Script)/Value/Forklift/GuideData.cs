using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using edu.tnu.dgd.project.forklift;

namespace edu.tnu.dgd.value
{
    [Serializable]
    public class GuideData
    {
        public int id;
        public string subject;
        public Step[] steps;
        public Remark[] remarks;
        //public string kind = "bas";
        public GuideDataType guideDataType = GuideDataType.Basic;

        public override string ToString()
        {
            return JsonUtility.ToJson(this);
        }

        public string GetFullRemarkString()
        {
            string result = "";
            for (int i = 0; i < remarks.Length; i++)
            {
                result += "<color=yellow>" + (i+1) + ".</color> " + remarks[i].text + System.Environment.NewLine;
            }
            return result;
        }

        public string GetAudioClipPath(SoundType type)
        {
            string kind = "bas";
            if (guideDataType == GuideDataType.Advanced)
            {
                kind = "adv";
            }
            return "SFX/" + type.ToString() + "/" + kind + "_"+ id.ToString().PadLeft(2, '0');
        }
    }

}
