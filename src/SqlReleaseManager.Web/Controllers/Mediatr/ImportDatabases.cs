using MediatR;

namespace SqlReleaseManager.Web.Controllers.Mediatr;

public record ImportDatabases(int ServerId) : IRequest;