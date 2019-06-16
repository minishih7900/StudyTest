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

namespace StudyTest.Controllers
{
    public class HkLot49Controller : Controller
    {
        private HkLot49Services _HkLot49Services = new HkLot49Services();
        // 單頁可容納之資料筆數(可參數化此數值)
        private const int PageSize = 10;

        // GET: HkLot49
        public ActionResult Index()
        {
            return View();
        }

        #region 查詢號碼
        [HttpGet]
        public ActionResult QueryNumber6(int page = 1)
        {
            // 進入搜尋頁面 不主動撈取資料
            HkLot49ViewModel viewModel = new HkLot49ViewModel();
            // 從資料庫撈資料
            List<LotNumber6> DBdata = _HkLot49Services.GetNumber6Services();
            viewModel.LotNumber6 = DBdata.OrderByDescending(x => x.開獎日期).ToPagedList(page, PageSize);
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult QueryNumber6(HkLot49ViewModel data)
        {
            // 從資料庫撈資料
            List<LotNumber6> DBdata = _HkLot49Services.GetNumber6Services();
            data.Page = data.Page == 0 ? 2 : data.Page + 1;

            // 當日期符合開始與結束日期
            if (!string.IsNullOrWhiteSpace(data.StartDate) && !string.IsNullOrWhiteSpace(data.EndDate))
            {
                data.LotNumber6 = DBdata.Where(p => p.開獎日期.CompareTo(data.StartDate) >= 0 && p.開獎日期.CompareTo(data.EndDate) <= 0).OrderByDescending(m => m.開獎日期).ToPagedList(data.Page > 0 ? data.Page - 1 : 0, PageSize);
            }
            else
            {
                data.LotNumber6 = DBdata.OrderByDescending(x => x.開獎日期).ToPagedList(data.Page > 0 ? data.Page - 1 : 0, PageSize);
            }
            return View(data);
        }
        #endregion

        #region 新增號碼
        [HttpGet]
        public ActionResult AddNumber6()
        {
            LotNumber6 model = new LotNumber6();
            model.開獎日期 = DateTime.Now.ToString("yyyyMMdd");
            SetMaxNo6();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddNumber6(LotNumber6 data)
        {
            SetMaxNo6();
            //CheckRepeatFun(data);
            TrimStartZone(data);
            if (ModelState.IsValid)
            {
                if (_HkLot49Services.AddNumber6Services(data))
                {
                    TempData["message"] = "寫入成功";
                }
                else
                {
                    TempData["message"] = "寫入失敗";
                }
                return RedirectToAction("AddNumber6");
            }
            return View();
        }
        #endregion

        #region 最合數字
        [HttpGet]
        public ActionResult QuerySelectNumber6Count()
        {
            SelectLotNumber6 model = new SelectLotNumber6();
            model.selectNumber6CountDarry = new int[50];
            model.selectNumber6CountListOrderBy = new Dictionary<int?, int?>();
            return View(model);
        }
        [HttpPost]
        public ActionResult QuerySelectNumber6Count(string selectnum, string StartDate, string EndDate, string StartPeriod, string EndPeriod)
        {
            SelectLotNumber6 model = new SelectLotNumber6();
            List<LotNumber6> numList = new List<LotNumber6>();
            numList = _HkLot49Services.GetNumber6ListServices(TrimStartZone(selectnum), StartDate, EndDate, StartPeriod, EndPeriod);
            SumNumber6Count(model, numList);
            ViewBag.Message = "查詢期間為：" + numList.Select(d => d.開獎日期).Min() + "~" + numList.Select(d => d.開獎日期).Max();
            ViewBag.Data = numList.Count;

            return View(model);
        }

        #endregion

        #region 熱門牌
        [HttpGet]
        public ActionResult HotNumber6()
        {
            return View();
        }
        [HttpPost]
        public ActionResult HotNumber6(string PeriodNum)
        {
            SelectLotNumber6 model = new SelectLotNumber6();
            List<LotNumber6> numList = new List<LotNumber6>();
            numList = _HkLot49Services.GetNumber6TopServices(PeriodNum);
            SumNumber6Count(model, numList);
            ViewBag.Message = "查詢期間為：" + numList.Select(d => d.開獎日期).Min() + "~" + numList.Select(d => d.開獎日期).Max();
            ViewBag.Data = numList.Count;
            return View(model);
        }

        #endregion

        #region 通用
        /// <summary>
        /// 計算查詢結果的各號碼總計
        /// </summary>
        /// <param name="model"></param>
        /// <param name="numList"></param>
        /// <returns></returns>
        private SelectLotNumber6 SumNumber6Count(SelectLotNumber6 model, List<LotNumber6> numList)
        {
            Dictionary<int?, int?> numDic = new Dictionary<int?, int?>();
            model.selectNumber6CountDarry = new int[50];
            foreach (var item in numList)
            {
                model.selectNumber6CountDarry[int.Parse(item.號碼1)] = model.selectNumber6CountDarry[int.Parse(item.號碼1)] + 1;
                model.selectNumber6CountDarry[int.Parse(item.號碼2)] = model.selectNumber6CountDarry[int.Parse(item.號碼2)] + 1;
                model.selectNumber6CountDarry[int.Parse(item.號碼3)] = model.selectNumber6CountDarry[int.Parse(item.號碼3)] + 1;
                model.selectNumber6CountDarry[int.Parse(item.號碼4)] = model.selectNumber6CountDarry[int.Parse(item.號碼4)] + 1;
                model.selectNumber6CountDarry[int.Parse(item.號碼5)] = model.selectNumber6CountDarry[int.Parse(item.號碼5)] + 1;
                model.selectNumber6CountDarry[int.Parse(item.號碼6)] = model.selectNumber6CountDarry[int.Parse(item.號碼6)] + 1;
            }
            for (int i = 1; i < 50; i++)
            {
                numDic.Add(i, model.selectNumber6CountDarry[i]);
            }
            model.selectNumber6CountList = numDic;
            model.selectNumber6CountListOrderBy = numDic.OrderByDescending(d => d.Value).ToDictionary(dkey => dkey.Key, dvalue => dvalue.Value);
            return model;
        }

        /// <summary>
        /// 取得最大期數+1
        /// </summary>
        private void SetMaxNo6()
        {
            ViewBag.MaxNo = (Convert.ToUInt32(_HkLot49Services.GetMaxNo6Services()) + 1).ToString().PadLeft(5, '0');
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
        private void TrimStartZone(LotNumber6 data)
        {
            data.號碼1 = data.號碼1.TrimStart('0');
            data.號碼2 = data.號碼2.TrimStart('0');
            data.號碼3 = data.號碼3.TrimStart('0');
            data.號碼4 = data.號碼4.TrimStart('0');
            data.號碼5 = data.號碼5.TrimStart('0');
            data.號碼6 = data.號碼6.TrimStart('0');
            data.特別號 = data.特別號.TrimStart('0');
        }
        private string TrimStartZone(string data)
        {
            data = data.TrimStart('0');
            return data;
        }
        #endregion
    }
}