using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShopWebsite.Domain.Model
{
    public class Cart : EntityBase
    {

        ///////////////////////////////////////////////////////////////////////////////////
        public Cart() { }
        public Cart(Guid guid, DiscountCodeAll discountCodeAll, VAT vAT, DateTime currentTime, DateTime changedTime)
        {
            Guid = guid;
            DiscountCodeAll = discountCodeAll;
            VAT = vAT;
            CurrentTime = currentTime;
            ChangedTime = changedTime;
        }

        ///////////////////////////////////////////////////////////////////////////////////
        public Guid Guid{ get; set; }
        public DiscountCodeAll? DiscountCodeAll { get; set; }
        public VAT VAT { get; set; }

        public DateTime CurrentTime { get; set; }
        public DateTime ChangedTime { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////


        public decimal cartPriceAll { get; set; }
        public int CountProduct { get; set; }

        //////////////////////////////////////User//////////////////////////////////

        private List<User> _user = new();
        public virtual IReadOnlyList<User> Users => _user;

        public void AddUser(User user)
        {
            if (user is not null)
                if (!_user.Any(u => u.Guid == user.Guid))
                    _user.Add(user);
        }

        ///////////////////////////////////Product/////////////////////////////////////////

        private List<Product> _product = new();
        public virtual IReadOnlyList<Product> Product => _product;

        public void AddProduct(Product product)
        {
            if (product is not null)
                if (!_product.Any(p => p.Guid == product.Guid))
                {
                     _product.Add(product);
                }
        }

        ///////////////////////////////////////////////////////////////////////////////////

    }
}
