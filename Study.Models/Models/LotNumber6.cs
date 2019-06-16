using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study.Models.Models
{
    public class LotNumber6
    {
        [Required]
        [RegularExpression("[0-9]*", ErrorMessage = "This field requires numeric character.")]
        public string 期數 { get; set; }
        [Required]
        public string 開獎日期 { get; set; }
        [Required]
        public string 號碼1 { get; set; }
        [Required]
        public string 號碼2 { get; set; }
        [Required]
        public string 號碼3 { get; set; }
        [Required]
        public string 號碼4 { get; set; }
        [Required]
        public string 號碼5 { get; set; }
        [Required]
        public string 號碼6 { get; set; }
        [Required]
        public string 特別號 { get; set; }
    }

    public class SelectLotNumber6
    {
        public int[] selectNumber6CountDarry { get; set; }
        public Dictionary<int?, int?> selectNumber6CountList { get; set; }
        public Dictionary<int?, int?> selectNumber6CountListOrderBy { get; set; }

    }
}
