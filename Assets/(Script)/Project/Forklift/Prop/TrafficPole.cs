using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using edu.tnu.dgd.audio;
using edu.tnu.dgd.game;
using edu.tnu.dgd.vrlearn;
using edu.tnu.dgd.forklift;
using edu.tnu.dgd.vehicle;
using UnityEngine.Assertions;

namespace edu.tnu.dgd.project.forklift
{

    public enum TrafficPoleState
    {
        Idle,
        Moved,
        Down
    }

    public class TrafficPole : MonoBehaviour
    {
        public GameObject signedObjectPrefab;

        private TrafficPoleState state = TrafficPoleState.Idle;

        [HideInInspector]
        public int collisionCount = 0;

        private Vector3 originLocalPosition;
        private Quaternion originLocalRotation;

        // 以下使用世界座標主要是用來計算距離
        private Vector3 originWorldPosition;
        private Quaternion originWorldRotation;

        private void Start()
        {
            SaveOriginLocalPosition();
        }

        public void SaveOriginLocalPosition()
        {
            originLocalPosition = this.transform.localPosition;
            originLocalRotation = this.transform.localRotation;

            originWorldPosition = this.transform.position;
            originWorldRotation = this.transform.rotation;
        }

        private GameObject CreateFailedSign()
        {
            Vector3 pos = new Vector3(originWorldPosition.x, originWorldPosition.y + 1f, originWorldPosition.z);
            GameObject result = Instantiate(signedObjectPrefab, pos, Quaternion.identity);

            return result;
        }

        public void ResetTransform()
        {
            collisionCount = 0;
            // 先將Rigidbody.useGravity 取消，因為有時候將交通錐用正之後，會倒掉
            Rigidbody rig = this.gameObject.GetComponent<Rigidbody>();
            rig.useGravity = false;
            rig.isKinematic = true;


            this.transform.localPosition = originLocalPosition;
            this.transform.localRotation = originLocalRotation;

            Invoke("EnableGravity", 0.2f);
        }

        private void EnableGravity()
        {
            Rigidbody rig = this.gameObject.GetComponent<Rigidbody>();
            rig.useGravity = true;
            rig.isKinematic = false;
        }

        private bool CheckCollider(string tag)
        {
            return tag.IndexOf("Player") >= 0 || tag.IndexOf("OilDrum") >= 0;
        }

        private void OnCollisionEnter(Collision collision)
        {
            string tag = collision.gameObject.tag;
            if (CheckCollider(tag) && collisionCount <= 0)
            {
                // 累計錯誤數量
                collisionCount = 1;
                FailedSignObjectCollector.instance.AddSignObject(CreateFailedSign());



                GameController.instance.failedCount++;

                // 學習紀錄
                LearnController.instance.WriteExperience(Experience.VERB.FORKLIFT_COLLISION, this.gameObject.name, typeof(TrafficPole).ToString(), ForkliftController.instance.gameObject.transform, GameController.instance.elapsedTime);

                // 播放音效
                AudioClip clip = AudioController.instance.LoadClip(AudioHelper.GetAudioClipPath());
                AudioController.instance.PlayOneShot(clip);
            }
        }

        private void UpdateTrafficPoleState()
        {
            float dist = Vector3.Distance(this.transform.position, originWorldPosition) * 100;
            float angle = Quaternion.Angle(this.transform.rotation, originWorldRotation);

            if (angle < 5f && dist > 2f)
            {
                state = TrafficPoleState.Moved;
                //RendererHelper.ChangeColor(signedObject, touchedColor);
            }
            else if (angle >= 5f)
            {
                state = TrafficPoleState.Down;
                //RendererHelper.ChangeColor(signedObject, downColor);
            }
            else
            {
                state = TrafficPoleState.Idle;
            }
        }

    }

}
