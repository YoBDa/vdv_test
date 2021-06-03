using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace TestApp
{
    internal class Unit : IValidatableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Employee Head { get; set; }
        public int? HeadId { get; set; }
        public ICollection<Employee> Employees { get; set; }
        public Unit()
        {
            Employees = new List<Employee>();
        }

        public override string ToString()
        {
            return Name;
        }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(this.Name))
                errors.Add(new ValidationResult("Wrong name"));


            return errors;
        }
    }
}
