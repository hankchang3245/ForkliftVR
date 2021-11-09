using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using edu.tnu.dgd.util;

namespace edu.tnu.dgd.web
{
    public class LocalInfo
    {
        /* this MUST end with '/'*/
        public static string imageTempFolder = "snapshot/";

        public static string GetImageTempSavePath(string filename)
        {
            string path = Application.dataPath + "/../" + imageTempFolder;
            path = WebUtil.CreateFolder(path);
            return path + "/" + filename;
        }
    }
}

