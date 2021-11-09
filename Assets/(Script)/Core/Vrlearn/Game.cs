using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using edu.tnu.dgd.util;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using edu.tnu.dgd.web;


namespace edu.tnu.dgd.vrlearn
{
    [System.Serializable]
    public class Game : BaseObject
    {
        //public static string dataPostUri = "/api/game/";
        public static string dataPostUri = ServerInfo.uri_prefix + "/game/";

        public enum GENDER
        {
            MALE = 1,
            FEMALE = 2
        }

        public string guid;
        public string school_id;
        public GENDER gender = GENDER.MALE;
        public string actor;
        public string client_ip;
        public string app_name;

        public Game(string schoolId, string actorId)
        {
            guid = Guid.NewGuid().ToString();
            school_id = schoolId;
            actor = actorId;
            client_ip = WebUtil.GetAllLocalIPv4WithString();
            app_name = Application.productName + "-" + Application.version;
        }
    }
}

