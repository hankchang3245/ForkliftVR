using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using edu.tnu.dgd.vehicle;
using edu.tnu.dgd.value;
using edu.tnu.dgd.debug;

namespace edu.tnu.dgd.project.forklift
{
    public class StationProperty : MonoBehaviour
    {
        public Transform _carOriginLocalTransform;
        public Transform _carOriginReverseLocalTransform;
        public GameObject forkliftCarObject;
        public GuideDataType guideDataType = GuideDataType.Basic;

        private TrafficPole[] trafficPoleList;

        
        public Transform CarOriginLocalTransform
        {
            get
            {
                return _carOriginLocalTransform;
            }
        }

        public Transform CarOriginReverseLocalTransform
        {
            get
            {
                return _carOriginReverseLocalTransform;
            }
        }
       

        void Awake()
        {
            VehicleController vehicleController = forkliftCarObject.GetComponent<VehicleController>();

            _carOriginLocalTransform = transform.Find("ForkliftCarPosition");
            vehicleController.OriginLocalPosition = _carOriginLocalTransform.localPosition;
            vehicleController.OriginLocalRotation = _carOriginLocalTransform.localRotation;

            _carOriginReverseLocalTransform = transform.Find("ForkliftCarReversePosition");
            vehicleController.OriginReverseLocalPosition = _carOriginReverseLocalTransform.localPosition;
            vehicleController.OriginReverseLocalRotation = _carOriginReverseLocalTransform.localRotation;

            trafficPoleList = PrepareTrafficPoles(transform.Find("Layout/TrafficPoleList"));

        }

        public void ResetTrafficPoleList()
        {
            
            foreach (TrafficPole tp in trafficPoleList)
            {
                if (tp.collisionCount > 0)
                {
                    StartCoroutine(ResetTrafficPole(tp));
                }
                
            }
        }

        public void StoreAllTrafficPolePosition()
        {
            foreach (TrafficPole tp in trafficPoleList)
            {
                tp.SaveOriginLocalPosition();
            }
        }

        IEnumerator ResetTrafficPole(TrafficPole tp)
        {
            tp.ResetTransform();

            yield return null;
        }

        public GuidePoint[] PrepareGuidePoints()
        {
            Transform guideRoot = transform.Find("GuidePointList");

            int count = guideRoot.childCount;
            GuidePoint[] guidePoints = new GuidePoint[count];
            for (int i = 0; i < count; i++)
            {
                guidePoints[i] = guideRoot.GetChild(i).GetComponent<GuidePoint>();
                guidePoints[i].childIndex = i;
                guidePoints[i].guideType = guideDataType;

                //guidePoints[i].Initialize(); // must be called
            }
            foreach (GuidePoint gp in guidePoints)
            {
                if (gp != null)
                {
                    gp.gameObject.SetActive(false);
                }
            }

            return guidePoints;
        }

        private TrafficPole[] PrepareTrafficPoles(Transform trafficPoleRoot)
        {
            int count = trafficPoleRoot.childCount;
            TrafficPole[] trafficPoleRoots = new TrafficPole[count];
            for (int i = 0; i < count; i++)
            {
                trafficPoleRoots[i] = trafficPoleRoot.GetChild(i).GetComponent<TrafficPole>();
            }


            return trafficPoleRoots;
        }
    }

}
