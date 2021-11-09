using System.ComponentModel;
using UnityEngine.SceneManagement;
using edu.tnu.dgd.web;

namespace edu.tnu.dgd.vrlearn
{
    [System.Serializable]
    public class School : BaseObject
    {

        //public static string dataPostUri = "/api/school/";
        public static string dataPostUri = ServerInfo.uri_prefix + "/school/";

        public string id;
        public int grade;
        public string name;
        public string type;
        public string city;
        public string city_code;
        public string addr;
        public string area_code;
        public string tel;
        public string web;
        public string misc;

        public School()
        {

        }

    }
}