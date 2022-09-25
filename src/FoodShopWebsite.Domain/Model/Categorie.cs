using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShopWebsite.Domain.Model
{
    public class Categorie : EntityBase
    {
        public Categorie() { }
        public Categorie(Guid guid, VAT vAT, string name, string shortDescription)
        {
            Guid = guid;
            VAT = vAT;
            Name = name;
            ShortDescription = shortDescription;
        }

        public Guid Guid{ get; set; }

        public VAT VAT { get; set; }
        public string Name { get; set; } = string.Empty;    
        public string ShortDescription { get; set; } = string.Empty;
    }
}
