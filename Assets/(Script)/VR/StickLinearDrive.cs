using UnityEngine;
using System.Collections;

namespace edu.tnu.dgd.misc
{
    //-------------------------------------------------------------------------
    /*[RequireComponent(typeof(Interactable))]*/
    public class StickLinearDrive : MonoBehaviour
    {
        public Transform startPosition;
        public Transform endPosition;


        public float initialMappingOffset = 0.03f; // 0.03數值是測試出來的，讓初始位置不會偏移太大
        protected int numMappingChangeSamples = 5;
        protected float[] mappingChangeSamples;
        protected float prevMapping = 0.0f;
        protected float mappingChangeRate;
        protected int sampleCount = 0;


        private Quaternion originalRotation;
        private float linearMappingValue;
        private AudioSource audioSource;
        public GameObject target;


        protected virtual void Awake()
        {
            mappingChangeSamples = new float[numMappingChangeSamples];
        }

        protected virtual void Start()
        {
            linearMappingValue = 0f;
            originalRotation = transform.rotation;
            audioSource = GetComponent<AudioSource>();
        }
        /*
        protected virtual void HandHoverUpdate(Hand hand)
        {
            GrabTypes startingGrabType = hand.GetGrabStarting();
            if (interactable.attachedToHand == null && startingGrabType != GrabTypes.None)
            {
                sampleCount = 0;
                mappingChangeRate = 0.0f;

                hand.AttachObject(gameObject, startingGrabType, attachmentFlags);
            }
        }

        protected virtual void HandAttachedUpdate(Hand hand)
        {
            UpdateLinearMapping(hand.transform);

            if (hand.IsGrabEnding(this.gameObject))
            {
                hand.DetachObject(this.gameObject);
            }
        }

        protected virtual void OnDetachedFromHand(Hand hand)
        {
            CalculateMappingChangeRate();
        }
        */


        protected void CalculateMappingChangeRate()
        {
            //Compute the mapping change rate
            mappingChangeRate = 0.0f;
            int mappingSamplesCount = Mathf.Min(sampleCount, mappingChangeSamples.Length);
            if (mappingSamplesCount != 0)
            {
                for (int i = 0; i < mappingSamplesCount; ++i)
                {
                    mappingChangeRate += mappingChangeSamples[i];
                }
                mappingChangeRate /= mappingSamplesCount;
            }
        }

        protected void UpdateLinearMapping(Transform updateTransform)
        {
            prevMapping = linearMappingValue; // linearMapping.value;
            //linearMapping.value = Mathf.Clamp01(initialMappingOffset + CalculateLinearMapping(updateTransform));

            float mapping = CalculateLinearMapping(updateTransform);
            linearMappingValue = Mathf.Clamp01(initialMappingOffset + mapping);
            mappingChangeSamples[sampleCount % mappingChangeSamples.Length] = (1.0f / Time.deltaTime) * (linearMappingValue - prevMapping);
            sampleCount++;

            //Debug.LogFormat(">>>>>>>>>{0}>>>>>>mapping={1}>>linearMappingValue={2}", sampleCount, mapping, linearMappingValue);

            transform.position = Vector3.Lerp(startPosition.position, endPosition.position, linearMappingValue);
            transform.rotation = originalRotation;
            Vector3 euler = target.transform.localEulerAngles;
            target.transform.localRotation = Quaternion.Euler(euler.x, linearMappingValue*180, euler.z);
         }

        protected float CalculateLinearMapping(Transform updateTransform)
        {
            Vector3 direction = endPosition.position - startPosition.position;
            float length = direction.magnitude;
            direction.Normalize();

            Vector3 displacement = updateTransform.position - startPosition.position;

            return Vector3.Dot(displacement, direction) / length;
        }
    }
}
