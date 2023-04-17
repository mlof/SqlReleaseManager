using MediatR;
using SqlReleaseManager.Web.Models;

namespace SqlReleaseManager.Web.Controllers.Mediatr;

public record GetDatabaseViewModel(int Id, string DatabaseName) : IRequest<DatabaseViewModel>;