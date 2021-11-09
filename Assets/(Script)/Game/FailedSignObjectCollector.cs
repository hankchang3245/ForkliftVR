using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace edu.tnu.dgd.game
{
    public class FailedSignObjectCollector
    {
        private HashSet<GameObject> failedSignObjectSet;
        private static FailedSignObjectCollector _instance;

        public static FailedSignObjectCollector instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FailedSignObjectCollector();
                }

                return _instance;
            }
        }

        public FailedSignObjectCollector()
        {
            failedSignObjectSet = new HashSet<GameObject>();
        }

        public void AddSignObject(GameObject obj)
        {
            failedSignObjectSet.Add(obj);
        }

        public void RemoveAll()
        {
            failedSignObjectSet.Clear();
        }
    }

}
