using Stellaway.Common.Payments;

namespace Stellaway.Services;

public interface IVnPayPaymentService
{
    public Task<string> CreatePaymentAsync(VnPayPayment payment);

}