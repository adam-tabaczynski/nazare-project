using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Application.Weather.WeatherResponse;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
  {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> City(string city)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("http://api.openweathermap.org");
                    var response = await client.GetAsync($"/data/2.5/weather?lat=-22.97&lon=-43.18&appid=97b8299dbeb8ec978fa2de2314c76e24&units=metric");
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    Root rawWeather = JsonConvert.DeserializeObject<Root>(stringResult);
                    return Ok(new {
                        Temp = rawWeather.main.temp + " Celsius",
                        Summary = string.Join(",", rawWeather.weather.Select(x=>x.main)),
                        City = rawWeather.name,
                        WindSpeed = rawWeather.wind.speed + "km/h"

                    });
                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
                }
            }
        }
    
    }
}