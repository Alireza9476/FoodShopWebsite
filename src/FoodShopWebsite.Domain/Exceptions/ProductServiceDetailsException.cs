using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShopWebsite.Domain.Exceptions
{
    public class ProductServiceDetailsException : Exception
    {
        public ProductServiceDetailsException()
           : base()
        { }

        public ProductServiceDetailsException(string message)
            : base(message)
        { }

        public ProductServiceDetailsException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
