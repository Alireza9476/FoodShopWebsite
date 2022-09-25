using FoodShopWebsite.Domain.Model;
using FoodShopWebsite.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FoodShopWebsite.Application.UserApp
{
    public interface IUserService
    {
        public Func<IQueryable<User>, IOrderedQueryable<User>>? SortOrderExpression { get; set; }
        public List<Expression<Func<User, bool>>> FilterExpressions { get; set; }


        public Func<IQueryable<UserDto>, PegenatedList<UserDto>> PagingExpression { get; set; }

        public PegenatedList<UserDto> ListAll();

    }
}
