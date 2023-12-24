using KanbanBoard.WebApi.Controllers.Filters;
using Microsoft.AspNetCore.Mvc;

namespace KanbanBoard.WebApi.Controllers.Base;

[ApiController]
[TypeFilter(typeof(DefaultRequestLog), Order = 1)]
public abstract class BaseController: ControllerBase
{
}