using FinTrack.Data.Contracts;
using FinTrack.Services.Context;
using FinTrack.Services.Context.Contracts;
using FinTrack.Services.Mappers.Contracts;
using Microsoft.Extensions.Logging;

namespace FinTrack.Services
{
    public abstract class AbstractService
    {
        public ILogger Logger { get; }

        public IMapperFactory MapperFactory { get; }

        public IDataContextManager DataContextManager { get; }

        public Localization.LocalizationContextLocator LocalizationContext;

        protected IContextLocator ContextLocator { get; private set; }

        public AbstractService(ILogger logger, IMapperFactory mapperFactory, IDataContextManager dataContextManager
            , Localization.LocalizationContextLocator localizationContext, IContextLocator contextLocator)
        {
            Logger = logger;
            MapperFactory = mapperFactory;
            LocalizationContext = localizationContext;
            ContextLocator = contextLocator;
            DataContextManager = dataContextManager;
        }

        protected UserContext UserContext
        {
            get
            {
                return ContextLocator.Get<UserContext>();
            }
        }
    }
}
