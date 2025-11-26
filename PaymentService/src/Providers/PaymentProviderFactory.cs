namespace PaymentService.Providers
{
    public class PaymentProviderFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public PaymentProviderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IPaymentProvider GetProvider(string provider)
        {
            return provider.ToLower() switch
            {
                "stripe" => _serviceProvider.GetRequiredService<StripePaymentProvider>(),
                "vnpay" => _serviceProvider.GetRequiredService<VnPayPaymentProvider>(),
                "momo" => _serviceProvider.GetRequiredService<MomoPaymentProvider>(),
                _ => throw new ArgumentException($"Unsupported payment provider: {provider}")
            };
        }
    }

}
