using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel.Dal;

namespace Core.Dal.Initializers.Internal;

internal abstract class BaseInitializer<TContext, TInitializer>
    where TContext : DbContext
    where TInitializer : class, IDataInitializer
{
    protected readonly TContext _context;
    protected readonly ILogger<TInitializer> _logger;

    protected BaseInitializer(TContext context, ILogger<TInitializer> logger)
       => (_context, _logger) = (context, logger);
}