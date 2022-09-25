using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShopWebsite.Domain.Model
{
    public class DiscountProduct : EntityBase
    {
        public DiscountProduct() { }
        public DiscountProduct(Guid guid, string percent, DateTime validTimeFrom, DateTime validTimeTo)
        {
            Guid = guid;
            Percent = percent;
            ValidTimeFrom = validTimeFrom;
            ValidTimeTo = validTimeTo;
        }

        public Guid Guid { get; set; }
        public string Percent { get; set; }
        public DateTime ValidTimeFrom { get; set; }
        public DateTime ValidTimeTo { get; set; }

    }
}
