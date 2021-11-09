using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using edu.tnu.dgd.audio;
using edu.tnu.dgd.game;
using edu.tnu.dgd.vrlearn;
using edu.tnu.dgd.forklift;

namespace edu.tnu.dgd.project.forklift
{
    public class Shelf : MonoBehaviour
    {
        public GameObject failedSignPrefab;
        private float prevContactTime = 0f;

        private void OnCollisionEnter(Collision collision)
        {
            string tag = collision.gameObject.tag;
            if (tag.IndexOf("Player") >= 0)
            {
                if (prevContactTime > 0f && (Time.time - prevContactTime) < 1f) // 在1秒內的碰撞只算一次
                {
                    prevContactTime = Time.time;
                    return;
                }
                prevContactTime = Time.time;

                ContactPoint[] contacts = new ContactPoint[collision.contactCount];
                collision.GetContacts(contacts);
                MarkSign(contacts);

                // 學習紀錄
                LearnController.instance.WriteExperience(Experience.VERB.FORKLIFT_COLLISION, this.gameObject.name, typeof(Shelf).ToString(), ForkliftController.instance.gameObject.transform, GameController.instance.elapsedTime);

                // 播放音效
                AudioClip clip = AudioController.instance.LoadClip(AudioHelper.GetAudioClipPath());
                AudioController.instance.PlayOneShot(clip);
            }
        }

        private void MarkSign(ContactPoint[] contacts)
        {
            if (contacts != null && contacts.Length > 0)
            {
                int cnt = 0;
                Vector3 pt = Vector3.zero;
                foreach (ContactPoint cp in contacts) // 計算中心點
                {
                    pt += cp.point;
                    cnt++;
                }
                if (cnt > 0)
                {
                    GameObject obj = Instantiate(failedSignPrefab, pt / cnt, Quaternion.identity);
                    FailedSignObjectCollector.instance.AddSignObject(obj);
                    // 累計錯誤數量
                    GameController.instance.failedCount++;
                }

            }
        }
    }

}
