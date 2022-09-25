using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShopWebsite.Domain.Exceptions
{
    public class ProductServiceCreateException : Exception
    {
        public ProductServiceCreateException()
            : base()
        { }

        public ProductServiceCreateException(string message)
            : base(message)
        { }

        public ProductServiceCreateException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
