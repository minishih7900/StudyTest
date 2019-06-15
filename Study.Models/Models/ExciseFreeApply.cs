using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study.Models.Models
{
    public class ExciseFreeApply
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public string Address { get; set; }
        public string ProdTaxNumber { get; set; }
        public string ProdChName { get; set; }
        public DateTime ProcessDate { get; set; }
        public decimal Qty { get; set; }
        public string SheetNumber { get; set; }
        public string TaxUnits { get; set; }
        public string Mode { get; set; }
        public string ProdEngName { get; set; }
    }
}
