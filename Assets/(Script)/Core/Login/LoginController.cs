using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using edu.tnu.dgd.web;
using edu.tnu.dgd.vrlearn;
using edu.tnu.dgd.debug;
using TMPro;
using UnityEngine.Assertions;

namespace edu.tnu.dgd.login
{

    public class LoginController : MonoBehaviour, IDataRequester<School>
    {
        public bool useVR = true;
        public InputField schoolIdInput;
        public TMP_Text schoolNameText;
        public InputField learnerIdInput;
        public TMP_Text errorMsgText;
        //public GameObject callerObject;
        public GameObject helpUI;

        [HideInInspector]
        public GameObject mainForm;

        private GameObject loginForm;

        public UnityEvent onClose;

        [HideInInspector]
        public School school;

        [HideInInspector]
        public Learner learner;

        private static LoginController _instance;
        public static LoginController instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<LoginController>();
                }

                return _instance;
            }
        }

        void Awake()
        {
            mainForm = this.gameObject.transform.Find("LoginForm/MainForm").gameObject;
            loginForm = this.gameObject.transform.Find("LoginForm").gameObject;
            Assert.IsNotNull(mainForm);
            Assert.IsNotNull(loginForm);
            GraphicRaycaster gray = loginForm.GetComponent<GraphicRaycaster>();
            OVRRaycaster oray = loginForm.GetComponent<OVRRaycaster>();

            if (useVR)
            {
                gray.enabled = false;
                oray.enabled = true;
            }
            else
            {
                gray.enabled = true;
                oray.enabled = false;

            }
        }


        void Start()
        {
            //ShowDebugLog.instance.Log("LoginController ..... ====>" + PlayerPrefs.GetString("CurrentLearner"));
            Learner lr = Learner.Load(false);
            if (lr != null)
            {
                //ShowDebugLog.instance.Log("LoginController ..... 2");
                schoolIdInput.text = lr.schoolId;
                //ShowDebugLog.instance.Log("schoolId ..... " + lr.schoolId);
                schoolNameText.text = lr.school;
                learnerIdInput.text = lr.id;
                //ShowDebugLog.instance.Log("LoginController ..... 3");
            } 
            else
            {
                schoolIdInput.text = "";
                schoolNameText.text = "";
                learnerIdInput.text = "";
                //ShowDebugLog.instance.Log("LoginController ..... 4");
                this.gameObject.SetActive(true);
            }

            errorMsgText.text = "";
        }

        public void LoginSuccess()
        {
            //ShowDebugLog.instance.Log("LoginController.LoginSuccess() ..... 8");
            Game g = new Game(schoolIdInput.text, learnerIdInput.text);
            learner = new Learner(learnerIdInput.text, school.name, schoolIdInput.text, g.guid, 0);
            learner.Save();
            //ShowDebugLog.instance.Log("LoginController.LoginSuccess() ..... 9");

            //AudioController.instance.PlayStartupAudio();
            DataPostController.instance.SubmitData(g);
            //ShowDebugLog.instance.Log("LoginController.LoginSuccess() ..... 10");
        }

        /**
         * Called by Button event
         * */
        public void QuerySchoolNameBySchoolId()
        {
            //ShowDebugLog.instance.Log("LoginController.QuerySchoolNameBySchoolId() ...... 1");
            // Guest 不須要查詢學校名稱
            if (schoolIdInput.text == "000000")
            {
                //ShowDebugLog.instance.Log("LoginController.QuerySchoolNameBySchoolId() ...... 2");
                schoolNameText.text = "<Guest>";
            }
            else
            {
                if (schoolIdInput.text.Length == 6)
                {
                    //ShowDebugLog.instance.Log("LoginController.QuerySchoolNameBySchoolId() ...... 3");
                    string url = DataPostController.GetJsonDataRequestUrlById(School.dataPostUri, schoolIdInput.text);
                    DataPostController.instance.SubmitRequest(this, url);
                    //ShowDebugLog.instance.Log("LoginController.QuerySchoolNameBySchoolId() ...... 4");
                } else
                {
                    schoolNameText.text = "";
                }
            }
        }

        public void OpenHelp()
        {
            if (helpUI != null)
            {
                helpUI.SetActive(true);
                mainForm.SetActive(false);
            }
        }

        public void OpenURL()
        {
            Application.OpenURL("http://xrl.tnu.edu.tw");
        }

        public void CloseHelp()
        {
            if (helpUI != null)
            {
                helpUI.SetActive(false);
                mainForm.SetActive(true);
            }
        }

        public void SetResponseData(School data)
        {
            
            this.school = data;
            if (this.school == null || this.school.name == "")
            {
                errorMsgText.text = "學校代碼錯誤或網路不通!";
                //schoolNameText.text = "<無資料>";
            }
            else
            {
                schoolNameText.text = school.name;
            }
            
        }

        public void SetCallbackMessage(string msg)
        {
            schoolNameText.text = msg;
        }

        private void OnDisable()
        {
            if (errorMsgText != null)
            {
                errorMsgText.text = "";
            }
        }

        public void ClearAll()
        {
            if (schoolIdInput != null)
            {
                schoolIdInput.text = "";
            }

            if (schoolNameText != null)
            {
                schoolNameText.text = "";
            }
            if (learnerIdInput != null)
            {
                learnerIdInput.text = "";
            }
            if (errorMsgText != null)
            {
                errorMsgText.text = "";
            }
        }

        public void ClearAllMsg()
        {

            if (schoolNameText != null)
            {
                schoolNameText.text = "";
            }
            if (errorMsgText != null)
            {
                errorMsgText.text = "";
            }
        }
        public void ClearErrorMsg()
        {

            if (errorMsgText != null)
            {
                errorMsgText.text = "";
            }
        }

        public void ConfirmLogin()
        {
            //ShowDebugLog.instance.Log("LoginController.ConfirmLogin() ... 5");

            string msg = "";

            if (CheckLogin(out msg))
            {
                //ShowDebugLog.instance.Log("LoginController.ConfirmLogin() ... 6");
                //callerObject.SetActive(true);
                LoginSuccess();

                if (onClose != null)
                {
                    onClose.Invoke();
                }

                LoginChecking.instance.hasLogined = true;

                this.gameObject.SetActive(false);

                //ShowDebugLog.instance.Log("LoginController.ConfirmLogin() ... 7");
            }
            else
            {
                errorMsgText.text = msg;
            }
        }
        private bool CheckLogin(out string msg)
        {
            //ShowDebugLog.instance.Log("LoginController.CheckLogin()");
            msg = "";
            if (schoolIdInput.text.Length == 0)
            {
                msg = "請輸入學校代碼。\n";
            }
            else if (schoolIdInput.text.Length < 6)
            {
                msg = "學校代碼需要6位數字。\n";
            }

            if (learnerIdInput.text.Length == 0)
            {
                msg = msg + "請輸入學號或帳號。";
            }
            msg = msg.Trim();

            if (msg.Length == 0)
            {
                return true;
            }

            return false;
        }

        private void OnDestroy()
        {
            _instance = null;
        }

    }

}
