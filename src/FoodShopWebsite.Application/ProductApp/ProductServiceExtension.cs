using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FoodShopWebsite.Domain.Model;
using FoodShopWebsite.Dtos;


namespace FoodShopWebsite.Application.ProductApp
{
    public static class ProductServiceExtension
    {

        //Wird im Contoller von Product verwendet
        public static IProductService UsePaging(this IProductService service, int pageIndex, int pageSize)
        {
            service.PagingExpression = products => PegenatedList<ProductDto>.Create(products, pageIndex, pageSize);
            return service;
        }
        

        public static IProductService UseSorting(this IProductService service, string orderParam)
        {
            Func<IQueryable<Product>, IOrderedQueryable<Product>>? SortOrderExpression = null;
            SortOrderExpression = orderParam switch
            {
                "name" => e => e.OrderBy(x => x.Name),
                "name_desc" => e => e.OrderByDescending(x => x.Name),

                "kategorie" => e => e.OrderBy(x => x.Categorie.Name),
                "kategorie_desc" => e => e.OrderByDescending(x => x.Categorie.Name),

                "preis" => e => e.OrderBy(x => x.Price),
                "preis_desc" => e => e.OrderByDescending(x => x.Price),

                "rabatt" => e => e.OrderBy(x => x.DiscountProduct),
                "rabatt_desc" => e => e.OrderByDescending(x => x.DiscountProduct),

                "changedProductDate" => e => e.OrderBy(x => x.ChangedTime),
                "changedProductDate_desc" => e => e.OrderByDescending(x => x.ChangedTime),

                _ => e => e.OrderBy(x => x.Name) //Default Verhalten
            };

            service.SortOrderExpression = SortOrderExpression;
            return service;
        }


        public static IProductService UseProductNameFilter(this IProductService service, string productName)
        {
            Expression<Func<Product, bool>> FilterExpressions = default!;
            if(!string.IsNullOrEmpty(productName))
                FilterExpressions = e => e.Name.Contains(productName);
            service.FilterExpressions.Add(FilterExpressions);   //FilterExpression in ProductService ist ein Link<Delegate>, deswegen .Add
            return service;
        }

        public static IProductService UseProductDateFilter(this IProductService service, string dateFrom, string dateTo)
        {
            DateTime dateFromFilter;
            DateTime dateToFilter;

            List<Expression<Func<Product, bool>>> filterExpressions = new();
            Expression<Func<Product, bool>>? dateFilterExpression = default!;

            if (!string.IsNullOrEmpty(dateFrom) && DateTime.TryParse(dateFrom, out dateFromFilter))
            {
                dateFilterExpression = e => e.ChangedTime.HasValue && e.ChangedTime.Value >= dateFromFilter;
                if (!string.IsNullOrEmpty(dateTo) && DateTime.TryParse(dateTo, out dateToFilter))
                {
                    //dateToFilter = dateToFilter.AddDays(dateToFilter.Day + 1);
                    dateFilterExpression = e => e.ChangedTime.HasValue && e.ChangedTime.Value >= dateFromFilter
                        && e.ChangedTime.Value < dateToFilter;
                }
            }
            else if (!string.IsNullOrEmpty(dateTo)
                && DateTime.TryParse(dateTo, out dateToFilter))
            {
                dateFilterExpression = e => e.ChangedTime.HasValue && e.ChangedTime.Value < dateToFilter;
            }
            filterExpressions.Add(dateFilterExpression);
            filterExpressions.Add(dateFilterExpression);

            service.FilterExpressions.Add(dateFilterExpression);
            return service;
        }


    }
}
