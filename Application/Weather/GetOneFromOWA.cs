using System.Security.Principal;
using System.Reflection.Metadata;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using Persistence;
using static Application.Weather.WeatherModel;

namespace Application.Weather
{
  public class GetOneFromOWA
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

        using (var client = new HttpClient())
        {

          client.BaseAddress = new Uri("http://api.openweathermap.org");
          var response = await client.GetAsync($"/data/2.5/weather?lat={latitude}&lon={longitude}&appid=97b8299dbeb8ec978fa2de2314c76e24&units=metric");
          response.EnsureSuccessStatusCode();

          var stringResult = await response.Content.ReadAsStringAsync();
          Root rawWeather = JsonConvert.DeserializeObject<Root>(stringResult);
          var weatherResponse = new WeatherDto
          {
            AirTemperature = rawWeather.main.temp,
            WindSpeed = rawWeather.wind.speed,
            WindAngle = rawWeather.wind.deg,
            Cloudiness = rawWeather.clouds.all
          };

          return weatherResponse;
        }
        
      }
    }
  }
}