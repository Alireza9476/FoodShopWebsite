using FoodShopWebsite.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShopWebsite.Application.CategorieApp
{
    public interface ICategorieService
    {
        public IEnumerable<CategorieDto> ListAll();
    }
}
