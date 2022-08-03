using System;
using PrettyNotify.Models;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;

namespace PrettyNotify
{
    public class NotifyTask
    {
        /// <summary>
        /// 启动入口
        /// </summary>
        public static void Run()
        {
            const string logName = "Run";
            LogFileHelper.WriteLog(logName, "===========开始发送消息===========");

            var nowTime = DateTime.Now;
            switch (nowTime.Hour)
            {
                case 10:
                    LogFileHelper.WriteLog(logName, "早安信息");
                    GoodMorning(nowTime);
                    break;
                case 22:
                    LogFileHelper.WriteLog(logName, "晚安信息");
                    GoodEvening(nowTime);
                    break;
            }

            LogFileHelper.WriteLog(logName, "===========发送消息完成===========");
        }

        /// <summary>
        /// 早安消息
        /// </summary>
        private static void GoodMorning(DateTime nowTime)
        {
            var nowTimeStr = $"{GetTodayStr(nowTime)}";
            var firstMeetingDay = GetFirstMeetingDay();
            var girlBirth = GetBirthday(Configs.GirlBirth.ToDateTime());
            var boyBirth = GetBirthday(Configs.BoyBirth.ToDateTime());
            var annDay = GetAnniversaryDay();
            var weatherAll = GetWeather<WeatherAll>("all");
            var weatherAllFirst = weatherAll.Forecasts[0].Casts[0];
            var weatherBase = GetWeather<WeatherBase>("base");
            var weatherBaseFirst = weatherBase.Lives[0];
            var hitokoto = GetHitokoto();

            foreach (var touser in Configs.ToUser)
            {
                var body = $"{{\"touser\": \"{touser}\",\"template_id\": \"{Configs.MorningTemplateId}\",\"url\": \"\",\"topcolor\": \"#FF0000\",\"data\": {{\"NowTime\": {{\"value\": \"{nowTimeStr}\",\"color\": \"#173177\"}},\"MeetingDay\": {{\"value\": \"{firstMeetingDay}\",\"color\": \"#173177\"}},\"GirlBirth\": {{\"value\": \"{girlBirth}\",\"color\": \"#173177\"}},\"BoyBirth\": {{\"value\": \"{boyBirth}\",\"color\": \"#173177\"}},\"AnnDay\": {{\"value\": \"{annDay}\",\"color\": \"#173177\"}},\"Weather\": {{\"value\": \"{weatherAllFirst.DayWeather}\",\"color\": \"#173177\"}},\"LowestTemp\": {{\"value\": \"{weatherAllFirst.NightTemp}\",\"color\": \"#173177\"}},\"Highest\": {{\"value\": \"{weatherAllFirst.DayTemp}\",\"color\": \"#173177\"}},\"CurrentTemp\": {{\"value\": \"{weatherBaseFirst.Temperature}\",\"color\": \"#173177\"}},\"Humidity\": {{\"value\": \"{weatherBaseFirst.Humidity}\",\"color\": \"#173177\"}},\"Winddirection\": {{\"value\": \"{weatherBaseFirst.WindDirection}\",\"color\": \"#173177\"}},\"WindPower\": {{\"value\": \"{weatherBaseFirst.WindPower}\",\"color\": \"#173177\"}},\"Hitokoto\": {{\"value\": \"{hitokoto.Hitokoto}\",\"color\": \"#173177\"}}}}}}";

                var tokenUrl = $"{Configs.GetToken}?grant_type=client_credential&appid={Configs.AppId}&secret={Configs.AppsSecret}";
                var token = HttpHelper.HttpGet(tokenUrl).ToObject<AccessToken>();
                HttpHelper.HttpPost($"{Configs.SendMsg}?access_token={token.Access_Token}", body);
            }
        }

        /// <summary>
        /// 晚安消息
        /// </summary>
        private static void GoodEvening(DateTime nowTime)
        {
            var nowTimeStr = $"{GetTodayStr(nowTime)}";
            var startDay = GetStartDay(nowTime);
            var balanceTime = GetBalanceTime(nowTime);
            var getUpTime = GetGetUpTime(nowTime);
            var weatherBase = GetWeather<WeatherBase>("base");
            var weatherBaseFirst = weatherBase.Lives[0];
            var hitokoto = GetHitokoto();
            foreach (var touser in Configs.ToUser)
            {
                var body = $"{{\"touser\": \"{touser}\",\"template_id\": \"{Configs.EveningTemplateId}\",\"url\": \"\",\"topcolor\": \"#FF0000\",\"data\": {{\"NowTime\": {{\"value\": \"{nowTimeStr}\",\"color\": \"#173177\"}},\"StartDay\": {{\"value\": \"{startDay}\",\"color\": \"#173177\"}},\"BalanceHour\": {{\"value\": \"{balanceTime[0]}\",\"color\": \"#173177\"}},\"BalanceMinute\": {{\"value\": \"{balanceTime[1]}\",\"color\": \"#173177\"}},\"GetUpHour\": {{\"value\": \"{getUpTime[0]}\",\"color\": \"#173177\"}},\"GetUpMinute\": {{\"value\": \"{getUpTime[1]}\",\"color\": \"#173177\"}},\"Weather\": {{\"value\": \"{weatherBaseFirst.Weather}\",\"color\": \"#173177\"}},\"CurrentTemp\": {{\"value\": \"{weatherBaseFirst.Temperature}\",\"color\": \"#173177\"}},\"Humidity\": {{\"value\": \"{weatherBaseFirst.Humidity}\",\"color\": \"#173177\"}},\"Winddirection\": {{\"value\": \"{weatherBaseFirst.WindDirection}\",\"color\": \"#173177\"}},\"WindPower\": {{\"value\": \"{weatherBaseFirst.WindPower}\",\"color\": \"#173177\"}},\"Hitokoto\": {{\"value\": \"{hitokoto.Hitokoto}\",\"color\": \"#173177\"}}}}}}";

                var tokenUrl = $"{Configs.GetToken}?grant_type=client_credential&appid={Configs.AppId}&secret={Configs.AppsSecret}";
                var token = HttpHelper.HttpGet(tokenUrl).ToObject<AccessToken>();
                HttpHelper.HttpPost($"{Configs.SendMsg}?access_token={token.Access_Token}", body);
            }
        }

