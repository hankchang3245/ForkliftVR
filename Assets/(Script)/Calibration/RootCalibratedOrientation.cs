using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using edu.tnu.dgd.debug;


namespace edu.tnu.dgd.vr.calibration
{
    public class RootCalibratedOrientation : MonoBehaviour
    {
		private static RootCalibratedOrientation _instance;
        public Vector3 rightHandCalibrationPosition;

		public static RootCalibratedOrientation instance
		{
			// Singleton design pattern
			get
			{

				if (_instance == null)
				{
					_instance = FindObjectOfType<RootCalibratedOrientation>();
				}

				return _instance;
			}
		}


		private void Awake()
        {
            //ShowDebugLog.instance.Log(">>>>>>>>>>>>>>>>>>>> RootCalibratedOrientation Awake()");
            DontDestroyOnLoad(this);
        }

        private void OnDestroy()
        {
            //ShowDebugLog.instance.Log(">>>>>>>>>>>>>>>>>>>> RootCalibratedOrientation OnDestroy()");
        }


	}
}