using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using edu.tnu.dgd.value;
using edu.tnu.dgd.project.forklift;
using UnityEngine.Assertions;
using TMPro;
using edu.tnu.dgd.vrlearn;
using edu.tnu.dgd.forklift;

namespace edu.tnu.dgd.game
{
    public class ForkliftRelocationController : MonoBehaviour
    {
        private GameObject[] locationList;

        private static ForkliftRelocationController _instance;

        public static ForkliftRelocationController instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<ForkliftRelocationController>();
                }

                return _instance;
            }
        }

        private void Awake()
        {
            locationList = PrepareRestartPositions();
        }

        public GameObject[] PrepareRestartPositions()
        {
            Transform root = transform.Find("RestartPositionList");

            int count = root.childCount;
            GameObject[] points = new GameObject[count];
            for (int i = 0; i < count; i++)
            {
                points[i] = root.GetChild(i).gameObject;
            }


            return points;
        }

        public GameObject GetLocation(int idx)
        {
            return locationList[idx];
        }

        public int GetCurrentChildIndex(int idx)
        {
            return GetCurrentGuidePoint(idx).childIndex;
        }

        public GuidePoint GetCurrentGuidePoint(int idx)
        {
            RelatedGuidePoint rgp = GetLocation(idx).gameObject.GetComponent<RelatedGuidePoint>();

            return rgp.relatedGuidePoint;
        }

        public void CurrentChildIndexChanged(int currIndex)
        {
            /*
            for (int i = 0; i < locationList.Length; i++)
            {
                GuidePoint gp = locationList[i].GetComponent<RelatedGuidePoint>().relatedGuidePoint;

                if (gp.childIndex <= currIndex)
                {
                    locationList[i].SetActive(false);
                }
                else
                {
                    locationList[i].SetActive(false);
                }
            }
            */
        }

    }
}

