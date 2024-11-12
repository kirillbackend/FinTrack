using FinTrack.Data.Contracts;
using FinTrack.Localization;
using FinTrack.Services.Mappers.Contracts;
using Microsoft.Extensions.Logging;

namespace FinTrack.Services
{
    public abstract class AbstractService
    {
        public ILogger Logger { get; }

        public IMapperFactory MapperFactory { get; }

        public IDataContextManager DataContextManager { get; }

        public  ContextLocator ContextLocator;

        public AbstractService(ILogger logger, IMapperFactory mapperFactory, IDataContextManager dataContextManager, ContextLocator contextLocator)
        {
            Logger = logger;
            MapperFactory = mapperFactory;
            ContextLocator = contextLocator;
            DataContextManager = dataContextManager;
        }
    }
}