        /// <summary>
        /// 获取当天时间
        /// </summary>
        /// <param name="nowTime"></param>
        /// <returns></returns>
        private static string GetTodayStr(DateTime nowTime)
        {
            return $"{nowTime.Year}年{nowTime.Month}月{nowTime.Day}日 星期{GetWeekDay((int)nowTime.DayOfWeek)}";
        }

        /// <summary>
        /// 获取天气信息
        /// </summary>
        private static T GetWeather<T>(string type) where T : class
        {
            var url = $"{Configs.WeatherApi}?key={Configs.WeatherKey}&city={Configs.CityCode}&extensions={type}";
            var weatherStr = HttpHelper.HttpGet(url);
            return weatherStr.ToObject<T>();
        }

        /// <summary>
        /// 获取每日一句
        /// </summary>
        private static HitokotoEntity GetHitokoto()
        {
            var hitStr = HttpHelper.HttpGet(Configs.Hitokoto);
            return hitStr.ToObject<HitokotoEntity>();
        }

        /// <summary>
        /// 获取纪念日
        /// </summary>
        private static int GetAnniversaryDay()
        {
            var annDay = Configs.AnniversaryDay.ToDateTime();
            var nowTime = DateTime.Now.Date;
            var currentAnnDay = new DateTime(nowTime.Year, annDay.Month, annDay.Day);
            return currentAnnDay >= nowTime
                ? (currentAnnDay - nowTime).Days
                : (currentAnnDay.AddYears(1) - nowTime).Days;
        }

        /// <summary>
        /// 获取相遇日
        /// </summary>
        private static int GetFirstMeetingDay()
        {
            var firstMeetingDay = Configs.FirstMeetingDay.ToDateTime();
            var nowTime = DateTime.Now.Date;
            return (nowTime - firstMeetingDay).Days;
        }

        /// <summary>
        /// 获取生日
        /// </summary>
        /// <param name="birthday"></param>
        /// <returns></returns>
        private static int GetBirthday(DateTime birthday)
        {
            var chineseCalendar = new ChineseLunisolarCalendar();
            var nowTime = DateTime.Now;
            var year = chineseCalendar.GetYear(nowTime);
            var month = chineseCalendar.GetMonth(nowTime);
            var day = chineseCalendar.GetDayOfMonth(nowTime);
            var chineseNowTime = new DateTime(year, month, day);
            var chineseBirthday = new DateTime(year, birthday.Month, birthday.Day);
            return chineseBirthday >= chineseNowTime
                ? (chineseBirthday - chineseNowTime).Days
                : (chineseBirthday.AddYears(1) - chineseNowTime).Days;

        }

        /// <summary>
        /// 获取运行天数
        /// </summary>
        /// <param name="nowTime"></param>
        /// <returns></returns>
        private static int GetStartDay(DateTime nowTime)
        {
            var startDay = Configs.StartTime.ToDateTime().Date;
            return (nowTime.Date - startDay).Days;
        }

        /// <summary>
        /// 获取剩余睡眠时长
        /// </summary>
        /// <param name="nowTime"></param>
        /// <returns></returns>
        private static string[] GetGetUpTime(DateTime nowTime)
        {
            var getUpTime = Configs.GetUpTime;
            var getUpDateTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day + 1, getUpTime[0].ToInt(8), getUpTime[1].ToInt(), 0);
            var totalHours = (getUpDateTime - nowTime).TotalHours.ToString("F").Split('.');
            var getUpHour = totalHours[0];
            var getUpMinute = (totalHours[1].ToInt() / 100m * 60).ToInt();
            return new[] { getUpHour, getUpMinute.ToString(CultureInfo.InvariantCulture) };
        }

        /// <summary>
        /// 获取当天倒计时
        /// </summary>
        /// <param name="nowTime"></param>
        /// <returns></returns>
        private static string[] GetBalanceTime(DateTime nowTime)
        {
            var zeroTime = DateTime.Now.Date.AddDays(1);
            var totalHours = (zeroTime - nowTime).TotalHours.ToString("F").Split('.');
            var balanceHour = totalHours[0];
            var balanceMinute = (totalHours[1].ToInt() / 100m * 60).ToInt();
            return new[] { balanceHour, balanceMinute.ToString(CultureInfo.InvariantCulture) };
        }

        /// <summary>
        /// 获取周几
        /// </summary>
        /// <param name="weekDay"></param>
        /// <returns></returns>
        private static string GetWeekDay(int weekDay)
        {
            var weekDic = new Dictionary<int, string>
            {
                { 0, "日" },
                { 1, "一" },
                { 2, "二" },
                { 3, "三" },
                { 4, "四" },
                { 5, "五" },
                { 6, "六" }
            };

            var weekDayDic = weekDic.FirstOrDefault(p => p.Key.Equals(weekDay));
            return default(KeyValuePair<int, string>).Equals(weekDayDic) ? weekDayDic.Value : "八";
        }
    }
}
