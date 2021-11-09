using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using edu.tnu.dgd.web;
using edu.tnu.dgd.util;
using edu.tnu.dgd.game;
using UnityEngine.SceneManagement;

namespace edu.tnu.dgd.vrlearn
{
    public class LearnController : MonoBehaviour
    {
        public bool enableSaveExperience = true;

        private Experience prevExperience = null;

        private static LearnController _instance;
        public static LearnController instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = FindObjectOfType<LearnController>();
                }

                return _instance;
            }
        }

        
        public void WriteExperience(Experience.VERB verb, string targetName, string targetDataType, Transform tr, float elapsedTime)
        {
            Learner lr = Learner.Load();
            
            if (lr.schoolId == "000000" || enableSaveExperience == false) // 如果是Guest則不紀錄
            {
                return;
            }

            Experience exp = new Experience();
            exp.actor = lr.id;
            exp.gameId = lr.gameId;
            exp.sessionId = lr.sessionId;
            exp.scene = SceneManager.GetActiveScene().name;
            exp.score = Mathf.FloorToInt(GameController.instance.progress);
            exp.target = RemoveExtraString(targetName);
            exp.target_dt = targetDataType;
            exp.target_tr = GetJsonFromTransform(tr);
            exp.verb = verb;
            exp.client_time = elapsedTime;

            if (prevExperience == null || !prevExperience.Skip(exp))
            {
                prevExperience = exp;
                DataPostController.instance.SubmitData(exp);
            }
        }


        public void WriteExperience(Experience.VERB verb, string targetName, string targetDataType, Transform tr)
        {
            WriteExperience(verb, targetName, targetDataType, tr, Time.time);
        }
        

        public static float Truncate(float value, int digits)
        {
            double mult = Math.Pow(10.0, digits);
            double result = Math.Truncate(mult * value) / mult;

            return (float)result;
        }

        private string RemoveExtraString(string val)
        {
            if (val == null)
            {
                return val;
            }

            return val.Replace("(Clone)", "");
        }

        private string GetJsonFromTransform(Transform tr)
        {
            PositionRotation pr = new PositionRotation(tr.position, tr.rotation.eulerAngles);

            return JsonHelper.toJson<PositionRotation>(pr);
        }

        private void OnDestroy()
        {
            _instance = null;
        }
    }

    [Serializable]
    class PositionRotation
    {
        public Vector3 position;
        public Vector3 rotation;

        public PositionRotation(Vector3 pos, Vector3 rot)
        {
            rotation = rot;
            position = pos;
        }
    }
}
