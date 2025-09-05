using App.Domain.Helpers;
using App.Domain.ValueObject;
using App.Infrastructure.Domain;

namespace App.Domain.ValueObjects
{
    public class Basic : EntityBase<int>, IAggregateRoot
    {
        public string Name { get; set; }
        public Email Email { get; set; }

        protected override void Validate()
        {
            Validator.ForNullOrEmptyDefaultMessage(Name, "E-mail");
        }
    }
}
