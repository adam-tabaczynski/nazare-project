using System.Security.Principal;
using System.Reflection.Metadata;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using Persistence;
using static Application.Weather.WeatherModelOWA;
using static Application.Weather.WeatherModelWWO;

namespace Application.Weather
{
  public class GetWeather
  {
    public class Query : IRequest<WeatherDto> { public Guid Id { get; set; }}

    public class Handler : IRequestHandler<Query, WeatherDto>
    {
      private readonly DataContext _context;
      public Handler(DataContext context)
      {
        _context = context;
      }
      public async Task<WeatherDto> Handle(Query request, CancellationToken cancellationToken)
      {
        var spot = await _context.Spots.FindAsync(request.Id);

        var latitude = spot.Latitude;
        var longitude = spot.Longitude;

        WeatherDto weatherResponse = new WeatherDto();

        using (var client = new HttpClient())
        {

          client.BaseAddress = new Uri("http://api.openweathermap.org");
          var response = await client.GetAsync($"/data/2.5/weather?lat={latitude}&lon={longitude}&appid=97b8299dbeb8ec978fa2de2314c76e24&units=metric");
          response.EnsureSuccessStatusCode();

          var stringResult = await response.Content.ReadAsStringAsync();
          RootOWA rawWeather = JsonConvert.DeserializeObject<RootOWA>(stringResult);

          weatherResponse.AirTemperature = rawWeather.main.temp;
          weatherResponse.WindSpeed = rawWeather.wind.speed;
          weatherResponse.Cloudiness = rawWeather.clouds.all;
        }
        using (var client = new HttpClient())
        {

          client.BaseAddress = new Uri("http://api.worldweatheronline.com/premium/v1/marine.ashx");
          var response = await client.GetAsync($"?key=78b44e53d6a24d13ba895644210401&format=json&q={latitude},{longitude}&tide=yes");
          response.EnsureSuccessStatusCode();

          var stringResult = await response.Content.ReadAsStringAsync();
          RootWWO rawWeather = JsonConvert.DeserializeObject<RootWWO>(stringResult);

          weatherResponse.WaterTemperature = rawWeather.data.weather[0].hourly[5].waterTemp_C;
          weatherResponse.TideHeight = rawWeather.data.weather[0].tides[0].tide_data[2].tideHeight_mt;
          weatherResponse.WindAngle = rawWeather.data.weather[0].hourly[5].winddir16Point;
        }


        return weatherResponse;
      }
    }
  }
}