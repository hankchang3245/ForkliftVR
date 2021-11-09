using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace edu.tnu.dgd.vrlearn
{
    //-------------------------------------------------------------------------
    public class LearnerSetting : MonoBehaviour
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
        private float playerOriginalHeight;
        public Slider heightSlider;
        public float heightRatio = 0.7f;
        private bool hasGetPlayerOriginalHeight = false;
        public Text heightText;
        public Transform playerStartPosition;


        protected virtual void Awake()
        {
            mappingChangeSamples = new float[numMappingChangeSamples];
        }

        protected virtual void Start()
        {
            linearMappingValue = 0f;
            originalRotation = transform.rotation;
        }

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

            float mapping = CalculateLinearMapping(updateTransform);
            linearMappingValue = Mathf.Clamp01(initialMappingOffset + mapping);
            mappingChangeSamples[sampleCount % mappingChangeSamples.Length] = (1.0f / Time.deltaTime) * (linearMappingValue - prevMapping);
            sampleCount++;

            transform.position = Vector3.Lerp(startPosition.position, endPosition.position, linearMappingValue);
            transform.rotation = originalRotation;

            heightSlider.value = linearMappingValue - 0.3f;
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
