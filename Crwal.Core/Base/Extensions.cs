using Newtonsoft.Json;
using System;
using System.Text;
using System.Web.Script.Serialization;

namespace Crwal.Core.Base
{
    public static class Extensions
    {
        public static bool IsStringNullOrEmpty(this string obj)
        {
            return string.IsNullOrWhiteSpace(obj);
        }

        public static T ToObject<T>(this string json)
        {
            return json.IsStringNullOrEmpty()
                ? default
                : new JavaScriptSerializer
                {
                    MaxJsonLength = int.MaxValue
                }.Deserialize<T>(json);
        }

        public static string ToJson(this object obj)
        {
            return obj == null ? string.Empty : JsonConvert.SerializeObject(obj);
        }

        public static string BytesToString(this byte[] b)
        {
            return Encoding.UTF8.GetString(b, 0, b.Length);
        }

        public static byte[] StringToBytes(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public static int ToInt32(this string obj)
        {
            var r = 0;
            try
            {
                r = Convert.ToInt32(obj);
            }
            catch
            {
            }

            return r;
        }
    }
}