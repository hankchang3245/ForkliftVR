using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using edu.tnu.dgd.value;
using UnityEngine.Assertions;
using edu.tnu.dgd.game;

namespace edu.tnu.dgd.project.forklift
{
    public class GuidePoint : MonoBehaviour
    {
        public int childIndex;
        public int guideId;
        public GuideDataType guideType = GuideDataType.Basic;
        public string triggerTag;
        public GuideTriggerTiming triggerTiming = GuideTriggerTiming.Enter;
        public GuideStopTriggerTiming stopTriggerTiming = GuideStopTriggerTiming.ExitTrigger;
        public float triggerDurationTime = 10f; // 觸發持續時間
        private float elapsedTime = 0f;

        private GuideData guideData;
        //private Text text;

        public bool _enable = true;
        private Collider[] colliders;

        [HideInInspector]
        public bool fired = false;
        
        void Awake()
        {
            colliders = gameObject.GetComponents<Collider>();
            guideData = GuideDataStore.instance.FindDataById(guideType, guideId);
            //text = transform.Find("Canvas/Text").GetComponent<Text>();
            //text.text = "" + guideData.id;
        }

        /*
        public void Initialize()
        {

        }
        */

        public string GetAudioClipPath(SoundType type = SoundType.Female)
        {
            return guideData.GetAudioClipPath(type);
        }
        private void Update()
        {
            if (stopTriggerTiming == GuideStopTriggerTiming.Time)
            {
                if (elapsedTime > triggerDurationTime)
                {
                    FireStopTrigger();
                }
                else
                {
                    elapsedTime += Time.deltaTime;
                }
                
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (_enable && other.gameObject.CompareTag(triggerTag) && triggerTiming == GuideTriggerTiming.Enter)
            {
                FireTrigger();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (_enable && other.gameObject.CompareTag(triggerTag) && triggerTiming == GuideTriggerTiming.Stay)
            {
                FireTrigger();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (_enable && other.gameObject.CompareTag(triggerTag) && triggerTiming == GuideTriggerTiming.Exit)
            {
                FireTrigger();
            }

            if (_enable && other.gameObject.CompareTag(triggerTag) && stopTriggerTiming == GuideStopTriggerTiming.ExitTrigger)
            {
                FireStopTrigger();
            }
        }

        private void FireTrigger()
        {
            elapsedTime = 0f;
            GuideController.instance.FireGuidePoint(this);
        }
        private void FireStopTrigger()
        {
            elapsedTime = 0f;
            GuideController.instance.FireStopGuidePoint(this);
        }

    }
}

