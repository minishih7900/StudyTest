using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList.Mvc;

namespace Study.Models.Models
{
    public class HkLot49ViewModel
    {
        // Properties
        public string StartDate { get; set; }  // 搜尋條件1

        public string EndDate { get; set; }  // 搜尋條件2

        public IPagedList<LotNumber6> LotNumber6 { get; set; }  // 符合條件資料
        public int Page { get; set; }  // 頁碼

        public List<LotNumber> LotNumberNoPage { get; set; }  // 符合條件資料不用分頁

        // Constructors
        public HkLot49ViewModel()
        {
            StartDate = string.Empty;
            EndDate = string.Empty;
            Page = 0;
        }
    }
}
