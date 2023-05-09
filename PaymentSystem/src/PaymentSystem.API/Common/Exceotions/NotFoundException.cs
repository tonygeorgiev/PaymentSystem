namespace PaymentSystem.API.Common.Exceotions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
}
