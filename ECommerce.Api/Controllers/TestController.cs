using ECommerce.BIL.Services.NotificationHubService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        private readonly INotificationService _notification;

        public TestController(INotificationService notification)
        {
            _notification = notification;
        }

        [HttpPost]
        public async Task<IActionResult> Test()
        {
            await _notification.NewOrderCreated(123);

            return Ok();
        }
    }
}
