using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShopWebsite.Domain.Model
{
    public class DiscountCodeAll : EntityBase
    {
        DiscountCodeAll() { }
        public DiscountCodeAll(Guid guid, string code, decimal percent,
                                DateTime validTimeFrom, DateTime validTimeTo)
        {
            Guid = guid;
            Code = code;
            Percent = percent;
            ValidTimeFrom = validTimeFrom;
            ValidTimeTo = validTimeTo;
           
        }

        public Guid Guid { get; set; }
        public string Code { get; set; } = String.Empty;
        public decimal Percent { get; set; }
        public DateTime ValidTimeFrom { get; set; }
        public DateTime ValidTimeTo { get; set; }

    }

}
