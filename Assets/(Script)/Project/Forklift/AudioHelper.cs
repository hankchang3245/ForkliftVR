using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace edu.tnu.dgd.project.forklift
{
    public static class AudioHelper
    {

        public static string GetAudioClipPath()
        {
            return "SFX/" + SoundType.Other.ToString() + "/error1";
        }
    }
}


