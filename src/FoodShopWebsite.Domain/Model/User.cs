using FoodShopWebsite.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShopWebsite.Domain.Model
{
    public class User : EntityBase
    {
        protected User() { }
        public User(Guid guid, string firstName, string secondName, string email,
                    Gender gender, string phoneNumer,
                    DateTime createdTime, DateTime changedTime, Adresse adresse)
        {
            Guid = guid;
            FirstName = firstName;
            SecondName = secondName;
            Email = email;
            Gender = gender;
            PhoneNumber = phoneNumer;
            Adresse = adresse;
            CreatedTime = createdTime;
            ChangedTime = changedTime;
        }

        public Guid Guid { get; set; }
        //public int CartForeignkey { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string SecondName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; }
        public Adresse Adresse { get; set; }

        public DateTime CreatedTime { get; set; }
        public DateTime ChangedTime { get; set; }


        ///////////////////////////////////////////////////////////////////////////////////

        private List<Product> _product = new();
        public virtual IReadOnlyList<Product> Product => _product;

        // public decimal cartPriceAll { get; set; }

        public void AddProduct(Product product)
        {
            if (product is not null)
                if (!_product.Any(p => p.Guid == product.Guid))
                {
                    _product.Add(product);
                }
        }

        private List<Cart> _cart = new();
        public virtual IReadOnlyList<Cart> Cart => _cart;

        public void AddCart(Cart cart)
        {
            if (cart is not null)
            {
                if (!_cart.Any(c => c.Guid == cart.Guid))
                {
                    _cart.Add(cart);
                }
            }
        }
    }
    ///////////////////////////////////////////////////////////////////////////////////

    //They have no identity.
    //They are immutable.
    public enum Gender
    {
        Male,
        Female,
        Diverse
    }

    public record PhoneNumber(string Number)
    {
        public string GetPrefix()
        {
            return Number.Split('/')[0];        //Alles vor / zb +43/0681718321 -> +43
        }
        public string GetNumber()
        {
            return Number.Split('/')[1];         //Alles nach / zb +43/0681718321 -> 0681....
        }
    }
}

