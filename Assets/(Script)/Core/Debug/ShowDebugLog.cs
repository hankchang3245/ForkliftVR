using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace edu.tnu.dgd.debug
{
    public class ShowDebugLog : MonoBehaviour
    {
        private static ShowDebugLog _instance;
        private Text logText;
        private int lineCount = 1;
        public int maxLineCount = 10;
        [SerializeField]
        private bool enable = true;

        public static ShowDebugLog instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<ShowDebugLog>();
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (enable)
            {
                Transform logTr = transform.Find("teleport_marker_lookat_joint/teleport_marker_canvas/teleport_marker_canvas_text");
                if (logTr != null)
                {
                    logText = logTr.gameObject.GetComponent<Text>();
                    logText.text = "Showing debug log is starting ...";
                }
            }
            else
            {
                Transform logCanvas = transform.Find("teleport_marker_lookat_joint/teleport_marker_canvas");
                if (logCanvas != null)
                {
                    logCanvas.gameObject.SetActive(false);
                }

            }
        }

        
        public void Log(string msg, bool clearOther=false)
        {
            if (enable)
            {
                StartCoroutine(LogText(msg, clearOther));
            }
        }
        

        IEnumerator LogText(string msg, bool clearOther)
        {
            if (msg != null)
            {
                if (clearOther)
                {
                    logText.text = msg;
                    lineCount = 1;
                }
                else
                {
                    string alltext = logText.text;
                    if (lineCount >= maxLineCount)
                    {
                        int idx = alltext.LastIndexOf("\n");
                        alltext = alltext.Substring(0, idx);
                    }
                    logText.text = "[" + (lineCount % 10) + "] " + msg + "\n" + alltext;
                    lineCount++;
                }

                yield return null;
            }
            else
            {
                yield return null;
            }
        }

        public void Log(string subject, string msg)
        {
            Log(subject + ": " + msg);
        }

        public void Log(string subject, Vector3 pos)
        {
            Log(subject + ": (" + pos.x.ToString("F5") + ", " + pos.y.ToString("F5") + ", " + pos.z.ToString("F5") + ")");
        }

        public void Log(string subject, float val)
        {
            Log(subject + ": " + val.ToString("F5"));
        }

        public void Log(GameObject obj)
        {
            if (obj != null)
            {
                Log(obj.ToString());
            }
        }

        private void OnDestroy()
        {
            //_instance = null;
        }
    }

}
