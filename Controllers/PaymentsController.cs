using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using Stellaway.Common.Exceptions;
using Stellaway.Common.Helpers;
using Stellaway.Domain.Entities;
using Stellaway.Domain.Entities.Identities;
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
    UserManager<User> userManager,
    IEmailSender emailSender,
    IVnPayPaymentService vnPayPaymentService) : ControllerBase
{
    private readonly IGenericRepository<Booking> _bookingRepository = unitOfWork.Repository<Booking>();
    private readonly IGenericRepository<Schedule> _scheduleRepository = unitOfWork.Repository<Schedule>();

    [HttpGet("callback/momo")]
    public async Task<IActionResult> MomoPaymentCallback(
    [FromQuery] MomoPaymentCallbackCommand request,
    CancellationToken cancellationToken)
    {
        var transId = request.OrderId.ConvertToGuid();

        var booking = await _bookingRepository
            .FindByAsync(_ => _.Id == transId,
            includeFunc: _ => _.Include(_ => _.User), cancellationToken: cancellationToken);

        if (booking == null)
        {
            throw new NotFoundException(nameof(Booking), transId);
        }

        if (request.IsSuccess)
        {
            booking.Status = BookingStatus.Completed;

            var content = await System.IO.File.ReadAllTextAsync("wwwroot/Templates/Email/mail.html");
            content = content
                .Replace("{0}", booking.User.FullName)
                .Replace("{1}", booking.Tickets.Count.ToString())
                .Replace("{2}", booking.TotalPrice.ToString())
                .Replace("{3}", booking.Status.ToString())
                .Replace("{4}", booking.CreatedAt.ToString());

            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode($"https://ticket-system-beta.vercel.app/admin/check-in?id={booking.Id}", QRCodeGenerator.ECCLevel.Q))
            using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
            {
                byte[] qrCodeImage = qrCode.GetGraphic(20);

                _ = emailSender.SendEmailAsync(
                       booking.User.Email!,
                       $"Your Qr Ticket",
                        content, qrCodeImage);
            }
        }
        else
        {
            booking.Status = BookingStatus.Failed;
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

        var booking = await _bookingRepository
            .FindByAsync(_ => _.Id == transId,
            includeFunc: _ => _.Include(_ => _.User), cancellationToken: cancellationToken);

        if (booking == null)
        {
            throw new NotFoundException(nameof(Booking), transId);
        }

        if (request.IsSuccess)
        {
            booking.Status = BookingStatus.Completed;

            var content = await System.IO.File.ReadAllTextAsync("wwwroot/Templates/Email/mail.html");
            content = content
                .Replace("{0}", booking.User.FullName)
                .Replace("{1}", booking.Tickets.Count.ToString())
                .Replace("{2}", booking.TotalPrice.ToString())
                .Replace("{3}", booking.Status.ToString())
                .Replace("{4}", booking.CreatedAt.ToString());

            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode($"https://ticket-system-beta.vercel.app/admin/check-in?id={booking.Id}", QRCodeGenerator.ECCLevel.Q))
            using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
            {
                byte[] qrCodeImage = qrCode.GetGraphic(20);

                _ = emailSender.SendEmailAsync(
                       booking.User.Email!,
                       $"Your Qr Ticket",
                        content, qrCodeImage);
            }
        }
        else
        {
            booking.Status = BookingStatus.Failed;
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
        var now = DateTimeOffset.UtcNow;

        if (await _scheduleRepository.ExistsByAsync(_ => _.Id == request.ScheduleId && now >= _.StartTime))
        {
            throw new BadRequestException("không thể book lịch đã diễn ra");

        }

        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
        {
            throw new NotFoundException(nameof(User), request.UserId);
        }

        var booking = request.Adapt<Booking>();
        booking.Status = BookingStatus.Failed;

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
