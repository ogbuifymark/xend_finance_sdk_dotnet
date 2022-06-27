using System;

namespace SdkClientWeb
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
    public class RequestDto
    {
        public int chainId { get; set; }
        public string tokenName { get; set; }
        public decimal? amount { get; set; }
    }
}
