using Microsoft.Extensions.Options;
using Stellaway.Common.Helpers;
using Stellaway.Common.Settings;
using Stellaway.DTOs.Payments;

namespace Stellaway.Services;

public class VnPayPaymentService : IVnPayPaymentService
{
    private readonly VnPaySettings _vnPaySettings;

    private const string DefaultPaymentInfo = "Thanh toán với VnPay";
    private readonly ICurrentUserService _currentUserService;

    public VnPayPaymentService(
        IOptions<VnPaySettings> vnPaySettings,
        ICurrentUserService currentUserService)
    {
        _vnPaySettings = vnPaySettings.Value;
        _currentUserService = currentUserService;
    }

    public async Task<string> CreatePaymentAsync(VnPayPayment payment)
    {
        var pay = new VnPayLibrary();
        pay.AddRequestData("vnp_ReturnUrl", $"{_currentUserService.ServerUrl}/{_vnPaySettings.CallbackUrl}?returnUrl={payment.returnUrl}");
        pay.AddRequestData("vnp_Version", _vnPaySettings.Version);
        pay.AddRequestData("vnp_Command", _vnPaySettings.Command);
        pay.AddRequestData("vnp_TmnCode", _vnPaySettings.TmnCode);
        pay.AddRequestData("vnp_CurrCode", _vnPaySettings.CurrCode);
        pay.AddRequestData("vnp_Locale", _vnPaySettings.Locale);

        pay.AddRequestData("vnp_Amount", ((int)payment.Amount * 100).ToString());
        pay.AddRequestData("vnp_CreateDate", payment.Time.ToString("yyyyMMddHHmmss"));
        pay.AddRequestData("vnp_IpAddr", UtilitiesExtensions.GetIpAddress());
        pay.AddRequestData("vnp_OrderInfo", payment.Info ?? DefaultPaymentInfo);
        pay.AddRequestData("vnp_OrderType", payment.OrderType.ToString());
        pay.AddRequestData("vnp_TxnRef", payment.PaymentReferenceId);

        var paymentUrl = pay.CreateRequestUrl(
            _vnPaySettings.PaymentEndpoint,
            _vnPaySettings.HashSecret);

        return await Task.FromResult(paymentUrl);
    }
}