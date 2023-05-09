namespace PaymentSystem.Domain.Models.Base
{
    public abstract class Entity<T>
    {
        public Entity() { }

        public T Id { get; set; }
    }
}
