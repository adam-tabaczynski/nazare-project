using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Weather;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Application.Weather.WeatherModel;

namespace API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  [AllowAnonymous]
  public class WeatherController : BaseController
  {
     
    [HttpGet("{id}")]
    public async Task<ActionResult<WeatherDto>> GetWeather(Guid Id)
    {
      return await Mediator.Send( new GetWeather.Query { Id = Id });
    }
    
    [HttpGet("spot/{id}")]
    public async Task<ActionResult<Spot>> GetSpot(Guid Id)
    {
      return await Mediator.Send(new GetSpot.Query { Id = Id });
    }

    [HttpGet]
    public async Task<ActionResult<List<Spot>>> GetAllSpots()
    {
      return await Mediator.Send(new GetAllSpots.Query());
    }
  }
}