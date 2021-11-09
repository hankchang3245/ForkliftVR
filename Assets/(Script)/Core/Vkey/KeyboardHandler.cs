using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace edu.tnu.dgd.vkey
{
    public class KeyboardHandler : MonoBehaviour
    {

        public TextMesh displayTextMesh;
        public GameObject firstPanel;
        public GameObject secondPanel;
        [Range(1, 30)]
        public int lengthLimit = 10;

        public UnityEvent onFinished;
        // when i'm activated, caller will be deactivated.
        public GameObject caller;
        private Transform keyboard;
        private InputField inputField;
        private Collider[] myColliders;


        public string text
        {
            get
            {
                return displayTextMesh.text;
            }

            set
            {
                displayTextMesh.text = value;
            }
        }

        void Start()
        {
            keyboard = this.gameObject.transform.Find("keyboard");
            myColliders = this.gameObject.GetComponentsInChildren<Collider>(true);
        }

        public void OpenPanel()
        {
            this.text = "";
            keyboard.gameObject.SetActive(true);
            StartCoroutine(DisableAllColliderForSecond(myColliders));
            caller.SetActive(false);
        }

        IEnumerator DisableAllColliderForSecond(Collider[] allcols)
        {

            foreach (Collider co in allcols)
            {
                co.enabled = false;
            }

            yield return new WaitForSeconds(1);

            foreach (Collider co in allcols)
            {
                co.enabled = true;
            }

        }

        public void ClosePanel()
        {
            inputField.text = this.text;
            keyboard.gameObject.SetActive(false);

            caller.SetActive(true);
            Collider[] cols = caller.GetComponentsInChildren<Collider>(true);
            StartCoroutine(DisableAllColliderForSecond(cols));
        }

        public void SwitchCase()
        {
            if (firstPanel.activeSelf)
            {
                if (firstPanel != null)
                {
                    firstPanel.SetActive(false);
                }
                if (secondPanel != null)
                {
                    secondPanel.SetActive(true);
                }
            }
            else
            {
                if (firstPanel != null)
                {
                    firstPanel.SetActive(true);
                }
                if (secondPanel != null)
                {
                    secondPanel.SetActive(false);
                }
                
            }
        }

        public void SetInputField(InputField inp)
        {
            inputField = inp;
        }

        public void Done()
        {
            onFinished.Invoke();
            ClosePanel();
        }

        public void Append(string str)
        {
            if(displayTextMesh.text.Length >= lengthLimit)
            {
                return;
            }
            displayTextMesh.text = displayTextMesh.text + str;
        }

        public void Cancel()
        {
            keyboard.gameObject.SetActive(false);
            caller.SetActive(true);
        }

        public void Clear()
        {
            displayTextMesh.text = "";
        }

        public void Back()
        {
            if (displayTextMesh.text.Length > 0)
            {
                displayTextMesh.text = displayTextMesh.text.Substring(0, displayTextMesh.text.Length - 1);
            }
        }
    }
}

