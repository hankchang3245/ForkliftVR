using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using ZXing;
using ZXing.QrCode;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace edu.tnu.dgd.util
{
    public class WebUtil
    {
        public static Color32[] EncodeQRCode(string textForEncoding, int width, int height)
        {
            //開始進行編碼動作
            BarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = height,
                    Width = width
                }
            };
            return writer.Write(textForEncoding);
        }

        public static string CreateGUIDString()
        {
            System.Guid guid = System.Guid.NewGuid();

            return guid.ToString("N");
        }

        /* the parameter ext MUST start with '.' */
        public static string CreateGuidFileNameWithExtension(string ext)
        {
            return CreateGUIDString() + ext;
        }

        public static Dictionary<string, object> GetDictionaryFromType(object atype)
        {
            if (atype == null) return new Dictionary<string, object>();
            Type t = atype.GetType();
            PropertyInfo[] props = t.GetProperties();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (PropertyInfo prp in props)
            {
                object value = prp.GetValue(atype, new object[] { });
                dict.Add(prp.Name, value);
            }
            return dict;
        }

        public static string CreateFolder(string path)
        {
            try
            {
                DirectoryInfo di = Directory.CreateDirectory(path);
                if (di.Exists == true)
                {
                    return di.FullName;
                }
            }
            catch (Exception e)
            {

            }
            finally { }

            return null;
        }

        // usage: GetLocalIPv4(NetworkInterfaceType.Ethernet).FirstOrDefault();
        public static List<string> GetAllLocalIPv4(NetworkInterfaceType _type)
        {
            try
            {
                List<string> ipAddrList = new List<string>();
                foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                    {
                        foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                ipAddrList.Add(ip.Address.ToString());
                            }
                        }
                    }
                }

                return ipAddrList;
            }
            catch (Exception ex)
            {
                Debug.LogError("GetAllLocalIPv4 Error：" + ex.Message);
            }

            return new List<string>();
        }

        public static string GetAllLocalIPv4WithString(NetworkInterfaceType _type)
        {

            StringBuilder sb = new StringBuilder();
            foreach (string ip in GetAllLocalIPv4(_type))
            {
                sb.Append("<" + _type.ToString() + ":" + ip + ">");
            }
            if (sb.Length > 0)
            {
                return sb.ToString();
            }

            return null;
        }

        public static string GetAllLocalIPv4WithString()
        {
            NetworkInterfaceType[] types = { NetworkInterfaceType.Ethernet, NetworkInterfaceType.Wireless80211};

            StringBuilder sb = new StringBuilder();
            foreach (NetworkInterfaceType _type in types)
            {
                foreach (string ip in GetAllLocalIPv4(_type))
                {
                    sb.Append("<" + _type.ToString() + ":" + ip + ">");
                }
            }

            if (sb.Length > 0)
            {
                return sb.ToString();
            }

            return "N/A";
        }
    }
}