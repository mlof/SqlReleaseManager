using MediatR;
using Microsoft.AspNetCore.Mvc;
using SqlReleaseManager.Core.Domain;
using SqlReleaseManager.Core.Persistence;
using SqlReleaseManager.Web.Controllers.Mediatr;

namespace SqlReleaseManager.Web.Controllers;

public class ServerController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMediator _mediator;

    public ServerController(ApplicationDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Detail(int id)
    {
        var vm = await _mediator.Send(new GetSqlServerViewModel(id));


        return View(vm);
    }

    [HttpGet("server/{serverId}/database/{databaseName}")]
    public async Task<IActionResult> Database(int serverId, string databaseName)
    {
        return RedirectToAction("Detail", "Database", new { serverId, databaseName });
    }

    [HttpPost("server/{serverId}/commands/import")]
    public async Task ImportDatabases(int serverId)
    {
        await _mediator.Send(new ImportDatabases(serverId));
    }

    
}