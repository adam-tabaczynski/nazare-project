namespace Application.Weather
{
  public class WeatherDto
  {
    public double AirTemperature { get; set; }
    public string WaterTemperature { get; set; }
    public double WindSpeed { get; set; }
    public string WindAngle { get; set; }
    public double Cloudiness { get; set; }
    public string TideHeight { get; set; }
  }
}