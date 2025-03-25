using FinTrack.Data.Contracts;
using FinTrack.Services.Context;
using FinTrack.Services.Mappers.Contracts;
using Microsoft.Extensions.Logging;

namespace FinTrack.Services.Facades
{
    public abstract class AbstractFacade : AbstractService
    {
        protected AbstractFacade(ILogger logger, IMapperFactory mapperFactory, IDataContextManager dataContextManager) 
            : base(logger, mapperFactory, dataContextManager)
        {
        }
    }
}
