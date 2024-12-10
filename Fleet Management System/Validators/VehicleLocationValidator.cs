using Fleet_Management_System.Enum;
using Fleet_Management_System.RequestModels;
using FluentValidation;

namespace Fleet_Management_System.Validators
{
    public class VehicleLocationValidator : AbstractValidator<VehicleLocationRequestModel>
    {
        public VehicleLocationValidator()
        {
            // RegistrationNumber validation
            RuleFor(v => v.RegistrationNumber)
                .NotEmpty().WithMessage("Registration number is required.")
                // Allow spaces between letters and digits, and allow the province code at the end
                .Matches(@"^[A-Z]{2}\s?\d{4}\s?[A-Z]{2}$").WithMessage("Registration number must start with 2 letters, followed by 4 digits, and end with a valid province code (e.g., ZN, GP).")
                .Must(EndWithValidProvince)
                .WithMessage("Registration number must end with a valid province code (e.g., ZN, GP).");

            // Latitude validation
            RuleFor(v => v.Latitude)
                .InclusiveBetween(-90, 90)
                .WithMessage("Latitude must be between -90 and 90.");

            // Longitude validation
            RuleFor(v => v.Longitude)
                .InclusiveBetween(-180, 180)
                .WithMessage("Longitude must be between -180 and 180.");
        }

        // Custom validator to check if RegistrationNumber ends with a valid province code
        private bool EndWithValidProvince(string registrationNumber)
        {
            if (string.IsNullOrWhiteSpace(registrationNumber))
                return false;

            var province = registrationNumber[^2..]; // Get the last 2 characters
            return ProvinceConstant.AllProvinces.Contains(province);
        }
    }
}