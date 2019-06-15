using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Study.DataAccess;
using Study.Models.Models;
using System.Transactions;
using System.Globalization;

namespace Study.Services
{
    public class LotteryServices
    {
        public SqlRepository _sqlRepository = new SqlRepository();

        /// <summary>
        /// 依期數TOP查詢筆數
        /// </summary>
        /// <param name="newCount"></param>
        /// <returns></returns>
        public List<LotNumber> GetNumberTopServices(string newCount)
        {
            return _sqlRepository.GetLotNumberNewTop(newCount);
        }
        /// <summary>
        /// 依開始日期結束日期查詢筆數
        /// </summary>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public List<LotNumber> GetNumberServices(string StartDate, string EndDate)
        {
            return _sqlRepository.GetLotNumber(StartDate, EndDate);
        }
        public List<LotNumber> GetNumberServices()
        {
            return _sqlRepository.GetLotNumber();
        }
        public List<LotNumber> GetNumberListServices(string selectnum, string StartDate, string EndDate, string StartPeriod, string EndPeriod)
        {
            StartPeriod = StartPeriod.PadLeft(8, '0');
            EndPeriod = EndPeriod.PadLeft(8, '0');
            return _sqlRepository.GetSelectLotNumber(selectnum, StartDate, EndDate, StartPeriod, EndPeriod);
        }

        public string GetMaxNoServices()
        {
            return _sqlRepository.GetMaxNo();
        }
        public bool AddNumberServices(LotNumber data)
        {
            bool success = false;
            List<string> inputNum = new List<string> { data.號碼1, data.號碼2, data.號碼3, data.號碼4, data.號碼5 };

            using (var scope = new TransactionScope())
            {
                success = _sqlRepository.InputLotNumber(data) &&
                          _sqlRepository.InsertCopyNumber(data.開獎日期) && 
                          _sqlRepository.UpdateCopyNumber(data);
                if (success)
                    scope.Complete();
            }
            return success;
        }
    }
}
