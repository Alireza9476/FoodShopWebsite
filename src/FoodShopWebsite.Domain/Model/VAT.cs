using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShopWebsite.Domain.Model
{
    public class VAT : EntityBase
    {
        public VAT() { }
        public VAT(Guid guid, string country, int vATPercent)
        {
            Guid = guid;
            Country = country;
            VATPercent = vATPercent;
        }
        public Guid Guid { get; set; }
        public int VATPercent { get; set; }
        public string Country { get; set; }
    }
}
