namespace Application.UseCases.Orders.CancelOrder
{
    public class CancelOrderOutput
    {
        public Guid OrderId { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public DateTime CancelationDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
