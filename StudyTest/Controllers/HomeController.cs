using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExcelTools;
using System.IO;
using Study.Models.Models;
using Study.Services;
using System.Diagnostics;
using System.Reflection;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private HomeServices _homeService = new HomeServices();

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "這是一個練習網站";
            return View();
        }
        public ActionResult Aboutbutton()
        {
            var MR = new report();
            List<ExciseFreeApply> EF = new List<ExciseFreeApply>
            {
                new ExciseFreeApply {
                    Name ="1111股份有限公司",
                    ID="050",
                    Address="台北市",
                    ProdTaxNumber="123456789012",
                    ProdChName="鳳梨",
                    ProdEngName="鳳梨",
                    ProcessDate=Convert.ToDateTime("2018-10-10"),
                    Qty=2240,
                    Mode="3",
                    TaxUnits="KU",
                    SheetNumber=""
                }
            };
            string downloadPath = MR.ExciseRpt(EF);
            byte[] fileBytes = System.IO.File.ReadAllBytes(downloadPath);
            string fileName = Path.GetFileName(downloadPath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        public ActionResult AboutWord()
        {
            var MR = new report();
            List<LotNumber> mb = new List<LotNumber>();
            mb = _homeService._sqlRepository.GetLotNumber("20190101", "20190131");
            string downloadPath = MR.OutputWord(mb);
            byte[] fileBytes = System.IO.File.ReadAllBytes(downloadPath);
            string fileName = Path.GetFileName(downloadPath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        public ActionResult selectList()
        {
            List<SelectListItem> mySelectItemList = new List<SelectListItem>();

            mySelectItemList.AddRange(new[]{
        new SelectListItem() {Text = "apple", Value = "1"},
        new SelectListItem() {Text = "banana", Value = "2"},
        new SelectListItem() {Text = "watermelon", Value = "3"},
        new SelectListItem() {Text = "guava", Value = "4"}
    });



            SelectList aSelectList = new SelectList(mySelectItemList, "Value", "Text");

            return View(aSelectList);
        }

        public ActionResult Tool()
        {
            return View();
        }
        public ActionResult TestCode()
        {
            
           
           
            
            return RedirectToAction("Tool", "Home");
        }
        [HttpPost]
        public string ranNumber(string inputNumer)
        {
            string guid = Guid.NewGuid().ToString(inputNumer);
            inputNumer = guid.ToString().Replace("-", "");
            
            return inputNumer;
        }
        #region 最合數字
        [HttpGet]
        public ActionResult QuerySelectNumberCount()
        {
            SelectLotNumber model = new SelectLotNumber();
            model.selectNumberCountDarry = new int[40];
            model.selectNumberCountListOrderBy = new Dictionary<int?, int?>();
            return View(model);
        }
        [HttpPost]
        public ActionResult QuerySelectNumberCount(string selectnum,string StartDate,string EndDate,string StartPeriod,string EndPeriod)
        {
            SelectLotNumber model = new SelectLotNumber();
            List<LotNumber> numList = new List<LotNumber>();
            numList = _homeService.GetNumberListServices(selectnum,StartDate,EndDate,StartPeriod,EndPeriod);
            SumNumberCount(model, numList);
            ViewBag.Message = "查詢期間為：" + numList.Select(d => d.開獎日期).Min() + "~" + numList.Select(d => d.開獎日期).Max();
            ViewBag.Data = numList.Count;

            return View(model);
        }

        #endregion

        #region 熱門牌
        [HttpGet]
        public ActionResult HotNumber()
        {
            return View();
        }
        [HttpPost]
        public ActionResult HotNumber(string PeriodNum)
        {
            SelectLotNumber model = new SelectLotNumber();
            List<LotNumber> numList = new List<LotNumber>();
            numList = _homeService.GetNumberTopServices(PeriodNum);
            SumNumberCount(model, numList);
            ViewBag.Message = "查詢期間為：" + numList.Select(d => d.開獎日期).Min() + "~" + numList.Select(d => d.開獎日期).Max();
            ViewBag.Data = numList.Count;
            return View(model);
        }
       
        #endregion

        #region 查詢號碼
        [HttpGet]
        public ActionResult QueryNumber()
        {
            List<LotNumber> model = new List<LotNumber>();
            model = _homeService.GetNumberTopServices("30");
            ViewBag.Message = "查詢期間為：" + model.Select(p => p.開獎日期).Min() + "~" + model.Select(p => p.開獎日期).Max();
            return View(model);
        }
        [HttpPost]
        public ActionResult QueryNumber(string StartDate ,string EndDate)
        {
            ViewBag.Message ="查詢期間為："+ StartDate + "~" +EndDate;
            List<LotNumber> model = new List<LotNumber>();
            model = _homeService.GetNumberServices(StartDate, EndDate);
            return View(model);
        }
        #endregion

        #region 新增號碼
        [HttpGet]
        public ActionResult AddNumber()
        {
            LotNumber model = new LotNumber();
            model.開獎日期 = DateTime.Now.ToString("yyyyMMdd");
            SetMaxNo();
            return View(model);
        }

        

        [HttpPost]
        public ActionResult AddNumber(LotNumber data)
        {
            SetMaxNo();
            //CheckRepeatFun(data);

            if (ModelState.IsValid)
            {
                if (_homeService.AddNumberServices(data))
                {
                    TempData["message"] = "寫入成功";
                }
                else
                {
                    TempData["message"] = "寫入失敗";
                }
                return RedirectToAction("AddNumber");
            }
            return View();
        }
        #endregion

        #region Datepicer練習
        [HttpGet]
        public ActionResult DatepicerTest()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DatepicerTest(LotNumber data)
        {
            return View();
        }
        #endregion

        #region Datepicer練習2
        [HttpGet]
        public ActionResult DatepicerTest2()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DatepicerTest2(LotNumber data)
        {
            return View();
        }
        #endregion

        #region 通用
        /// <summary>
        /// 取得最大期數+1
        /// </summary>
        private void SetMaxNo()
        {
            ViewBag.MaxNo = (Convert.ToUInt32(_homeService.GetMaxNoServices()) + 1).ToString().PadLeft(8, '0');
        }

        /// <summary>
        /// 判斷號碼是否有重覆
        /// </summary>
        /// <param name="data"></param>
        private void CheckRepeatFun(LotNumber data)
        {
            List<string> checkRepeat = new List<string> { data.號碼1, data.號碼2, data.號碼3, data.號碼4, data.號碼5 };
            //List<string> RepeatData = new List<string>();
            //for (int i = 0; i < checkRepeat.Count; i++)
            //{
            //    for (int j = i + 1; j < checkRepeat.Count; j++)
            //    {
            //        if (checkRepeat[i] == checkRepeat[j])
            //        {
            //            RepeatData.Add(checkRepeat[i]);
            //        }
            //    }
            //}
            //var dd = from p in checkRepeat
            //         group p by p.ToString() into g
            //         where g.Count() > 1//出現1次以上的數字
            //         select g.Key;
            //TempData["message"] = checkRepeat.GroupBy(p=>p).Where(g=>g.Count()>1).Select(m=>m.Key);
            if (checkRepeat.GroupBy(p => p).Count() > 1)
            {
                TempData["message"] = "重覆號碼：" + checkRepeat.GroupBy(p => p).Where(g => g.Count() > 1).Select(m => m.Key);
            }
            else
            {
                TempData["message"] = "無重覆值";
            }
        }
        #endregion

        #region 通用
        /// <summary>
        /// 計算查詢結果的各號碼總計
        /// </summary>
        /// <param name="model"></param>
        /// <param name="numList"></param>
        /// <returns></returns>
        private SelectLotNumber SumNumberCount(SelectLotNumber model, List<LotNumber> numList)
        {
            Dictionary<int?, int?> numDic = new Dictionary<int?, int?>();
            model.selectNumberCountDarry = new int[40];
            foreach (var item in numList)
            {
                model.selectNumberCountDarry[int.Parse(item.號碼1)] = model.selectNumberCountDarry[int.Parse(item.號碼1)] + 1;
                model.selectNumberCountDarry[int.Parse(item.號碼2)] = model.selectNumberCountDarry[int.Parse(item.號碼2)] + 1;
                model.selectNumberCountDarry[int.Parse(item.號碼3)] = model.selectNumberCountDarry[int.Parse(item.號碼3)] + 1;
                model.selectNumberCountDarry[int.Parse(item.號碼4)] = model.selectNumberCountDarry[int.Parse(item.號碼4)] + 1;
                model.selectNumberCountDarry[int.Parse(item.號碼5)] = model.selectNumberCountDarry[int.Parse(item.號碼5)] + 1;
            }
            for (int i = 1; i < 40; i++)
            {
                numDic.Add(i, model.selectNumberCountDarry[i]);
            }
            model.selectNumberCountList = numDic;
            model.selectNumberCountListOrderBy = numDic.OrderByDescending(d => d.Value).ToDictionary(dkey => dkey.Key, dvalue => dvalue.Value);
            return model;
        }
        #endregion

    }
}