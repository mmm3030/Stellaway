using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Stellaway.Common.Helpers;
using Stellaway.Common.Payments;
using Stellaway.Common.Settings;

namespace Stellaway.Services;

public class MomoPaymentService : IMomoPaymentService
{
    private const string DefaultOrderInfo = "Thanh toán với Momo";

    private readonly MomoSettings _momoSettings;
    private readonly ILogger<MomoPaymentService> _logger;
    private readonly ICurrentUserService _currentUserService;
    public MomoPaymentService(
        IOptions<MomoSettings> momoSettings,
        ILogger<MomoPaymentService> logger,
        ICurrentUserService currentUserService)
    {
        _momoSettings = momoSettings.Value;
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<string> CreatePaymentAsync(MomoPayment payment)
    {
        var requestType = "captureWallet";
        var request = new MomoPaymentRequest();
        request.OrderInfo = payment.Info ?? DefaultOrderInfo;
        request.PartnerCode = _momoSettings.PartnerCode;
        request.IpnUrl = _momoSettings.IpnUrl;
        request.RedirectUrl = $"{_currentUserService.ServerUrl}/{_momoSettings.RedirectUrl}?returnUrl={payment.returnUrl}";

        request.Amount = payment.Amount;
        request.OrderId = payment.PaymentReferenceId;
        request.ReferenceId = $"{payment.PaymentReferenceId}";
        request.RequestId = Guid.NewGuid().ToString();
        request.RequestType = requestType;
        request.ExtraData = "s";
        request.AutoCapture = true;
        request.Lang = "vi";

        var rawSignature = $"accessKey={_momoSettings.AccessKey}&amount={request.Amount}&extraData={request.ExtraData}&ipnUrl={request.IpnUrl}&orderId={request.OrderId}&orderInfo={request.OrderInfo}&partnerCode={request.PartnerCode}&redirectUrl={request.RedirectUrl}&requestId={request.RequestId}&requestType={requestType}";
        request.Signature = GetSignature(rawSignature, _momoSettings.SecretKey);

        var httpContent = new StringContent(JsonSerializerUtils.Serialize(request), Encoding.UTF8, "application/json");
        using var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(30);
        var momoResponse = await httpClient.PostAsync(_momoSettings.PaymentEndpoint, httpContent);
        var responseContent = momoResponse.Content.ReadAsStringAsync().Result;

        if (momoResponse.IsSuccessStatusCode)
        {
            var momoPaymentResponse = JsonSerializerUtils.Deserialize<MomoPaymentResponse>(responseContent);
            _logger.LogInformation($"[Momo payment] Message: {momoPaymentResponse?.Message}");
            if (momoPaymentResponse != null)
            {
                return momoPaymentResponse.PayUrl;
            }
        }

        throw new Exception($"[Momo payment] Error: There is some error when create payment with momo. {responseContent}");
    }

    private static string GetSignature(string text, string key)
    {
        // change according to your needs, an UTF8Encoding
        // could be more suitable in certain situations
        var encoding = new UTF8Encoding();

        var textBytes = encoding.GetBytes(text);
        var keyBytes = encoding.GetBytes(key);

        byte[] hashBytes;

        using HMACSHA256 hash = new HMACSHA256(keyBytes);
        hashBytes = hash.ComputeHash(textBytes);

        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }
}