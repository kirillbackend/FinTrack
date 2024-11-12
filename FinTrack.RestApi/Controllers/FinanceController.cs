using FinTrack.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FinanceController : AbstractController
    {
        private readonly IFinanceService _financeService;

        public FinanceController(ILogger<FinanceController> logger, IFinanceService financeService)
            : base(logger)
        {
            _financeService = financeService;
        }

    }
}
