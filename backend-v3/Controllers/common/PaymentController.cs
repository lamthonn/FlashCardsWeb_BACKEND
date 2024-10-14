using backend_v3.Context;
using backend_v3.Dto;
using backend_v3.Models;
using backend_v3.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend_v3.Controllers.common
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly StripePaymentService _paymentService;
        private readonly AppDbContext _context;

        public PaymentController(StripePaymentService paymentService, AppDbContext context)
        {
            _paymentService = paymentService;
            _context = context;
        }

        [HttpPost("create-checkout-session")]
        public ActionResult CreateCheckoutSession(PaymentParam param)
        {
            var session = _paymentService.CreateCheckoutSession(param);

            // Record the session in your DB
            _context.paymentRecords.Add(new PaymentRecord
            {
                Id = Guid.NewGuid().ToString(),
                StripeSessionId = session.Id,
                Created = DateTime.UtcNow,
                Status = "Created",
                UserId = session.Id,
                
            });
            _context.SaveChanges();

            return Ok(new { sessionId = session.Id });
        }

        [HttpGet("success")]
        public async Task<IActionResult> Success(string sessionId)
        {
            var paymentRecord = await _context.paymentRecords.FirstOrDefaultAsync(p => p.StripeSessionId == sessionId);
            if (paymentRecord != null)
            {
                paymentRecord.Status = "Success";
                await _context.SaveChangesAsync();
            }

            // Redirect to a success page or return success response
            return Ok("Payment successful.");
        }

        [HttpGet("cancel")]
        public async Task<IActionResult> Cancel(string sessionId)
        {
            var paymentRecord = await _context.paymentRecords.FirstOrDefaultAsync(p => p.StripeSessionId == sessionId);
            if (paymentRecord != null)
            {
                paymentRecord.Status = "Cancelled";
                await _context.SaveChangesAsync();
            }

            // Redirect to a cancel page or return cancel response
            return Ok("Payment cancelled.");
        }
    }
}

