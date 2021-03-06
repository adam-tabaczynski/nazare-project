using System.Collections.Generic;

namespace Application.Weather
{
    public class WeatherModelWWO
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Request    {
        public string type { get; set; } 
        public string query { get; set; } 
    }

    public class Astronomy    {
        public string sunrise { get; set; } 
        public string sunset { get; set; } 
        public string moonrise { get; set; } 
        public string moonset { get; set; } 
        public string moon_phase { get; set; } 
        public string moon_illumination { get; set; } 
    }

    public class TideData    {
        public string tideTime { get; set; } 
        public string tideHeight_mt { get; set; } 
        public string tideDateTime { get; set; } 
        public string tide_type { get; set; } 
    }

    public class Tide    {
        public List<TideData> tide_data { get; set; } 
    }

    public class WeatherIconUrl    {
        public string value { get; set; } 
    }

    public class WeatherDesc    {
        public string value { get; set; } 
    }

    public class Hourly    {
        public string time { get; set; } 
        public string tempC { get; set; } 
        public string tempF { get; set; } 
        public string windspeedMiles { get; set; } 
        public string windspeedKmph { get; set; } 
        public string winddirDegree { get; set; } 
        public string winddir16Point { get; set; } 
        public string weatherCode { get; set; } 
        public List<WeatherIconUrl> weatherIconUrl { get; set; } 
        public List<WeatherDesc> weatherDesc { get; set; } 
        public string precipMM { get; set; } 
        public string precipInches { get; set; } 
        public string humidity { get; set; } 
        public string visibility { get; set; } 
        public string visibilityMiles { get; set; } 
        public string pressure { get; set; } 
        public string pressureInches { get; set; } 
        public string cloudcover { get; set; } 
        public string HeatIndexC { get; set; } 
        public string HeatIndexF { get; set; } 
        public string DewPointC { get; set; } 
        public string DewPointF { get; set; } 
        public string WindChillC { get; set; } 
        public string WindChillF { get; set; } 
        public string WindGustMiles { get; set; } 
        public string WindGustKmph { get; set; } 
        public string FeelsLikeC { get; set; } 
        public string FeelsLikeF { get; set; } 
        public string sigHeight_m { get; set; } 
        public string swellHeight_m { get; set; } 
        public string swellHeight_ft { get; set; } 
        public string swellDir { get; set; } 
        public string swellDir16Point { get; set; } 
        public string swellPeriod_secs { get; set; } 
        public string waterTemp_C { get; set; } 
        public string waterTemp_F { get; set; } 
    }

    public class Weather    {
        public string date { get; set; } 
        public List<Astronomy> astronomy { get; set; } 
        public string maxtempC { get; set; } 
        public string maxtempF { get; set; } 
        public string mintempC { get; set; } 
        public string mintempF { get; set; } 
        public List<Tide> tides { get; set; } 
        public List<Hourly> hourly { get; set; } 
    }

    public class Data    {
        public List<Request> request { get; set; } 
        public List<Weather> weather { get; set; } 
    }

    public class RootWWO    {
        public Data data { get; set; } 
    }


    }
}