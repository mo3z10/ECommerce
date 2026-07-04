using ECommerce.BIL.DTOS.PaymentDtos;
using ECommerce.BIL.Services.PaymentServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> Create(CreatePaymentDto dto)
    {
        var payment =
            await _paymentService.CreatePaymentIntentAsync(dto);

        return Ok(payment);
    }

    [HttpGet("{paymentIntentId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Status(string paymentIntentId)
    {
        var status =
            await _paymentService.GetPaymentStatus(paymentIntentId);

        return Ok(status);
    }

    [HttpPost("cancel")]
    [Authorize]
    public async Task<IActionResult> Cancel(CancelPaymentDto dto)
    {
        await _paymentService.CancelPayment(dto);

        return Ok("Payment cancelled.");
    }

    [AllowAnonymous]
    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook()
    {
        var valid =
            await _paymentService.VerifyWebhookAsync(Request);

        if (!valid)
            return BadRequest();

        return Ok();
    }
}