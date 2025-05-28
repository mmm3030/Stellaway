using Mapster;
using Microsoft.AspNetCore.Mvc;
using Stellaway.Common.Exceptions;
using Stellaway.Common.Helpers;
using Stellaway.Domain.Entities;
using Stellaway.Domain.Enums;
using Stellaway.DTOs;
using Stellaway.DTOs.Payments;
using Stellaway.Repositories;
using Stellaway.Services;

namespace Stellaway.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PaymentsController(
    IUnitOfWork unitOfWork,
    IMomoPaymentService momoPaymentService,
    IVnPayPaymentService vnPayPaymentService) : ControllerBase
{
    private readonly IGenericRepository<Booking> _bookingRepository = unitOfWork.Repository<Booking>();

    [HttpGet("callback/momo")]
    public async Task<IActionResult> MomoPaymentCallback(
    [FromQuery] MomoPaymentCallbackCommand request,
    CancellationToken cancellationToken)
    {
        var transId = request.OrderId.ConvertToGuid();

        var transaction = await _bookingRepository
            .FindByAsync(_ => _.Id == transId, cancellationToken: cancellationToken);

        if (transaction == null)
        {
            throw new NotFoundException(nameof(Booking), transId);
        }

        if (request.IsSuccess)
        {
            transaction.Status = BookingStatus.Completed;
        }
        else
        {
            transaction.Status = BookingStatus.Failed;
        }

        await unitOfWork.CommitAsync(cancellationToken);

        //return Redirect($"{callback.returnUrl}{HttpUtility.UrlEncode(_httpContextAccessor?.HttpContext?.Request.QueryString.Value)}");
        //return Redirect($"{callback.returnUrl}{HttpUtility.UrlEncode($"?isSuccess={callback.IsSuccess}")}");
        return Redirect($"{request.returnUrl}?isSuccess={request.IsSuccess}");
    }

    [HttpGet("callback/vnpay")]
    public async Task<IActionResult> VnPayPaymentCallback(
        [FromQuery] VnPayPaymentCallbackCommand request,
        CancellationToken cancellationToken)
    {

        var transId = request.vnp_TxnRef?.ConvertToGuid();

        var transaction = await _bookingRepository
            .FindByAsync(_ => _.Id == transId, cancellationToken: cancellationToken);

        if (transaction == null)
        {
            throw new NotFoundException(nameof(Booking), transId);
        }

        if (request.IsSuccess)
        {
            transaction.Status = BookingStatus.Completed;
        }
        else
        {
            transaction.Status = BookingStatus.Failed;
        }

        await unitOfWork.CommitAsync(cancellationToken);

        //return Redirect($"{callback.returnUrl}{HttpUtility.UrlEncode(_httpContextAccessor?.HttpContext?.Request.QueryString.Value)}");
        //return Redirect($"{callback.returnUrl}{HttpUtility.UrlEncode($"?isSuccess={callback.IsSuccess}")}");
        return Redirect($"{request.returnUrl}?isSuccess={request.IsSuccess}");
    }

    [HttpPost("pay")]
    public async Task<IActionResult> Pay(
    CreateBookingCommand request,
    string returnUrl,
    CancellationToken cancellationToken)
    {
        var booking = request.Adapt<Booking>();
        booking.Status = BookingStatus.Completed;

        await _bookingRepository.CreateAsync(booking, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return Ok(request.Method switch
        {
            BookingMethod.Momo => await MomoPaymentServiceHandler(booking, returnUrl),
            BookingMethod.VnPay => await VnPayPaymentServiceHandler(booking, returnUrl),
            _ => throw new ArgumentOutOfRangeException()
        });
    }

    private async Task<string> MomoPaymentServiceHandler(Booking transaction, string returnUrl)
    {
        return await momoPaymentService.CreatePaymentAsync(new MomoPayment
        {
            Amount = (long)transaction.TotalPrice,
            Info = "Thanh toán với momo",
            PaymentReferenceId = transaction.Id.ToString(),
            returnUrl = returnUrl
        });
    }

    private async Task<string> VnPayPaymentServiceHandler(Booking transaction, string returnUrl)
    {
        return await vnPayPaymentService.CreatePaymentAsync(new VnPayPayment
        {
            Amount = (long)transaction.TotalPrice,
            Info = "Thanh toán với vnpay",
            PaymentReferenceId = transaction.Id.ToString(),
            OrderType = TransactionType.Deposit,
            Time = transaction.CreatedAt.Value,
            returnUrl = returnUrl
        });
    }
}
