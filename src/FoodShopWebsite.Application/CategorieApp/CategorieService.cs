using FoodShopWebsite.Domain.Model;
using FoodShopWebsite.Dtos;
using FoodShopWebsite.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShopWebsite.Application.CategorieApp
{
    public class CategorieService : ICategorieService
    {
        private readonly IRepositoryBase<Categorie> _categorieRepository;
        public CategorieService(IRepositoryBase<Categorie> categorieRepository)
        {
            _categorieRepository = categorieRepository;
        }
        public IEnumerable<CategorieDto> ListAll()
        {
            return _categorieRepository
                   .GetAll()
                   .Select(dp => new CategorieDto()
                   {
                       Guid = dp.Guid,
                       Name = dp.Name,
                       ShortDescription = dp.ShortDescription,
                       VAT = dp.VAT,
                       //VATPercent = dp.VAT.VATPercent
                   })
                   .ToList()
                   .DistinctBy(dp => dp.Name)
                   .OrderByDescending(dp => dp.Name)
                   ;
        }
    }
}
