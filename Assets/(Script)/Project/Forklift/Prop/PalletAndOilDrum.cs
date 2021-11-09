using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace edu.tnu.dgd.project.forklift
{
    // 棧板
    public class PalletAndOilDrum : MonoBehaviour
    {
        public GameObject[] props;

        private Vector3[] localPositions;
        private Quaternion[] localRotations;

        private void Awake()
        {
            localPositions = new Vector3[3];
            localRotations = new Quaternion[3];

            for (int i = 0; i < 3; i++)
            {
                localPositions[i] = props[i].transform.localPosition;
            }

            for (int i = 0; i < 3; i++)
            {
                localRotations[i] = props[i].transform.localRotation;
            }
        }

        public void ResetTransform()
        {
            this.transform.localPosition = Vector3.zero;
            this.transform.localRotation = Quaternion.identity;

            for (int i = 0; i < 3; i++)
            {
                if (this.gameObject.activeSelf)
                {
                    ResetMyTransform(props[i], localPositions[i], localRotations[i]);
                }
            }
        }

        private void ResetMyTransform(GameObject obj, Vector3 pos, Quaternion rot)
        {
            Rigidbody rig = obj.GetComponent<Rigidbody>();
            if (rig != null)
            {
                rig.useGravity = false;
                rig.isKinematic = true;
            }

            obj.transform.localPosition = pos;
            obj.transform.localRotation = rot;

            
            //物體在inactive的情況下不能調用協程。
            //解決方法：判斷物體是否可見，不可見則停止所有協程調用。
            if (this.gameObject.activeSelf)
            {
                StartCoroutine(EnableGravity(obj, 0.1f));
            }
            else
            {
                StopAllCoroutines();
            }
            
        }

        IEnumerator EnableGravity(GameObject obj, float delay)
        {
            yield return new WaitForSeconds(delay);

            Rigidbody rig = obj.GetComponent<Rigidbody>();
            if (rig != null)
            {
                rig.useGravity = true;
                rig.isKinematic = false;
            }
        }

    }
}
