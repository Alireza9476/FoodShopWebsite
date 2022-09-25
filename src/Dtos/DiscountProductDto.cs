using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShopWebsite.Dtos
{
    public class DiscountProductDto
    {
        public DiscountProductDto(Guid guid, string percent, DateTime validTimeFrom, DateTime validTimeTo)
        {
            Guid = guid;
            Percent = percent;
            ValidTimeFrom = validTimeTo;
            ValidTimeTo = validTimeTo;
        }
        public Guid Guid { get; set; }
        public string Percent { get; set; }
        public DateTime ValidTimeFrom { get; set; }
        public DateTime ValidTimeTo { get; set; }
    }
}
