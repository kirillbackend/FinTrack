using FinTrack.Data.Contracts;
using FinTrack.Localization;
using FinTrack.Services.Context;
using FinTrack.Services.Mappers.Contracts;
using Microsoft.Extensions.Logging;

namespace FinTrack.Services.Facades
{
    public abstract class AbstractFacade : AbstractService
    {
        protected AbstractFacade(ILogger logger, IMapperFactory mapperFactory, IDataContextManager dataContextManager
            , LocalizationContextLocator localizationContext, ContextLocator contextLocator) 
            : base(logger, mapperFactory, dataContextManager, localizationContext, contextLocator)
        {
        }
    }
}
