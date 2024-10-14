namespace backend_v3.Models
{
    public class PaymentRecord
    {
        public string Id { get; set; }
        public string StripeSessionId { get; set; }
        public DateTime Created { get; set; }
        public string Status { get; set; }
        public string UserId { get; set; }
    }
}
