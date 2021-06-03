using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TestApp
{
    internal enum Gender { Male, Female, Unknown};
    internal class Employee : IValidatableObject
    {
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public Gender Gender { get; set; } = Gender.Unknown;
        public DateTime? BirthDate { get; set; }
        public int UnitId { get; set; }
        public Unit Unit { get; set; }
        public ICollection<Unit> Subordinated { get; set; }

        public ICollection<Order> Orders { get; set; }
        
        public Employee()
        {
            Orders = new List<Order>();
        }
        public override string ToString()
        {

            if (Name.Length == 0) return $"{Surname}";
            if (Patronymic.Length == 0) return $"{Surname} {Name[0]}.";
            return $"{Surname} {Name[0]}.{Patronymic[0]}.";
        }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(Name))
                errors.Add(new ValidationResult("Wrong name"));
            if (string.IsNullOrWhiteSpace(Surname))
                errors.Add(new ValidationResult("Wrong surname"));
            if (BirthDate == null)
                errors.Add(new ValidationResult("Wrong birthdate"));
            if (string.IsNullOrWhiteSpace(this.Name))
                errors.Add(new ValidationResult("Wrong name"));


            return errors;
        }
        
    }
}
