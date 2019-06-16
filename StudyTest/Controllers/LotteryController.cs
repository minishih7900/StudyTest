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
using PagedList;
using PagedList.Mvc;

namespace StudyTEST.Controllers
{
    public class LotteryController : Controller
    {
        private LotteryServices _lotteryService=new LotteryServices();
        // 單頁可容納之資料筆數(可參數化此數值)
        private const int PageSize = 10;

        // GET: Lottery
        public ActionResult Index()
        {
            return View();
        }

        #region 查詢號碼
        [HttpGet]
        public ActionResult QueryNumber(int page = 1)
        {
            // 進入搜尋頁面 不主動撈取資料
            LotteryViewModel viewModel = new LotteryViewModel();
            // 從資料庫撈資料
            List<LotNumber> DBdata = _lotteryService.GetNumberServices();
            viewModel.LotNumber = DBdata.OrderByDescending(x => x.開獎日期).ToPagedList(page, PageSize);
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult QueryNumber(LotteryViewModel data)
        {
            // 從資料庫撈資料
            List<LotNumber> DBdata = _lotteryService.GetNumberServices();
            data.Page = data.Page == 0 ? 2 : data.Page + 1;

            // 當日期符合開始與結束日期
            if (!string.IsNullOrWhiteSpace(data.StartDate) && !string.IsNullOrWhiteSpace(data.EndDate))
            {
                data.LotNumber = DBdata.Where(p => p.開獎日期.CompareTo(data.StartDate) >= 0 && p.開獎日期.CompareTo(data.EndDate) <= 0).OrderByDescending(m => m.開獎日期).ToPagedList(data.Page > 0 ? data.Page - 1 : 0, PageSize);
            }
            else
            {
                data.LotNumber = DBdata.OrderByDescending(x => x.開獎日期).ToPagedList(data.Page > 0 ? data.Page - 1 : 0, PageSize);
            }
            return View(data);
        }
        #endregion

        #region 查詢號碼不分頁
        [HttpGet]
        public ActionResult QueryNumberNoPage()
        {
            // 進入搜尋頁面 不主動撈取資料
            LotteryViewModel viewModel = new LotteryViewModel();
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult QueryNumberNoPage(LotteryViewModel data)
        {
            // 從資料庫撈資料
            List<LotNumber> DBdata = _lotteryService.GetNumberServices();
            

            // 當日期符合開始與結束日期
            if (!string.IsNullOrWhiteSpace(data.StartDate) && !string.IsNullOrWhiteSpace(data.EndDate))
            {
                data.LotNumberNoPage = DBdata.Where(p => p.開獎日期.CompareTo(data.StartDate) >= 0 && p.開獎日期.CompareTo(data.EndDate) <= 0).OrderBy(m => m.開獎日期).ToList();
            }
            else
            {
                data.LotNumberNoPage = DBdata.OrderBy(x => x.開獎日期).ToList();
            }
            return View(data);
        }
        #endregion

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
        public ActionResult QuerySelectNumberCount(string selectnum, string StartDate, string EndDate, string StartPeriod, string EndPeriod)
        {
            SelectLotNumber model = new SelectLotNumber();
            List<LotNumber> numList = new List<LotNumber>();
            numList = _lotteryService.GetNumberListServices(selectnum, StartDate, EndDate, StartPeriod, EndPeriod);
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
            numList = _lotteryService.GetNumberTopServices(PeriodNum);
            SumNumberCount(model, numList);
            ViewBag.Message = "查詢期間為：" + numList.Select(d => d.開獎日期).Min() + "~" + numList.Select(d => d.開獎日期).Max();
            ViewBag.Data = numList.Count;
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
            TrimStartZone(data);
            if (ModelState.IsValid)
            {
                if (_lotteryService.AddNumberServices(data))
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

        /// <summary>
        /// 取得最大期數+1
        /// </summary>
        private void SetMaxNo()
        {
            ViewBag.MaxNo = (Convert.ToUInt32(_lotteryService.GetMaxNoServices()) + 1).ToString().PadLeft(8, '0');
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

        /// <summary>
        /// 去掉前面0
        /// </summary>
        /// <param name="data"></param>
        private void TrimStartZone(LotNumber data)
        {
            data.號碼1 = data.號碼1.TrimStart('0');
            data.號碼2 = data.號碼2.TrimStart('0');
            data.號碼3 = data.號碼3.TrimStart('0');
            data.號碼4 = data.號碼4.TrimStart('0');
            data.號碼5 = data.號碼5.TrimStart('0');
        }
        #endregion
    }
}