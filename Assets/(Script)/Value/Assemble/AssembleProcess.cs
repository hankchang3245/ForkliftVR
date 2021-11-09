using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace edu.tnu.dgd.value
{
    [Serializable]
    public class AssembleProcess
    {
        public string nameCh;
        public string nameEn;
        public string[] steps;
        public string[] step0_voices;
        public string[] step_voices;
    }
}