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
    public class HkLot49Services
    {
        public SqlRepository _sqlRepository = new SqlRepository();

        /// <summary>
        /// 依期數TOP查詢筆數
        /// </summary>
        /// <param name="newCount"></param>
        /// <returns></returns>
        public List<LotNumber6> GetNumber6TopServices(string newCount)
        {
            return _sqlRepository.GetLotNumber6NewTop(newCount);
        }
        /// <summary>
        /// 依開始日期結束日期查詢筆數
        /// </summary>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public List<LotNumber6> GetNumberServices(string StartDate, string EndDate)
        {
            return _sqlRepository.GetLotNumber6(StartDate, EndDate);
        }
        public List<LotNumber6> GetNumber6Services()
        {
            return _sqlRepository.GetLotNumber6();
        }
        public List<LotNumber6> GetNumber6ListServices(string selectnum, string StartDate, string EndDate, string StartPeriod, string EndPeriod)
        {
            StartPeriod = StartPeriod.PadLeft(5, '0');
            EndPeriod = EndPeriod.PadLeft(5, '0');
            return _sqlRepository.GetSelectLotNumber6(selectnum, StartDate, EndDate, StartPeriod, EndPeriod);
        }

        public string GetMaxNo6Services()
        {
            return _sqlRepository.GetMaxNo6();
        }
        public bool AddNumber6Services(LotNumber6 data)
        {
            bool success = false;

            using (var scope = new TransactionScope())
            {
                success = _sqlRepository.InputLotNumber6(data) &&
                          _sqlRepository.InsertCopyNumber6(data.開獎日期) &&
                          _sqlRepository.UpdateCopyNumber6(data);
                if (success)
                    scope.Complete();
            }
            return success;
        }

    }
}