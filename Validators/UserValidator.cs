using FluentValidation;
using WebServiceForDatabase.DatabaseContext;
using WebServiceForDatabase.Models;

namespace WebServiceForDatabase.Validators
{
    public class UserValidator : AbstractValidator<UserModel>
    {
        public UserValidator()
        {
            RuleFor(x => x.iD)
                .NotNull();

            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(2, 200)
                .Must(BeAValidName)
                .WithMessage("Wrong name format");

            RuleFor(x => x.Surname)
                .NotEmpty()
                .Length(2, 200)
                .Must(BeAValidName)
                .WithMessage("Wrong surname format");

            RuleFor(x => x.birthDate)
                .NotEmpty()
                .Must(BeAValidDate)
                .WithMessage("Wrong date format");

            RuleFor(x => x.Address)
                .NotEmpty()
                .Length(5, 250)
                .WithMessage("Address is too short or too long");
                //Address should be broken into postal code, street, city, other administive areas,
                //stored in seperated model and validated according to region in which the app is used field by field.
                //Another issue is that someone can live in many places and/or can order packages to work or to relative
                //so in address field we should only store list of iDs
                //Having 3 models is also viable - one with user data, one with single address and one to glue them up
                //containing userId and addressId, then we could look for all addresses associated with given user by checking iDs.
                //When address is contained in a single string there is little that we can check.
        }
        private bool BeAValidDate(string dateString)
        {
            //Birth date cannot be placed in the future and must be possible to parse into DateTime type
            bool isConvertable = DateTime.TryParse(dateString, out DateTime result);
            if (result > DateTime.Now || !isConvertable)
            {
                return false;
            }
            return true;
        }
        private bool BeAValidName(string name)
        {
            if (char.IsUpper(name.ElementAt(0))) //Names and surnames need to start with uppercase letter
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
