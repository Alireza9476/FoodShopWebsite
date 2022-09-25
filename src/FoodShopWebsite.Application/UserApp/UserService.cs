using FoodShopWebsite.Domain.Model;
using FoodShopWebsite.Dtos;
using FoodShopWebsite.Infrastructure;
using FoodShopWebsite.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FoodShopWebsite.Application.UserApp
{
    public class UserService : IUserService
    {
        //Dependency Injection
        //private readonly TestFoodShopWebsiteDbContext _dbContext;
        public Func<IQueryable<User>, IOrderedQueryable<User>>? SortOrderExpression { get; set; }
        public List<Expression<Func<User, bool>>> FilterExpressions { get; set; } = new();

        public Func<IQueryable<UserDto>, PegenatedList<UserDto>> PagingExpression { get; set; }

        private readonly IRepositoryBase<User> _userRepository;

        public UserService(IRepositoryBase<User> UserRepository)
        {
            _userRepository = UserRepository;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////
       
        public PegenatedList<UserDto> ListAll()
        {
        IQueryable<User> result = _userRepository.GetAll(null, null, null);

            foreach (Expression<Func<User, bool>> filter in FilterExpressions)
            {
                if (filter is not null)
                    result = result.Where(filter);
            }

            if (SortOrderExpression is not null)
                result = SortOrderExpression(result);

            IQueryable<UserDto> users = result.Select(u => new UserDto()
            {
                FirstName = u.FirstName,
                SecondName = u.SecondName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Gender = u.Gender,
                Adresse = new AdressDto() { City = u.Adresse.City, Street = u.Adresse.Street, Zip = u.Adresse.Zip }
            });

            if (PagingExpression is not null)
            {
                return PagingExpression(users);
            }

            return PegenatedList<UserDto>.CreateWithoutPaging(users);
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
