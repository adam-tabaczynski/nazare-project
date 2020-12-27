using System.Collections.Generic;
using Domain;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Activities;
using System;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
  public class ActivitiesController : BaseController
  {
    [HttpGet]
    public async Task<ActionResult<List.ActivitiesEnvelope>> List(int? limit, int? offset,
      bool isGoing, bool isHost, DateTime? startDate)
    {
      // for that List I used 'using Application.Activities'
      //
      return await Mediator.Send(new List.Query(limit, offset, isGoing, isHost, startDate));
    }

    [HttpGet("{id}")]
    // Any request that comes to that method has to be authorized via JWT.
    [Authorize]
    public async Task<ActionResult<ActivityDto>> Details(Guid id)
    {
      // These curly braces statement assures that Query can get access to id passed in URL.
      return await Mediator.Send(new Details.Query { Id = id });
    }

    [HttpPost]
    // Argument Create.Command is what I will receive in the body of a request.
    // New activity will be sent up in body of the request. Thanks to ApiController tag,
    // ApiController know that the searched values is in body. W/o it, I would have to put
    // [FromBody] tag before Create.Command.
    public async Task<ActionResult<Unit>> Create(Create.Command command)
    {
      return await Mediator.Send(command);
    }

    // This policy tag is added specially for my custom policy.
    [HttpPut("{id}")]
    [Authorize(Policy = "IsActivityHost")]
    // Passing the ID cause I need to specify which Activity I want to change.
    public async Task<ActionResult<Unit>> Edit(Guid id, Edit.Command command)
    {
      command.Id = id;
      return await Mediator.Send(command);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "IsActivityHost")]
    public async Task<ActionResult<Unit>> Delete(Guid id)
    {
      return await Mediator.Send(new Delete.Command{ Id = id });
    }

    [HttpPost("{id}/attend")]
    public async Task<ActionResult<Unit>> Attend(Guid id)
    {
      return await Mediator.Send(new Attend.Command{Id = id});
    }

    [HttpDelete("{id}/attend")]
    public async Task<ActionResult<Unit>> Unattend(Guid id)
    {
      return await Mediator.Send(new Unattend.Command{Id = id});
    }
  }
}