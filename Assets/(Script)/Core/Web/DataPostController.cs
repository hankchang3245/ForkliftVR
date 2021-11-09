using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.IO;
using edu.tnu.dgd.vrlearn;
using edu.tnu.dgd.util;
using edu.tnu.dgd.login;
using UnityEngine.SceneManagement;
using System;

namespace edu.tnu.dgd.web
{
    public class DataPostController : MonoBehaviour
    {
        private Image imageToDisplayQRCode;
        private string email = "hankchang@mail.tnu.edu.tw";

        private static DataPostController _instance;
        public static DataPostController instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<DataPostController>();
                }

                return _instance;
            }
        }

        private void Start()
        {
        }

        public void TakePicture()
        {
            StartCoroutine(CaptureScreenToImageAndSubmit());
        }

        public void SubmitExperienceData()
        {
            Experience exp = Experience.GetSample();
            StartCoroutine(AsyncPostJSONData<Experience>(exp));
        }

        public void SubmitData(Experience val)
        {
            StartCoroutine(AsyncPostJSONData<Experience>(val));
        }

        public void SubmitData(Game val)
        {
            StartCoroutine(AsyncPostJSONData<Game>(val));
        }



        IEnumerator CaptureScreenToImageAndSubmit()
        {
            yield return new WaitForEndOfFrame();

            // Capture Screen
            int width = Screen.width;
            int height = Screen.height;
            var tex = new Texture2D(width, height, TextureFormat.RGB24, false);

            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();

            byte[] bytes = tex.EncodeToPNG();
            Destroy(tex);

            string fn = WebUtil.CreateGuidFileNameWithExtension(".png");
            File.WriteAllBytes(LocalInfo.GetImageTempSavePath(fn), bytes);

            GenerateQRCode(ServerInfo.GetImageDownloadUrl(fn));

            WWWForm form = new WWWForm();
            form.AddField("email", email);
            form.AddBinaryData("image", bytes, fn, "image/png");

            StartCoroutine(AsyncPostImage(form));
        }

        private void GenerateQRCode(string content)
        {
            Texture2D encoded = new Texture2D(256, 256);

            Color32[] color32 = WebUtil.EncodeQRCode(content, encoded.width, encoded.height);
            encoded.SetPixels32(color32);
            encoded.Apply();
            imageToDisplayQRCode.GetComponent<Image>().sprite = Sprite.Create(encoded, new Rect(0, 0, encoded.width, encoded.height), Vector2.zero);
        }

        IEnumerator AsyncPostJSONData(object dataObj)
        {
            Dictionary<string, string> postHeader = new Dictionary<string, string>();
            postHeader.Add("Content-Type", "application/json");

            string json = JsonUtility.ToJson(dataObj);
            Debug.Log(json);
            byte[] formData = System.Text.Encoding.UTF8.GetBytes(json) as byte[];

            using (WWW www = new WWW(ServerInfo.GetDataPostUrl(), formData, postHeader))
            {
                yield return www;
                if (www.error != null)
                {
                    Debug.LogFormat("ERROR: " + www.error);
                }
                else
                {
                    Debug.Log("Finished Uploading JSON Data.");
                }
            }
        }

        IEnumerator AsyncPostJSONData<T>(T dataObj) where T : BaseObject
        {
            Dictionary<string, string> postHeader = new Dictionary<string, string>();
            postHeader.Add("Content-Type", "application/json");

            string json = "";
            string dataPostUri = ServerInfo.dataPostUri;
            if (dataObj is Experience)
            {
                dataPostUri = Experience.dataPostUri;
                json = processClientTime(JsonUtility.ToJson(dataObj));
            }
            else if (dataObj is Game)
            {
                dataPostUri = Game.dataPostUri;
                json = JsonUtility.ToJson(dataObj);
            }

            //Debug.Log(json);
            byte[] formData = System.Text.Encoding.UTF8.GetBytes(json) as byte[];

            //Debug.Log("dataPostUri: " + ServerInfo.GetDataPostUrl(dataPostUri));
            using (WWW www = new WWW(ServerInfo.GetDataPostUrl(dataPostUri), formData, postHeader))
            {
                yield return www;
                if (www.error != null)
                {
                    Debug.Log("ERROR: " + www.error);
                }
                else
                {
                    //Debug.Log("Finished Uploading JSON Data.");
                }
            }
        }

        // Experience的client_time轉成JSON時，其float的小數點會多出來
        private string processClientTime(string val)
        {
            
            int idx = val.LastIndexOf(".");
            if (val.Length > (idx + 4))
            {
                return val.Substring(0, idx + 4) + "}";
            }

            return val;
        }

        IEnumerator AsyncPostImage(WWWForm formData)
        {
            using (UnityWebRequest web = UnityWebRequest.Post(ServerInfo.GetImagePostUrl(), formData))
            {
                web.chunkedTransfer = false;
                yield return web.SendWebRequest();
                if (web.isNetworkError || web.isHttpError)
                {
                    Debug.Log("ERROR: " + web.error);
                }
                else
                {
                    imageToDisplayQRCode.gameObject.SetActive(true);
                    Debug.Log("Finished Uploading Image.");
                }
            }
        }

        IEnumerator AsyncPostFormData(WWWForm formData)
        {
            using (UnityWebRequest web = UnityWebRequest.Post(ServerInfo.GetDataPostUrl(), formData))
            {
                web.chunkedTransfer = false;
                yield return web.SendWebRequest();
                if (web.isNetworkError || web.isHttpError)
                {
                    Debug.Log("ERROR: " + web.error);
                }
                else
                {
                    Debug.Log("Finished Uploading Form Data.");
                }
            }
        }

        public void SubmitRequest(LoginController requester, string url)
        {
            StartCoroutine(AsyncGetJsonData<LoginController, School>(requester, url));
        }

        public static string GetJsonDataRequestUrlById(string uri, string id)
        {
            return ServerInfo.GetSecuredHost() + uri + id + "/?format=json";
        }

        IEnumerator AsyncGetJsonData<R, T>(R requester, string url) where R: IDataRequester<T>
        {
            //Debug.Log("URL:" + url);
            using (UnityWebRequest web = UnityWebRequest.Get(url))
            {
                requester.SetCallbackMessage("<資料查詢中...>");
                yield return web.SendWebRequest();

                if (web.isNetworkError)
                {
                    Debug.LogError("NETWORK ERROR: " + web.error);
                    requester.SetCallbackMessage("<網路無法連線>");
                    //requester.SetResponseData(default(T)); // default(T) return null
                }
                else if (web.isHttpError)
                {
                    Debug.LogError("HTTP ERROR: " + web.error);
                    if (web.responseCode == 404)
                    {
                        requester.SetCallbackMessage("<查無資料>");
                    }
                    else
                    {
                        requester.SetCallbackMessage("<伺服器錯誤>");
                    }
                }
                else
                {
                    T result = JsonHelper.fromJson<T>(web.downloadHandler.text);

                    requester.SetResponseData(result);
                    //Debug.Log("Data Received:" + web.downloadHandler.text);
                }
            }
        }

        private string FormatHttpHeader(Dictionary<string, string> headers)
        {
            if (headers == null)
            {
                return "";
            }

            string result = "";
            foreach (string h in headers.Keys)
            {
                string outstr;
                bool b = headers.TryGetValue(h, out outstr);
                result = result + h + ": " + outstr + "\n";
            }

            return result;
        }
    }
}
