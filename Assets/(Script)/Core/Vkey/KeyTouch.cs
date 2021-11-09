using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace edu.tnu.dgd.vkey
{

    public enum KeyType
    {
        General,
        Back,
        SwitchCase,
        Clear,
        Done,
        Cancel
    }
    public class KeyTouch : MonoBehaviour
    {
        private float deepth = 0.01f;

        private float prevLen = 0f;
        private float speed = 0.004f;

        private bool motionEnabled = false;
        private AudioSource audio;
        private TextMesh textMesh;
        private KeyboardHandler keyboardHandler;

        public KeyType keyType = KeyType.General;
        private Color yellow = new Color(1f, 1f, 0.25f);
        private Vector3 originPosition;


        private void Awake()
        {
            Transform tr = this.transform.Find("ButtonTextLabel");
            textMesh = tr.gameObject.GetComponent<TextMesh>();
            audio = GetComponent<AudioSource>();
        }


        void Start()
        {
            keyboardHandler = GetComponentInParent<KeyboardHandler>();
            originPosition = this.gameObject.transform.position;
        }


        void Update()
        {

        }

        public void KeyPress()
        {
            if (!motionEnabled)
            {
                motionEnabled = true;
                StartCoroutine(Motion());
            }

        }

        private IEnumerator Motion()
        {
            audio.Play();
            textMesh.color = yellow;
            while (prevLen <= deepth)
            {
                prevLen += speed;
                transform.Translate(Vector3.forward * speed);

                yield return new WaitForSeconds(0.05f);
            }

            prevLen = 0f;
            KeyPressedHandler();

            while (prevLen < deepth)
            {
                prevLen += speed;
                transform.Translate(-Vector3.forward * speed);

                yield return new WaitForSeconds(0.05f);
            }

            ResetColorPosition();
            prevLen = 0f;

        }


        private void KeyPressedHandler()
        {
            switch (keyType)
            {
                case KeyType.Back:

                    keyboardHandler.Back();
                    break;

                case KeyType.SwitchCase:
                    keyboardHandler.SwitchCase();
                    break;

                case KeyType.Cancel:
                    keyboardHandler.Cancel();
                    break;

                case KeyType.Done:
                    keyboardHandler.Done();
                    break;

                case KeyType.Clear:
                    keyboardHandler.Clear();
                    break;

                default:
                    keyboardHandler.Append(textMesh.text);
                    break;
            }
        }

        private void OnDisable()
        {
            ResetColorPosition();
        }

        private void ResetColorPosition()
        {
            motionEnabled = false;
            this.gameObject.transform.position = originPosition;
            textMesh.color = Color.white;           
        }
    }
}

