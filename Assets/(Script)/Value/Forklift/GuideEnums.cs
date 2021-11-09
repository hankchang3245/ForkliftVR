using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace edu.tnu.dgd.value
{

    public enum GuideDataType
    {
        Basic,
        Advanced
    }

    public enum GuideTriggerTiming
    {
        Enter,
        Stay,
        Exit
    }

    public enum GuideStopTriggerTiming
    {
        Time,
        ExitTrigger
    }
}
