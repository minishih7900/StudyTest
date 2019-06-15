using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Study.Models.RouteTest
{
    public class TempProducts
    {
        public int id { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }

        public static List<TempProducts> getAllProducts()
        {
            List<TempProducts> result = new List<TempProducts>();
            result.Add(new TempProducts { id = 1, name = "公車",price=300 });
            result.Add(new TempProducts { id = 2, name = "小巴", price = 100 });
            result.Add(new TempProducts { id = 3, name = "計程車", price = 30 });
            return result;
        }
    }
}