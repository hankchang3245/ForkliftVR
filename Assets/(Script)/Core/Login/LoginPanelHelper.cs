using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;

namespace edu.tnu.dgd.login
{
    public class LoginPanelHelper : MonoBehaviour
    {
        public GameObject loginObject;
        public Transform location;

        public UnityEvent onBeforeOpen;
        public UnityEvent onClose;

        private static LoginPanelHelper _instance;

        public static LoginPanelHelper instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<LoginPanelHelper>();
                }

                return _instance;
            }
        }

        private void Awake()
        {
            Assert.IsNotNull(loginObject);
            loginObject.SetActive(false);


        }
        public void OpenLoginPanel()
        {
            
            if (onBeforeOpen != null)
            {
                onBeforeOpen.Invoke();
            }
            loginObject.SetActive(true);
            //GameObject panel = Instantiate(loginPanelPrefab, location.position, Quaternion.identity);
            LoginController login = loginObject.GetComponent<LoginController>();
            login.onClose = onClose;

        }
    }

}
