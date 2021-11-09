using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace edu.tnu.dgd.util
{
    public static class JsonHelper
    {
        /// <summary> 把物件轉換為Json字串 </summary>
        /// <param name="obj">物件</param>
        public static string toJson<T>(T obj)
        {
            if (obj == null) return "null";
            if (typeof(T).IsArray)
            {
                Pack<T> pack = new Pack<T>();
                pack.data = obj;
                string json = JsonUtility.ToJson(pack);
                return json.Substring(8, json.Length - 9);
            }
            return JsonUtility.ToJson(obj);
        }
        /// <summary> 解析Json </summary>
        /// <typeparam name="T">型別</typeparam>
        /// <param name="json">Json字串</param>
        public static T fromJson<T>(string json)
        {
            if (json == "null" && typeof(T).IsClass) return default(T);
            if (typeof(T).IsArray)
            {
                json = "{\"data\":{data}}".Replace("{data}", json);
                Pack<T> Pack = JsonUtility.FromJson<Pack<T>>(json);
                return Pack.data;
            }
            return JsonUtility.FromJson<T>(json);
        }
        /// <summary> 內部包裝類 </summary>
        private class Pack<T>
        {
            public T data;
        }
    }
}