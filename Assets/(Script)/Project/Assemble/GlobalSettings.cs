using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using edu.tnu.dgd.vrlearn;
using edu.tnu.dgd.util;

namespace edu.tnu.dgd.assemble
{
    public class GlobalSettings
    {
        private static PlayerPrefs playerPrefs;
        private static GlobalSettings _instance;

        public Learner currentLearner;

        public static GlobalSettings instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GlobalSettings();
                }

                return _instance;
            }
        }

        public GlobalSettings()
        {
            string userStr = PlayerPrefs.GetString("CurrentLearner");
            currentLearner = JsonHelper.fromJson<Learner>(userStr);
        }

        public void SaveLearner(Learner lr)
        {
            string userJson = JsonHelper.toJson<Learner>(lr);
            if (userJson != null && userJson.Length > 0)
            {
                PlayerPrefs.SetString("CurrentLearner", userJson);
                PlayerPrefs.Save();
            }
        }

    }

    public enum PartPositionStatus
    {
        None,
        InMachine,
        InHand,
        SlidingToMachine,
        LeavingFromMachine,
        JustInstantiate,
        NearMachine, /* 給螺絲用的，螺絲在螺絲孔外 */
        Thrown
    }

 
    public enum ContactPointMatchCode
    {
        None,
        Matched,
        ErrorPart,
        Error,
        IndexError
    }

    /* Part位置是何時決定的 */
    public enum PartPointType
    {
        Fixed,  /* 一般零組件 */
        RuntimeDecided  /* 像有多顆螺絲的零組件，每個螺絲是相同的Prefab，因此其最後完成的位置，要視其正在接觸的ContactPoint來決定其PartPoint */
    }

    /*  零組件的類型 */
    public enum PartCategory {
        Other,
        Screw,
        CaseWithoutBackPanel,
        CaseBackPanel,
        PowerSupply,
        Mainboard,
        HardDisk,
        CDROM,
        RAM,
        CPU,
        CPUFan,
        VideoCard,
        One_OpenedEnd_Rope,
        Two_OpenedEnd_Rope,
        RJ45CConnector,
        Peripherals
    }

    /* Part固定的方式 */
    public enum PartFixedMethod
    {
        UseScrew,   /* 螺絲，例如：硬碟，光碟機，主機板，... */
        UseLatch,   /* 卡榫，例如：記憶體，外殼背板，CPU */
        AtPoint,    /* 位置，例如：電腦外殼、螺絲，線材，... */
        NoFixedPoint /* 無固定位置，例如：滑鼠、鍵盤、螢幕 */
    }

    /* 使用螺絲起子時，螺絲移動方向 */
    public enum ScrewMovingDirection
    {
        In,
        Out
    }

    ///* RAM,CPU固定工具的狀態 */
    public enum FixingToolState
    {
        Closed,
        Opened,
        InHand
    }

    ///* RAM,CPU固定工具種類 */
    public enum FixingTool
    {
        CPU_Rod,
        CPU_Cover,
        RAM_Latch
    }

    public enum FixingToolAction
    {
        Closing = 1,
        Opening = 2
    }


}