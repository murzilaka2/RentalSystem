namespace RentalSystem.Models
{
    public class Payment
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string PaymentMethod { get; set; }

        public Payment()
        {
            
        }
        public Payment(decimal amount, DateTime date, string paymentMethod)
        {
            Amount = amount;
            Date = date;
            PaymentMethod = paymentMethod;
        }

        public void DisplayInfo()
        {
            
        }
    }
}
