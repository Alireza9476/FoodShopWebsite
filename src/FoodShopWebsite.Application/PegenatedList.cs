using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShopWebsite.Application
{
    public class PegenatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }  //Derzeiges Page localhost:7000/Product/PageIndex=10
        public int TotalPages { get; private set; }


        //items = products, count = gesamte Anzahl Products, pageSize = wv Products in einer Seite
        //NICHT public PegenatedList<T>(){}
        public PegenatedList(List<T> items, int countItems, int pageIndex, int pageSize) 
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(countItems / (double)pageSize);  //Je Seite 10 Produkte, 100 Produkte insgesamt
            AddRange(items);    //erbt von List und hat diese Function
        }  


        //Wenn PageIndex größer 1 dann true, sonst wird Button (Previous) in View deaktiviert
        public bool HasPreviousPage
        {
            get { return (PageIndex > 1);  }
        }

        public bool HasNextPage
        {
            get { return (PageIndex < TotalPages); }
        }


        //Product hat ein PagingExpression wie IPegenatedList, durch Extension Method von Product wird dieser in Controller geused
        //Deswegen muss Create static sein, sie erzeugt ein PegenatedList Object mit pageIndex und pageSize und source
        public static PegenatedList<T> Create(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var items = source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList(); //source kann alles sein, deshalb var
            
            return new PegenatedList<T>(items, source.Count(), pageIndex, pageSize);
        }


        //PageIndex ist 1, deswegen einfach übergeben
        //Konstruktur: public PegenatedList(List<T> items, int countItems, int pageIndex, int pageSize)    
        public static PegenatedList<T> CreateWithoutPaging(IQueryable<T> source)
        {
            return new PegenatedList<T>(source.ToList(), source.Count(), 1, source.Count());
        }

    }
}
