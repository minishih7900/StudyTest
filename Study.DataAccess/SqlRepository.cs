using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Study.Models.Models;


namespace Study.DataAccess
{
    public class SqlRepository:BaseRepository
    {
        public bool insertMember (List<member> mm)
        {
            var sql = @"INSERT INTO [dbo].[member]
           ([email]
           ,[password])
             VALUES
           (@email,
           @password)";
            return _dbDapper.NonQuerySQL(sql, mm) > 0;
        }
        public List<LotNumber> GetSelectLotNumber(string selectnum, string StartDate, string EndDate, string StartPeriod, string EndPeriod)
        {
            var sqlTemp = "";
            var sql = @"
select * from [dbo].[TwLot59]
where (號碼1=@num or 號碼2=@num or  號碼3=@num or 號碼4=@num or 號碼4=@num) {0}
";
            if (!string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate))
            {
                sqlTemp= sqlTemp + "and (開獎日期 between @startdate and @enddate) ";
            }
            if (StartPeriod !="00000000" && EndPeriod != "00000000")
            {
                sqlTemp = sqlTemp + "and 期數 between @startperiod and @endperiod ";
            }
            sql = string.Format(sql, sqlTemp);

            var param = new Dictionary<string, object>();
            param.Add("num", selectnum);
            param.Add("startdate", StartDate);
            param.Add("enddate", EndDate);
            param.Add("startperiod", StartPeriod);
            param.Add("endperiod", EndPeriod);

            return _dbDapper.QueryList<LotNumber>(sql, param);
        }
        public List<LotNumber> GetLotNumber()
        {
            var sql = @"
SELECT           *
FROM              TwLot59
";
            return _dbDapper.QueryList<LotNumber>(sql,null);
        }
        public List<LotNumber> GetLotNumber(string startDate ,string endDate)
        {
            var sql = @"
SELECT           *
FROM              TwLot59
WHERE          (開獎日期 BETWEEN @StartDate AND @EndDate)
";
            return _dbDapper.QueryList<LotNumber>(sql,new { StartDate=startDate,EndDate=endDate});
        }
        public List<LotNumber> GetLotNumberNewTop(string newCount)
        {
            var sql = @"
SELECT   Top " + newCount  + @"*
FROM              TwLot59
order by 期數 desc";
            return _dbDapper.QueryList<LotNumber>(sql, null);
        }
        public bool InputLotNumber(LotNumber data)
        {
            var sql = @"
INSERT INTO [dbo].[TwLot59]
           ([期數]
           ,[開獎日期]
           ,[號碼1]
           ,[號碼2]
           ,[號碼3]
           ,[號碼4]
           ,[號碼5])
     VALUES
           (@期數
           ,@開獎日期
           ,@號碼1
           ,@號碼2
           ,@號碼3
           ,@號碼4
           ,@號碼5)
";
            return _dbDapper.NonQuerySQL(sql, data) > 0;
        }

        public string GetMaxNo()
        {
            var sql = @"select max(期數) from [dbo].[TwLot59]";
            return _dbDapper.ExecuteScalarSQL<string>(sql,null);
        }
        public bool InsertCopyNumber(string data)
        {
            var sql = @"
            IF(NOT EXISTS(SELECT top 1 * FROM [dbo].[TwLot59_StoredCount] ))
BEGIN
	 insert into [dbo].[TwLot59_StoredCount]
	(日期,[1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13]
      ,[14],[15],[16],[17],[18],[19],[20],[21],[22],[23],[24],[25],[26]
      ,[27],[28],[29],[30],[31],[32],[33],[34],[35],[36],[37],[38],[39])
	  VALUES
	  ( @data,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0)
END	
    ELSE
BEGIN
    insert into [dbo].[TwLot59_StoredCount]
SELECT TOP (1) @data
      ,[1]+1 as [1]
      ,[2]+1 as [2]
      ,[3]+1 as [3]
      ,[4]+1 as [4]
      ,[5]+1 as [5]
      ,[6]+1 as [6]
      ,[7]+1 as [7]
      ,[8]+1 as [8]
      ,[9]+1 as [9]
      ,[10]+1 as [10]
      ,[11]+1 as [11]
      ,[12]+1 as [12]
      ,[13]+1 as [13]
      ,[14]+1 as [14]
      ,[15]+1 as [15]
      ,[16]+1 as [16]
      ,[17]+1 as [17]
      ,[18]+1 as [18]
      ,[19]+1 as [19]
      ,[20]+1 as [20]
      ,[21]+1 as [21]
      ,[22]+1 as [22]
      ,[23]+1 as [23]
      ,[24]+1 as [24]
      ,[25]+1 as [25]
      ,[26]+1 as [26]
      ,[27]+1 as [27]
      ,[28]+1 as [28]
      ,[29]+1 as [29]
      ,[30]+1 as [30]
      ,[31]+1 as [31]
      ,[32]+1 as [32]
      ,[33]+1 as [33]
      ,[34]+1 as [34]
      ,[35]+1 as [35]
      ,[36]+1 as [36]
      ,[37]+1 as [37]
      ,[38]+1 as [38]
      ,[39]+1 as [39]
       FROM [MyDB].[dbo].[TwLot59_StoredCount] order by 日期 desc
END";
            var param = new Dictionary<string, object>();
            param.Add("data", data);
            
            return _dbDapper.NonQuerySQL(sql, param) > 0;
        }

        public bool UpdateCopyNumber(LotNumber data)
        {
            var sql = @"
            DECLARE @TSQL NVARCHAR(4000)
SET @TSQL =	'update [dbo].[TwLot59_StoredCount]
set ' + @num1 + '=0,' + @num2 + '=0,' + @num3 +'=0,' + @num4 + '=0,' + @num5 + '=0
where 日期=' + @date
EXEC SP_EXECUTESQL @TSQL";
            var param = new Dictionary<string, object>();
            param.Add("num1", "[" + data.號碼1 + "]");
            param.Add("num2", "[" + data.號碼2 + "]");
            param.Add("num3", "[" + data.號碼3 + "]");
            param.Add("num4", "[" + data.號碼4 + "]");
            param.Add("num5", "[" + data.號碼5 + "]");
            param.Add("date", data.開獎日期);

            return _dbDapper.NonQuerySQL(sql, param) > 0;
        }
    }
}
