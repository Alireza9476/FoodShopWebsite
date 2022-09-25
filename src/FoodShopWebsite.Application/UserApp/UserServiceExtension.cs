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
    public static class UserServiceExtension
    {
        public static IUserService UsePaging(this IUserService service, int pageIndex, int pageSize)
        {
            service.PagingExpression = products => PegenatedList<UserDto>.Create(products, pageIndex, pageSize);
            return service;
        }


        public static IUserService UseSorting(this IUserService service, string orderParam)
        {
            Func<IQueryable<User>, IOrderedQueryable<User>>? SortOrderExpression = null;
            SortOrderExpression = orderParam switch
            {
                "firstname" => e => e.OrderBy(x => x.FirstName),
                "firstname_desc" => e => e.OrderByDescending(x => x.FirstName),

                "secondname" => e => e.OrderBy(x => x.SecondName),
                "secondname_desc" => e => e.OrderByDescending(x => x.SecondName),

                "email" => e => e.OrderBy(x => x.Email),
                "email_desc" => e => e.OrderByDescending(x => x.Email),

                "gender" => e => e.OrderBy(x => x.Gender),
                "gender_desc" => e => e.OrderByDescending(x => x.Gender),

                _ => e => e.OrderBy(x => x.FirstName) //Default Verhalten
            };

            service.SortOrderExpression = SortOrderExpression;
            return service;
        }


        public static IUserService UseProductNameFilter(this IUserService service, string productName)
        {
            Expression<Func<User, bool>> FilterExpressions = default!;
            if (!string.IsNullOrEmpty(productName))
                FilterExpressions = e => e.FirstName.Contains(productName);
            service.FilterExpressions.Add(FilterExpressions);   //FilterExpression in ProductService ist ein Link<Delegate>, deswegen .Add
            return service;
        }

    }
}
