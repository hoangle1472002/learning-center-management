using LearningCenter.Application.DTOs.Payment;
using LearningCenter.Application.Handlers.Payment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningCenter.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PaymentListResponse>>> GetPayments(
        [FromQuery] string? status = null,
        [FromQuery] int? studentId = null,
        [FromQuery] int? classId = null)
    {
        var query = new GetAllPaymentsQuery
        {
            Status = status,
            StudentId = studentId,
            ClassId = classId
        };

        var payments = await _mediator.Send(query);
        return Ok(payments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PaymentListResponse>> GetPayment(int id)
    {
        // Implementation for getting single payment
        return Ok();
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult<PaymentListResponse>> CreatePayment(CreatePaymentRequest request)
    {
        var command = new CreatePaymentCommand { Request = request };
        var payment = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetPayment), new { id = payment.Id }, payment);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult<PaymentListResponse>> UpdatePayment(int id, UpdatePaymentRequest request)
    {
        // Implementation for updating payment
        return Ok();
    }

    [HttpPost("{id}/process")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult<PaymentListResponse>> ProcessPayment(int id, ProcessPaymentRequest request)
    {
        var command = new ProcessPaymentCommand
        {
            PaymentId = id,
            Request = request
        };

        var payment = await _mediator.Send(command);
        return Ok(payment);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeletePayment(int id)
    {
        // Implementation for deleting payment
        return NoContent();
    }

    [HttpGet("student/{studentId}")]
    public async Task<ActionResult<IEnumerable<PaymentListResponse>>> GetStudentPayments(int studentId)
    {
        var query = new GetAllPaymentsQuery { StudentId = studentId };
        var payments = await _mediator.Send(query);
        return Ok(payments);
    }

    [HttpGet("overdue")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult<IEnumerable<PaymentListResponse>>> GetOverduePayments()
    {
        var query = new GetAllPaymentsQuery { Status = "Overdue" };
        var payments = await _mediator.Send(query);
        return Ok(payments);
    }
}
