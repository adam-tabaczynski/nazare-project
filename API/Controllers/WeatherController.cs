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
  [Route("[controller]")]
  [AllowAnonymous]
  public class WeatherController : BaseController
  {
     
    [HttpGet("{id}")]
    public async Task<ActionResult<WeatherDto>> GetOneFromOWA(Guid Id)
    {
      return await Mediator.Send( new GetOneFromOWA.Query { Id = Id });
    }
    
    [HttpGet("/one/{id}")]
    public async Task<ActionResult<Spot>> GetOne(Guid Id)
    {
      return await Mediator.Send(new GetOne.Query { Id = Id });
    }

    [HttpGet]
    public async Task<ActionResult<List<Spot>>> GetAll()
    {
      return await Mediator.Send(new GetAll.Query());
    }
  }
}