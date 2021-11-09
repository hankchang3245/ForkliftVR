using System.ComponentModel;
using UnityEngine.SceneManagement;
using System;
using edu.tnu.dgd.web;

namespace edu.tnu.dgd.vrlearn
{
    [System.Serializable]
    public class Experience : BaseObject
    {
        //public static string dataPostUri = "/api/exp/";
        public static string dataPostUri = ServerInfo.uri_prefix + "/exp/";

        public static string SCENE_LOGIN        = "Login";
        public static string SCENE_MAIN_MENU    = "MainMenu";

        /* 電腦組裝計畫 */
        public static string SCENE_GUIDE        = "VRGuide";
        public static string SCENE_TESTING      = "VRTesting";
        public static string SCENE_EXPLORE      = "VRExplore";

        /* 堆高機計畫  */
        public static string SCENE_ForkliftComponentBrief   = "ForkliftComponentBrief";
        public static string SCENE_ForkliftBasicTraining    = "ForkliftBasicTraining";
        public static string SCENE_ForkliftAdvTraining      = "ForkliftAdvTraining";
        public static string SCENE_ForkliftTesting          = "ForkliftTesting";

        public enum VERB
        {
            [Description("登入")] LOGIN = 0,
            [Description("登出")] LOGOUT = 1,
            [Description("切換場景")] TRANSITION = 2,

            /* 電腦組裝計畫 */
            [Description("拿取")] TAKE = 10,
            [Description("組裝")] INSTALL = 11,
            [Description("回收")] RECYCLE = 12,
            [Description("轉入螺絲")] SCREW_DRIVER = 13,
            [Description("開啟固定器")] OPEN_FIXING = 14,
            [Description("關閉固定器")] CLOSE_FIXING = 15,
            [Description("螺絲就位")] SCREW_READY = 16,
            [Description("錯誤：方位")] ERROR_ORIENTATION= 20,
            [Description("錯誤：其他")] ERROR_OTHER = 25,
            [Description("掉到地板")] FALL_TO_FLOOR = 30,

            /* 堆高機計畫  */
            [Description("碰撞")]         FORKLIFT_COLLISION = 50,
            [Description("壓線")]         FORKLIFT_STEP_ON_LINE = 51,
            [Description("歪斜")]         FORKLIFT_SKEW = 52,
            [Description("貨叉位置錯誤")]  FORKLIFT_WRONG_POSITION = 53,
            [Description("到達任務")]      FORKLIFT_REACH_TASK = 54
        }

        public string scene;
        public string actor;
        public VERB verb;
        public string target;
        public string gameId;
        public int sessionId = 1;
        public string target_dt;
        public string target_tr = "{}";
        public int score = 0;
        public float client_time = 0f;

        public Experience()
        {

        }

        public static Experience GetSample()
        {
            Experience exp = new Experience();
            exp.scene = SceneManager.GetActiveScene().name;
            exp.actor = "hankchang";
            exp.verb = VERB.LOGIN;
            exp.target = "光碟機";
            exp.gameId = Guid.NewGuid().ToString();
            exp.target_dt = "Part";
            exp.target_tr = "{}";
            exp.score = 0;
            exp.sessionId = 1;
            exp.client_time = 0f;

            return exp;
        }

        public override string ToString()
        {
            string retVal = string.Empty;
            retVal = "scene:" + this.scene + ", actor:" + this.actor + ", verb:" + this.verb + ", target:" + this.target + 
                ", gameId:" + this.gameId + ", target_dt:" + this.target_dt + ", target_tr:" + this.target_tr + 
                ", score:" + this.score + ", sessionId:" + this.sessionId + ", client_time:" + this.client_time; 

            return retVal;
        }

        public bool Skip(Experience exp)
        {
            if (exp.verb == VERB.ERROR_ORIENTATION || exp.verb == VERB.ERROR_OTHER)
            {
                bool match = (exp.actor == this.actor && exp.target == this.target && 
                    exp.sessionId == this.sessionId && exp.score == this.score && exp.gameId == this.gameId);

                return match;
            }
            else
            {
                return false;
            }
        }
    }
}