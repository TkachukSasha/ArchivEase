using Microsoft.AspNetCore.Mvc;
using SharedKernel.Dispatchers;

namespace Api.Controllers.Base;

[ApiController]
public class BaseController : ControllerBase
{
    protected readonly IDispatcher _dispatcher;

    public BaseController(IDispatcher dispatcher) => _dispatcher = dispatcher;
}