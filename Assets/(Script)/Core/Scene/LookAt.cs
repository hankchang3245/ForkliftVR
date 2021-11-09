using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace edu.tnu.dgd.scene
{
    public class LookAt : MonoBehaviour
    {
        private Transform ThisTransform = null;
        public Transform Target = null;
        public float RotateSpeed = 100f;

        void Awake()
        {
            ThisTransform = GetComponent<Transform>();
        }

        void Start()
        {
            StartCoroutine(TrackRotation(Target));
        }

        private void Update()
        {
            
            //Debug.Log(Player.instance.hmdTransform.transform.rotation.eulerAngles);
        }

        IEnumerator TrackRotation(Transform Target)
        {
            while (true)
            {
                if (ThisTransform != null && Target != null)
                {
                    Vector3 relativePos = Target.position - ThisTransform.position;
                    Quaternion NewRotation = Quaternion.LookRotation(relativePos);
                    ThisTransform.rotation = Quaternion.RotateTowards(ThisTransform.rotation, NewRotation, RotateSpeed * Time.deltaTime);
                }
                yield return null;
            }
        }

        public void ChooseScene(string name)
        {
            Debug.Log(name);
        }

        void OnDrawGizmos()
        {
            
            Gizmos.color = Color.red;
            //Gizmos.DrawLine(Player.instance.rigSteamVR.transform.position, Player.instance.rigSteamVR.transform.rotation.eulerAngles.normalized * 5f);
        }
    }

}
