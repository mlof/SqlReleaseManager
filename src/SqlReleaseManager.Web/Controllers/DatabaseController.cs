using MediatR;
using Microsoft.AspNetCore.Mvc;
using SqlReleaseManager.Core.Persistence;
using SqlReleaseManager.Web.Controllers.Mediatr;

namespace SqlReleaseManager.Web.Controllers;

public class DatabaseController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMediator _mediator;

    public DatabaseController(ApplicationDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }


    [HttpGet]
    public async Task<IActionResult> Detail(int serverId, string databaseName)
    {
        var vm = await _mediator.Send(new GetDatabaseViewModel(serverId, databaseName));

        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Update(int serverId, string databaseName)
    {
        var vm = await _mediator.Send(new GetDatabaseViewModel(serverId, databaseName));
        return Ok(vm);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateDatabaseConfiguration vm)
    {
        await _mediator.Send(vm);

        return Ok();
    }


    [HttpPost]
    public async Task<IActionResult> Deploy(int serverId, string databaseName)
    {
        await _mediator.Send(new DeployDatabase(serverId, databaseName));
        return Ok();
    }
}