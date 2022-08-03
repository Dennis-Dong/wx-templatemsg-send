using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PrettyNotify
{
    public static class Extensions
    {
        /// <summary>
        /// String Is Null
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool NullOrEmpty(this string str)
        {
            return str == null || string.IsNullOrEmpty(str) && string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// String Is Not Null
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool NotNullOrEmpty(this string str)
        {
            return NullOrEmpty(str);
        }

        /// <summary>
        /// 将string类型转换成int类型
        /// </summary>
        /// <param name="s">目标字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static int ToInt(this string s, int defaultValue = 0)
        {
            if (s.NullOrEmpty()) return defaultValue;
            return int.TryParse(s, out var result) ? result : defaultValue;
        }

        /// <summary>
        /// 将decimal类型转换成int类型
        /// </summary>
        /// <param name="d">目标字符串</param>
        /// <returns></returns>
        public static int ToInt(this decimal d)
        {
            return Convert.ToInt32(d);
        }

        /// <summary>
        /// 将string类型转换成datetime类型
        /// </summary>
        /// <param name="s">目标字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>转换失败则返回defaultValue</returns>
        public static DateTime ToDateTime(this string s, DateTime defaultValue)
        {
            if (s.NullOrEmpty()) return defaultValue;
            return DateTime.TryParse(s, out var result) ? result : defaultValue;
        }

        /// <summary>
        /// 将string类型转换成datetime类型
        /// </summary>
        /// <param name="s">目标字符串</param>
        /// <returns>转换失败则返回当前时间</returns>
        public static DateTime ToDateTime(this string s)
        {
            return ToDateTime(s, DateTime.Now);
        }

        /// <summary>
        /// yyyy-MM-dd hh:mm:ss 转 yyyyMMddhhmmss
        /// </summary>
        /// <param name="dateTime">要转换的时间</param>
        /// <param name="dateFormatCurrent">转换前的额时间格式</param>
        /// <param name="dateFormatNew">转换后的时间格式</param>
        /// <returns></returns>
        public static string ToDateTimeStr(this string dateTime, string dateFormatCurrent = "yyyy-MM-dd HH:mm:ss", string dateFormatNew = "yyyyMMddhhmmss")
        {
            return DateTime.ParseExact(dateTime, dateFormatCurrent, null).ToFormatString(dateFormatNew);
        }

        /// <summary>
        /// 格式化时间格式 yyyy-MM-dd hh:mm:ss
        /// </summary>
        /// <returns></returns>
        public static string ToFormatString(this DateTime date, string format = "yyyy-MM-dd hh:mm:ss")
        {
            return date.ToString(format);
        }

        /// <summary>
        /// Object To Json
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd hh:mm:ss" };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }

        /// <summary>
        /// Json To Object
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string json) where T : class
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
