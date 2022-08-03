using System.Configuration;

namespace PrettyNotify
{
    public static class Configs
    {
        //Settings
        public static string GetToken = GetAppSetting("getToken");
        public static string SendMsg = GetAppSetting("sendMsg");
        public static string WeatherApi = GetAppSetting("weatherApi");
        public static string CityCode = GetAppSetting("cityCode");
        public static string Hitokoto = GetAppSetting("hitokoto");
        public static string MorningTemplateId = GetAppSetting("morningTemplateId");
        public static string EveningTemplateId = GetAppSetting("eveningTemplateId");
        public static string WeatherKey = GetAppSetting("weatherKey");
        public static string AppId = GetAppSetting("appId");
        public static string AppsSecret = GetAppSetting("appsSecret");
        public static string[] ToUser = GetAppSetting("toUser").Split(',');

        //Time
        public static string BoyBirth = GetAppSetting("boyBirth");
        public static string GirlBirth = GetAppSetting("girlBirth");
        public static string[] GetUpTime = GetAppSetting("getUpTime").Split(':');

        //Ours
        public static string AnniversaryDay = GetAppSetting("anniversaryDay");
        public static string FirstMeetingDay = GetAppSetting("firstMeetingDay");
        public static string SalaryDay = GetAppSetting("salaryDay");
        public static string StartTime = GetAppSetting("startTime");

        /// <summary>
        /// 获取appSettings 配置文件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
