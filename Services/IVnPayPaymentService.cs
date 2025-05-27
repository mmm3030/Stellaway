using Stellaway.DTOs.Payments;

namespace Stellaway.Services;

public interface IVnPayPaymentService
{
    public Task<string> CreatePaymentAsync(VnPayPayment payment);

}