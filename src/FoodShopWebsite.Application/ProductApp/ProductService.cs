using Microsoft.EntityFrameworkCore;  //Damit Include() benutzt werden kann  result = result.Include(p => p.Categorie);

using FoodShopWebsite.Domain.Model;
using FoodShopWebsite.Repository;
using FoodShopWebsite.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FoodShopWebsite.Infrastructure;
using FoodShopWebsite.Application.DiscountProducts;
using FoodShopWebsite.Domain.Exceptions;

namespace FoodShopWebsite.Application.ProductApp
{
    public class ProductService : IProductService
    {
        public List<Expression<Func<Product, bool>>> FilterExpressions { get; set; } = new();
        public Func<IQueryable<Product>, IOrderedQueryable<Product>>? SortOrderExpression { get; set; }
        public Func<IQueryable<ProductDto>, PegenatedList<ProductDto>> PagingExpression { get; set; }

        private readonly IRepositoryBase<Product> _productRepository;
        private readonly IRepositoryBase<DiscountProduct> _discountProductRepository;
        private readonly IRepositoryBase<Categorie> _categorieRepository;
        private readonly IRepositoryBase<ProductDetails> _productDetailsRepository;

        public ProductService(IRepositoryBase<Product> productRepository,
                              IRepositoryBase<DiscountProduct> discountProductRespository,
                              IRepositoryBase<Categorie> categorieRepository,
                              IRepositoryBase<ProductDetails> productDetails
            )
        {
            _productRepository = productRepository;
            _discountProductRepository = discountProductRespository;
            _categorieRepository = categorieRepository;
            _productDetailsRepository = productDetails;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////

        //Das wird nach dem Filtern, sortieren(Extension Mehode) usw in Service erstellt
        public PegenatedList<ProductDto> ListAll()
        {
            IQueryable<Product> result = _productRepository.GetAll(null, null , "Categorie,ProductDetails,DiscountProduct");

            //filter
            foreach (Expression<Func<Product, bool>> filter in FilterExpressions)
            {
                if (filter is not null)
                    result = result.Where(filter);
            }

            //sort
            if (SortOrderExpression is not null)
                result = SortOrderExpression(result);   //Eine Funktion (Delegate) die woanders aufgerufen wird

            //include
            //result = result.Include(p => p.Categorie);
            //result = result.Include(p => p.ProductDetails);
            //result = result.Include(p => p.DiscountProduct);

            //Dto die in dieser Func ins Controller zurück gegeben wird -> View -> Model.DiscountProductPercent
            IQueryable<ProductDto> products = result.Select(p => new ProductDto()
            {
                Guid = p.Guid,
                Name = p.Name,
                CategorieName = p.Categorie.Name,
                Categorie = p.Categorie,

                ProductDetailsGuid = p.ProductDetails.Guid,
                ProductDetailsWeight = p.ProductDetails.Weight,
                ShortDescription = p.ShortDescription,
                Price = p.Price,
                DiscountProduct = p.DiscountProduct,
                DiscountProductPercent = p.DiscountProduct.Percent,
                ChangedTime = (DateTime)p.ChangedTime
            });

            if(PagingExpression is not null)
            {
                return PagingExpression(products);
            }

            return PegenatedList<ProductDto>.CreateWithoutPaging(products);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////

        public bool Create(ProductDto dto, Guid DiscountProduct, Guid Categorie)
        {
            if (dto.Name.Length < 3)
            {
                throw new ServiceValidationException("Name muss mindestens 3 Zeichen lang sein");
            }

            if (dto.Name.Length >= 10)
            {
                throw new ServiceValidationException("Name muss maximal 10 Zeichen lang sein");
            }

            if (dto.Price > 10000)
            {
                throw new ServiceValidationException("Der Preis darf 10000 nicht überschreiten");
            }


            Product newProduct = new Product(Guid.NewGuid(), dto.ProductDetails, dto.Categorie, dto.Name, dto.Name[..3], dto.Price, dto.DiscountProduct, dto.ChangedTime);

            DiscountProduct newDiscountProduct = _discountProductRepository.GetSingle(dp => dp.Guid == DiscountProduct)
                ?? throw new ProductServiceCreateException("DiscountProduct konnte nicht gefunden werden");

            Categorie newCategorie = _categorieRepository.GetSingle(c => c.Guid == Categorie)
                ?? throw new ProductServiceCreateException("Create konnte nicht gefunden werden");

            Guid ProductDetailsGuid = new Guid("c10c3dcc-41f9-43b4-a0fb-ea42e8435e93");
            ProductDetails newProductDetails = _productDetailsRepository.GetSingle(pd => pd.Guid == ProductDetailsGuid)
                ?? throw new ProductServiceCreateException("ProductDetails konnte nicht gefunden werden");

            if (DiscountProduct != Guid.Empty)
                newProduct.DiscountProduct = newDiscountProduct;
            if (Categorie != Guid.Empty)
                newProduct.Categorie = newCategorie;
            if (ProductDetailsGuid != Guid.Empty)
                newProduct.ProductDetails = newProductDetails;

            newProduct.ChangedTime = DateTime.Now;
            try
            {
                _productRepository.Create(newProduct);
                return true;
            }
            catch (RepositoryCreateException ex) 
            {
                throw new ProductServiceCreateException("Methode [Create] ist fehlgeschlagen" , ex);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////

        public ProductDto Details(Guid id)
        {
            Product entity = _productRepository
                .GetSingle(p => p.Guid == id, "Categorie,ProductDetails,DiscountProduct")
                ?? throw new ProductServiceDetailsException("Produkt konnte nicht gelanden werden");

          ProductDto dto = new()
            {
                Guid = entity.Guid,
                Name = entity.Name,
                ShortDescription = entity.ShortDescription,
                Price = entity.Price,
                
                Categorie = entity.Categorie,
                CategorieName = entity.Categorie.Name,
              

              ProductDetailsWeight = entity.ProductDetails.Weight,
              ProductDetailsKcal = entity.ProductDetails.Kcal,
              ProductDetailsSugar = entity.ProductDetails.Sugar,
              ProductDetailsCarb = entity.ProductDetails.Carb,
          };
            
            if(entity.DiscountProduct != null) {
                dto.DiscountProduct = entity.DiscountProduct;
                dto.DiscountProductPercent = entity.DiscountProduct.Percent;
            }

            if (entity.ChangedTime != null)
                dto.ChangedTime = (DateTime) entity.ChangedTime;    

            return dto;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////

        public bool Edit(Guid id, int price, string? name, Guid discountProductPercent)
        {
            Product entity = _productRepository.GetSingle(p => p.Guid == id, null)
             ?? throw new ProductServiceEditException("Produkt konnte nicht gelanden werden");

            //update price
            entity.Name = name != null ? name : entity.Name;
            entity.Price = price != 0 ? price : entity.Price;

            DiscountProduct entityDiscountProduct = _discountProductRepository.GetSingle(dp => dp.Guid == discountProductPercent)
                ?? throw new ProductServiceEditException("DiscountProduct konnte nicht gefunden werden");

            if (discountProductPercent != Guid.Empty)
                entity.DiscountProduct = entityDiscountProduct;
                //entity.DiscountProduct.Percent = discountProductPercent != 0.0m ? discountProductPercent : entity.DiscountProduct.Percent;    

            if (price == 0 && name == null && entityDiscountProduct == null)
                return false;
            
            entity.ChangedTime = DateTime.Now;

            bool editComplete = false;
            try
            {
                editComplete = _productRepository.Edit(entity);
            }
            catch (RepositoryCreateException ex)
            {
                throw new ProductServiceEditException("Methode [Edit] ist fehlgeschlagen", ex);
            }

            return editComplete;
        }
        //  public TEntity Edit(TEntity newModel)

        ///////////////////////////////////////////////////////////////////////////////////////////

        public bool Delete(Guid id)
        {
            Product entity = _productRepository.GetSingle(p => p.Guid == id)
                ?? throw new ProductServiceDeleteException("Product konnte nicht gefunden werden");

            if (id != Guid.Empty) { 
                try 
                {
                     _productRepository.Delete(entity);
                    return true;
                }
                catch (RepositoryCreateException ex)
                {
                    throw new ProductServiceDeleteException("Methode [Delete] ist fehlgeschlagen", ex);
                }
            }
            return false;
        }

        public DiscountProduct? DiscountProduct { get; set; }
        public decimal? DiscountProductPercent { get; set; }
        public DateTime ChangedTime { get; set; }

    }
}
