using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TestApp
{
    internal class Order : IValidatableObject
    {

        public int Id { get; set; }
        public int Number { get; set; }
        public string Counterparty { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? AuthorId { get; set; }
        public Employee Author { get; set; }
        public override string ToString()
        {
            return $"{Number} ({Counterparty})";
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(this.Counterparty))
                errors.Add(new ValidationResult("Wrong counterparty"));

            if (Number<0)
                errors.Add(new ValidationResult("Wrong number"));

            if (OrderDate == null)
                errors.Add(new ValidationResult("Wrong order date"));

            if (AuthorId == null)
                errors.Add(new ValidationResult("Wrong author"));

            return errors;
        }
    }
}
