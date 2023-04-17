using MediatR;
using SqlReleaseManager.Web.Models;

namespace SqlReleaseManager.Web.Controllers.Mediatr;

public record GetSqlServerViewModel(int Id) : IRequest<ServerViewModel>;