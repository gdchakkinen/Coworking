using System.ComponentModel.DataAnnotations;

namespace Coworking.Application.Validations
{
    public class FutureHourOnlyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateTime)
            {
                var now = DateTime.Now;

                if (dateTime <= now)
                    return new ValidationResult("A reserva deve ser para um horário futuro.");

                if (dateTime.Minute != 0 || dateTime.Second != 0)
                    return new ValidationResult("A reserva deve estar em uma hora cheia (ex: 14:00).");

                if (dateTime.Hour < 8 || dateTime.Hour > 16)
                    return new ValidationResult("O horário da reserva deve ser entre 08:00 e 17:00.");

                return ValidationResult.Success;
            }

            return new ValidationResult("Data e hora inválidas.");
        }
    }
}
