namespace backend_v3.Dto
{
    public class PaymentParam
    {
        public string? successUrl { get; set; }
        public string? cancelUrl{ get; set; }
        public long? priceInCents { get; set; }
        public string? productName { get; set; }
        public string? userId { get; set; }

        
    }
}
