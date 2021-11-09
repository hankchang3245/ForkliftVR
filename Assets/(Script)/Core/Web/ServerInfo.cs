using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace edu.tnu.dgd.web
{
    public class ServerInfo
    {
        // http://140.129.143.219:8000/school/011056/?format=json
        /* this should include port number if the post number is not equal to 80 */
        public static string host = "140.129.140.224";
        public static string uri_prefix = "/api";
        //public static string host = "127.0.0.1:8080";
        //public static string uri_prefix = "";
        public static string loginUid = "twlibuser";
        public static string loginPwd = "twlib1234";
        /* ths should start with '/' */
        public static string imagePostUri = "/upload/images/";
        /* ths should start with '/' */
        public static string dataPostUri = "/surveys/";
        /* ths should start with '/' and end with '/' */
        public static string imageDownloadUriPrefix = "/media/";

        public static string GetSecuredHost()
        {
            if ((loginUid == null || loginUid == "") && (loginPwd == null || loginPwd == ""))
            {
                return "http://" + host;
            }
            return "http://" + loginUid + ":" + loginPwd + "@" + host;
        }

        public static string GetHost()
        {
            return "http://" + host;
        }
        public static string GetImagePostUrl()
        {
            return GetSecuredHost() + imagePostUri;
        }

        public static string GetDataPostUrl()
        {
            return GetSecuredHost() + dataPostUri;
        }

        public static string GetDataPostUrl(string uri)
        {
            return GetSecuredHost() + uri;
        }

        public static string GetImageDownloadUrl(string filename)
        {
            return GetHost() + imageDownloadUriPrefix + filename;
        }
    }
}