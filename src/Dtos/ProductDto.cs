using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodShopWebsite.Domain.Model;
using System.ComponentModel.DataAnnotations;

namespace FoodShopWebsite.Dtos
{
    public class ProductDto
    {
        //Ist zwar alles wie im Model aber es gibt nichts was wichtig wäre was der User nicht sehen darf, wenn einfach entfernen
        public Guid Guid { get; set; }
      
        public Categorie Categorie { get; set; }
        public Guid CategorieGuid { get; set; }
        public string? CategorieName { get; set; } = default!;

       
        public ProductDetails? ProductDetails { get; set; }
        public Guid ProductDetailsGuid { get; set; }
        public decimal ProductDetailsWeight { get; set; }        //Weight, Kcal, Sugar, Carb -> Focus = Weight, Kcal
        public decimal ProductDetailsKcal { get; set; }
        public decimal ProductDetailsSugar { get; set; }
        public decimal ProductDetailsCarb { get; set; }

        [Required(ErrorMessage = "Der Name ist erfolderlich!")]
        [MinLength(3, ErrorMessage = "Der Name darf mindestens 3 Zeichen beinhalten!")]
        [MaxLength(10, ErrorMessage = "Der Name darf maximal 10 Zeichen beinhalten!")]
        public string Name { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = String.Empty;

        [Required(ErrorMessage = "Der Preis ist erfolderlich!")]
        [Range(1, 10000, ErrorMessage = "Der Preis darf: Keine Buchstaben beinhalten, nicht 0 sein und nicht 10000 überschreiten")]
        public int Price { get; set; }

        public DiscountProduct? DiscountProduct { get; set; }
        public Guid DiscountProductGuid { get; set; }
        public string? DiscountProductPercent { get; set; }
        public DateTime ChangedTime { get; set; }

    }
}
