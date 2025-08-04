using FinTrack.Services;
using Microsoft.Extensions.Caching.Distributed;
using Moq;

namespace FinTrack.Test
{
    public class CacheServiceTests
    {
        private readonly Mock<IDistributedCache> _cacheMock;
        private readonly CacheService _cacheService;

        public CacheServiceTests()
        {
            _cacheMock = new Mock<IDistributedCache>();
            _cacheService = new CacheService(_cacheMock.Object);
        }
    }
}
