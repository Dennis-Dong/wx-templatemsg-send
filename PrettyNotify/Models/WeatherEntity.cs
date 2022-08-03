namespace PrettyNotify.Models
{
    /// <summary>
    /// 天气预报
    /// </summary>
    public class WeatherAll
    {
        public string Status { get; set; }
        public string Count { get; set; }
        public string Info { get; set; }
        public string InfoCode { get; set; }
        public Forecast[] Forecasts { get; set; }
    }

    public class Forecast
    {
        public string City { get; set; }
        public string AdCode { get; set; }
        public string Province { get; set; }
        public string ReportTime { get; set; }
        public Cast[] Casts { get; set; }
    }

    public class Cast
    {
        public string Date { get; set; }
        public string Week { get; set; }
        public string DayWeather { get; set; }
        public string NightWeather { get; set; }
        public string DayTemp { get; set; }
        public string NightTemp { get; set; }
        public string DayWind { get; set; }
        public string NightWind { get; set; }
        public string DayPower { get; set; }
        public string NightPower { get; set; }
    }

    /// <summary>
    /// 实时天气
    /// </summary>
    public class WeatherBase
    {
        public string Status { get; set; }
        public string Count { get; set; }
        public string Info { get; set; }
        public string InfoCode { get; set; }
        public Life[] Lives { get; set; }
    }

    public class Life
    {
        public string Province { get; set; }
        public string City { get; set; }
        public string AdCode { get; set; }
        public string Weather { get; set; }
        public string Temperature { get; set; }
        public string WindDirection { get; set; }
        public string WindPower { get; set; }
        public string Humidity { get; set; }
        public string ReportTime { get; set; }
    }

}
