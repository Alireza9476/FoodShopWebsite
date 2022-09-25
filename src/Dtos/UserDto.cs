using FoodShopWebsite.Domain.Model;

namespace FoodShopWebsite.Dtos
{
    public class UserDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string SecondName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public AdressDto Adresse { get; set; } = default!;
    }
}