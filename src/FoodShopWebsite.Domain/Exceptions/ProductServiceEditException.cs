using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShopWebsite.Domain.Exceptions
{
    public class ProductServiceEditException : Exception
    {
        public ProductServiceEditException()
         : base()
        { }

        public ProductServiceEditException(string message)
            : base(message)
        { }

        public ProductServiceEditException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
