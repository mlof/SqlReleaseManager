using MediatR;

namespace SqlReleaseManager.Web.Controllers;

public record DeployDatabase(int ServerId, string DatabaseName) : IRequest
{
    public string? Package { get; set; }
}