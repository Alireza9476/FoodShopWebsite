using FoodShopWebsite.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace FoodShopWebsite.Dtos
{
    public class CategorieDto
    {
        public Guid Guid { get; set; }
        public VAT? VAT { get; set; } = default!;
        public int VATPercent { get; set; }

        [Compare("Nichts ausgewählt", ErrorMessage = "Kategorie muss angegeben sein")]
        public string Name { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
    }
}
