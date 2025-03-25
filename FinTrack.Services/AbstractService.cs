using FinTrack.Data.Contracts;
using FinTrack.Services.Mappers.Contracts;
using Microsoft.Extensions.Logging;

namespace FinTrack.Services
{
    public abstract class AbstractService
    {
        public ILogger Logger { get; }
        public IMapperFactory MapperFactory { get; }
        public IDataContextManager DataContextManager { get; }

        public AbstractService(ILogger logger, IMapperFactory mapperFactory, IDataContextManager dataContextManager)
        {
            Logger = logger;
            MapperFactory = mapperFactory;
            DataContextManager = dataContextManager;
        }
    }
}
