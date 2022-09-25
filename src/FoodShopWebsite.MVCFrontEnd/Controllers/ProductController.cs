using FoodShopWebsite.Application;
using FoodShopWebsite.Domain.Exceptions;
using FoodShopWebsite.Application.CategorieApp;
using FoodShopWebsite.Application.DiscountProducts;
using FoodShopWebsite.Application.ProductApp;
using FoodShopWebsite.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodShopWebsite.MVCFrontEnd.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IDiscountProductService _discountProductService;
        private readonly ICategorieService _categorieService;
        public ProductController(IProductService productService, IDiscountProductService discountProductService, ICategorieService categorieService)
        {
            _productService = productService;
            _discountProductService = discountProductService;
            _categorieService = categorieService;
        }

        //?dateFrom=2022-06-09&dateTo=2022-06-11
        public IActionResult Index(string sort, string productNameFilter, string currentProductName, string dateFrom, string dateTo,int pageIndex = 1)
        {
            productNameFilter = productNameFilter ?? currentProductName;

            ViewData["sortParamName"] = sort == "name" ? "name_desc" : "name";
            ViewData["sortParamKategorie"] = sort == "kategorie" ? "kategorie_desc" : "kategorie";
            ViewData["sortParamPreis"] = sort == "preis" ? "preis_desc" : "preis";
            ViewData["sortParamRabatt"] = sort == "rabatt" ? "rabatt_desc" : "rabatt";
            ViewData["sortParamChangedProductDate"] = sort == "changedProductDate" ? "changedProductDate_desc" : "changedProductDate";

            _productService
                .UsePaging(pageIndex, 6)
                .UseSorting(sort)
                .UseProductNameFilter(productNameFilter)
                .UseProductDateFilter(dateFrom, dateTo)
                ;

            PegenatedList<ProductDto> result = _productService.ListAll();

            return View((result, productNameFilter));
        }

        ///////////////////////////////////////////////////////////

        [HttpGet()]
        public IActionResult Details(Guid id)
        {
            ProductDto model = _productService.Details(id);
            return View(model);
        }

        ///////////////////////////////////////////////////////////

        [HttpGet()]
        public IActionResult Create()
        {
            List<CategorieDto> dtoCategorie = _categorieService.ListAll().ToList();
            dtoCategorie.Insert(0, new CategorieDto()
            {
                Guid = Guid.NewGuid(),
                VAT = null,
                VATPercent = 0,
                Name = "Nicht ausgewählt",
                ShortDescription = ""
            });
            ViewBag.CategorieList = new SelectList(dtoCategorie, "Guid", "Name");       //Percent, PercentText

            List<DiscountProductDto> dtoProduct = _discountProductService.ListAll().ToList();
            dtoProduct.Insert(0, new DiscountProductDto(Guid.Empty, "Nichts ausgewählt", new DateTime(), new DateTime()));
            ViewBag.discountProduct = new SelectList(dtoProduct, "Guid", "Percent");       //Percent, PercentText

            return View();
        }

        [HttpPost()]
        public IActionResult Create(ProductDto model, Guid DiscountProduct, Guid Categorie)
        {
            List<CategorieDto> dtoCategorie = _categorieService.ListAll().ToList();
            dtoCategorie.Insert(0, new CategorieDto()
            {
                Guid = Guid.Empty,
                VAT = null,
                VATPercent = 0,
                Name = "Nichts ausgewählt",
                ShortDescription = ""
            });
            ViewBag.CategorieList = new SelectList(dtoCategorie, "Guid", "Name");       //Percent, PercentText

            List<DiscountProductDto> dtoProduct = _discountProductService.ListAll().ToList();
            dtoProduct.Insert(0, new DiscountProductDto(Guid.Empty, "Nichts ausgewählt", new DateTime(), new DateTime()));
            ViewBag.discountProduct = new SelectList(dtoProduct, "Guid", "Percent");       //Percent, PercentText

            try
            {
                _productService.Create(model, DiscountProduct, Categorie);
                return RedirectToAction("Index", "Product");
            }
            catch (ProductServiceCreateException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (ServiceValidationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View();
        }

        /////////////////////////////////////////////////////////// 

        [HttpGet()]
        public IActionResult Edit(Guid id)
        {
            List<DiscountProductDto> dto = _discountProductService.ListAll().ToList();
            dto.Insert(0, new DiscountProductDto(Guid.Empty, "Nichts ausgewählt", new DateTime(), new DateTime()));

            ViewBag.discountProduct = new SelectList(dto, "Guid", "Percent");       //Percent, PercentText

            ProductDto model = _productService.Details(id);
            return View(model);
        }

        [HttpPost()]
        public IActionResult Edit(string Name, int Price, Guid id, Guid discountProduct)
        {
            List<DiscountProductDto> dto = _discountProductService.ListAll().ToList();
            ViewBag.discountProduct = new SelectList(dto, "Guid", "Percent");       //Percent, PercentText
            dto.Insert(0, new DiscountProductDto(Guid.Empty, "Nichts ausgewählt", new DateTime(), new DateTime()));

            //decimal editDiscountProduct = discountProduct != 0.0m ? discountProduct : 0.0m;
            //decimal editDiscountProduct = decimal.Parse(discountProduct);

            ProductDto modelResult = _productService.Details(id);

            try
            {
                _productService.Edit(id, Price, Name, discountProduct);
                return RedirectToAction("Index", "Product");
            }
            catch (ProductServiceEditException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (ServiceValidationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View();
        }

        /////////////////////////////////////////////////////////// 

        [HttpGet()]
        public IActionResult Delete(Guid id)
        {
            ProductDto dto = _productService.Details(id);
            return View((dto, false));
        }

        [HttpPost()]
        public IActionResult Delete(Guid id, int deleted)
        {
            ProductDto dto = _productService.Details(id);
            bool result = _productService.Delete(id);
            return View((dto, result));
        }

    }
}