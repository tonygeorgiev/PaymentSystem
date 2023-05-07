using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Domain.Models.Base
{
    public abstract class Entity<T>
    {
        public Entity() { }

        public T Id { get; set; }
    }
}
