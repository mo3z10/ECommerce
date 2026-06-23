using ECommerce.BIL.Services.JobSercvices;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

[Route("api/jobs")]
[ApiController]
public class JobsController : ControllerBase
{

    private readonly IJobService _jobService;

    public JobsController(IJobService jobService)
    {
        _jobService = jobService;
    }


    [HttpPost("cleanup-cart")]
    public IActionResult CleanupCart()
    {
        BackgroundJob.Enqueue<IJobService>(
            x => x.CleanupCarts()
        );

        return Ok("Cleanup job started");
    }



    [HttpPost("low-stock")]
    public IActionResult LowStock()
    {
        BackgroundJob.Enqueue<IJobService>(
            x => x.LowStockMail()
        );

        return Ok("Low stock job started");
    }

}