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

          client.BaseAddress = new Uri("http://api.worldweatheronline.com/premium/v1/marine.ashx");
          var response = await client.GetAsync($"?key=c5379ceb82b5463b849145737210605&format=json&q={latitude},{longitude}");
          response.EnsureSuccessStatusCode();

          var stringResult = await response.Content.ReadAsStringAsync();
          RootWWO rawWeather = JsonConvert.DeserializeObject<RootWWO>(stringResult);

          weatherResponse.AirTemperature = rawWeather.data.weather[0].hourly[5].tempC;
          weatherResponse.WaterTemperature = rawWeather.data.weather[0].hourly[5].waterTemp_C;
          weatherResponse.WindSpeed = rawWeather.data.weather[0].hourly[5].windspeedKmph;
          weatherResponse.Cloudiness = rawWeather.data.weather[0].hourly[5].cloudcover;
          weatherResponse.TideHeight = rawWeather.data.weather[0].hourly[5].sigHeight_m;
          weatherResponse.WindAngle = rawWeather.data.weather[0].hourly[5].winddir16Point;
        }


        return weatherResponse;
      }
    }
  }
}