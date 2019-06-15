using NPOI.SS.Formula.Functions;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;
using Study.Models.Models;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.Globalization;
using NPOI.HSSF.UserModel;
using Xceed.Words.NET;

namespace ExcelTools
{
    public class report
    {
        public string OutputWord(List<LotNumber> data)
        {
            string templateFilePath = Path.Combine(HostingEnvironment.MapPath("~/ExcelTemplate"), "test.docx");
            DocX document = DocX.Load(templateFilePath);
            //foreach (var item in data)
            //{
            //    document.ReplaceText("{{name}}", item.name);
            //    document.ReplaceText("{{date}}", item.date.ToString());
            //    document.ReplaceText("{{age}}", item.age.ToString());
            //}
            //Table t = document.AddTable(data.Count, 3);
            //Table t =  document.Tables[0];
            //for (int i = 1; i <=20; i++)
            //{
            //    t.Rows[i].Cells[0].Paragraphs.First().Append("A");
            //    t.Rows[i].Cells[1].Paragraphs.First().Append("B");
            //    t.Rows[i].Cells[2].Paragraphs.First().Append("C");
            //    t.Rows[i].Cells[3].Paragraphs.First().Append("D");
            //    t.Rows[i].Cells[4].Paragraphs.First().Append("E");
            //    t.Rows[i].Cells[5].Paragraphs.First().Append("E");
            //}
            Table t = document.Tables[0];
            t.Rows[0].Cells[0].Paragraphs.First().Append("月");
            t.Rows[0].Cells[1].Paragraphs.First().Append("日");
            t.Rows[0].Cells[2].Paragraphs.First().Append("星");
            t.Rows[0].Cells[3].Paragraphs.First().Append("1");
            t.Rows[0].Cells[4].Paragraphs.First().Append("2");
            t.Rows[0].Cells[5].Paragraphs.First().Append("3");
            t.Rows[0].Cells[6].Paragraphs.First().Append("4");
            t.Rows[0].Cells[7].Paragraphs.First().Append("5");
            var monthData = "";

            foreach (var item in data)
            {
                var i = data.IndexOf(item)+1;
                DateTime NewDate = DateTime.ParseExact(item.開獎日期, "yyyyMMdd", null, DateTimeStyles.AllowWhiteSpaces);
                var lbDay = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(NewDate.DayOfWeek);
                if (monthData != item.開獎日期.Substring(4,2))
                {
                    monthData = item.開獎日期.Substring(4, 2);
                    t.Rows[i].Cells[0].Paragraphs.First().Append(monthData);
                }
                t.Rows[i].Cells[1].Paragraphs.First().Append(item.開獎日期.Substring(6,2));
                t.Rows[i].Cells[2].Paragraphs.First().Append(lbDay.Substring(2,1));
                t.Rows[i].Cells[3].Paragraphs.First().Append(item.號碼1);
                t.Rows[i].Cells[4].Paragraphs.First().Append(item.號碼2);
                t.Rows[i].Cells[5].Paragraphs.First().Append(item.號碼3);
                t.Rows[i].Cells[6].Paragraphs.First().Append(item.號碼4);
                t.Rows[i].Cells[7].Paragraphs.First().Append(item.號碼5);
            }

            string filePath = @"C:\temp\word";

            //建folder
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            //新完整檔名
            string newFileName = "word.docx";
            //檔案路徑 + 新完整檔名
            string fullFilePath = Path.Combine(filePath, newFileName);

            FileStream file = new FileStream(fullFilePath, FileMode.Create);//產生檔案
            document.SaveAs(file);
            file.Close();
            GC.Collect();
            return fullFilePath;
        }
        public string ExciseRpt(List<ExciseFreeApply> data)
        {
            string templateFilePath = Path.Combine(HostingEnvironment.MapPath("~/ExcelTemplate"), "ExciseFreeApplyTemplate.xlsx");

            // Open Template
            FileStream fs = new FileStream(templateFilePath, FileMode.Open, FileAccess.Read);

            // Load the template into a NPOI workbook
            XSSFWorkbook templateWorkbook = new XSSFWorkbook(fs);

            // Load the sheet you are going to use as a template into NPOI
            XSSFSheet sheet = (XSSFSheet)templateWorkbook.GetSheetAt(0);

            // 建立新頁籤並命名Rpt1
            templateWorkbook.CreateSheet("Rpt1");

            XSSFSheet sheet2 = (XSSFSheet)templateWorkbook.GetSheetAt(1);

            //寬度格式設定
            int[] ColWidthSetArr = new int[] { 1,13,32,4,7,
                                               9,10,1,11,1
                                              };
            SetSheetColumnWidth(ref sheet2, ColWidthSetArr);

            //設定將所有欄放入單一頁面中
            sheet2.FitToPage = true;
            sheet2.PrintSetup.FitWidth = 1;
            sheet2.PrintSetup.FitHeight = 0;

            //設定列印格式為A4(A3=8,A4=9,Letter=1)
            sheet2.PrintSetup.PaperSize = 9;

            //基本參數
            int originalSheetLastRow = sheet.LastRowNum;
            int headerRowLen = 7;
            int templateBodyRow = 7;
            int footerRowStart = 14;
            int footerRowLen = originalSheetLastRow - footerRowStart;
            //欄位資訊
            int newSheetRowLen = headerRowLen + data.Count + (sheet.LastRowNum - footerRowStart);
            int newSheetPageNum = Convert.ToInt16(Math.Ceiling(newSheetRowLen / 44.0));

            var tempdata = new ExciseFreeApply();
            tempdata.ID = data.Count == 0 ? "" : data[0].ID;
            tempdata.Name = data.Count == 0 ? "" : data[0].Name;
            tempdata.Address = data.Count == 0 ? "" : data[0].Address;

            //表頭
            ReplaceRowCellValue(ref sheet, 5, 1, "#0", tempdata.ID);
            //表尾
            ReplaceRowCellValue(ref sheet, 17, 4, "#1", tempdata.Name);
            ReplaceRowCellValue(ref sheet, 18, 4, "#2", tempdata.Address);

            for (var i = 0; i < headerRowLen; i++)
            {
                CopySheetRow(ref sheet, ref sheet2, i, i, false, false, true, false);
            }
            int sheet2Last = sheet2.LastRowNum + 1;

            for (var i = 0; i < data.Count; i++)
            {
                if (i == 0)
                {
                    SetRowCellValue(ref sheet, templateBodyRow, 1, data[i].ProdTaxNumber);
                    SetRowCellValue(ref sheet, templateBodyRow, 2, data[i].ProdEngName + data[i].ProdChName);
                    SetRowCellValue(ref sheet, templateBodyRow, 3, data[i].TaxUnits);
                    SetRowCellValue(ref sheet, templateBodyRow, 4, (double)data[i].Qty);
                    SetRowCellValue(ref sheet, templateBodyRow, 5, ToSimpleTaiwanDate(data[i].ProcessDate));
                    SetRowCellValue(ref sheet, templateBodyRow, 6, data[i].SheetNumber);
                    SetRowCellValue(ref sheet, templateBodyRow, 7, data[i].Mode);
                    CopySheetRow(ref sheet, ref sheet2, templateBodyRow, sheet2Last + i, false, false, true, true);
                }
                else
                {
                    SetRowCellValue(ref sheet, templateBodyRow + 1, 1, data[i].ProdTaxNumber);
                    SetRowCellValue(ref sheet, templateBodyRow + 1, 2, data[i].ProdEngName + data[i].ProdChName);
                    SetRowCellValue(ref sheet, templateBodyRow + 1, 3, data[i].TaxUnits);
                    SetRowCellValue(ref sheet, templateBodyRow + 1, 4, (double)data[i].Qty);
                    SetRowCellValue(ref sheet, templateBodyRow + 1, 5, ToSimpleTaiwanDate(data[i].ProcessDate));
                    SetRowCellValue(ref sheet, templateBodyRow + 1, 6, data[i].SheetNumber);
                    SetRowCellValue(ref sheet, templateBodyRow + 1, 7, data[i].Mode);
                    CopySheetRow(ref sheet, ref sheet2, templateBodyRow + 1, sheet2Last + i, false, false, false, false);
                }

            }
            sheet2.CreateRow(sheet2.LastRowNum + 1);
            sheet2.CopyRow(sheet2.LastRowNum-1, sheet2.LastRowNum + 1);

            



            sheet2Last = sheet2.LastRowNum + 1;
            for (var i = 0; i < footerRowLen; i++)
            {
                CopySheetRow(ref sheet, ref sheet2, footerRowStart + i, sheet2Last + i, false, false, true, false);
            }

            //自動調整欄位寬度，此欄位為數量
            sheet2.AutoSizeColumn(4);

            //合併儲存格
            if (data.Count > 0)
            {
                RemoveMergeCells(ref sheet2, headerRowLen);
                sheet2.AddMergedRegion(new CellRangeAddress(headerRowLen, sheet2.LastRowNum - footerRowLen, 8, 8));
            }

            string filePath = @"C:\temp\excel";

            //建folder
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            //新完整檔名
            string newFileName = "export.xlsx";
            //檔案路徑 + 新完整檔名
            string fullFilePath = Path.Combine(filePath, newFileName);

            FileStream file = new FileStream(fullFilePath, FileMode.Create);//產生檔案
            templateWorkbook.Write(file);
            file.Close();
            GC.Collect();
            return fullFilePath;
        }


        /// <summary>
        /// 設定欄寬
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="colWidthArr"></param>
        private void SetSheetColumnWidth(ref XSSFSheet worksheet, int[] colWidthArr)
        {
            for (var i = 0; i < colWidthArr.Length; i++)
            {
                worksheet.SetColumnWidth(i, (int)((colWidthArr[i] + 0.72) * 256));
            }
        }
        /// <summary>
        /// 取代指定Row、Cell內容值
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="rowNum"></param>
        /// <param name="cellNum"></param>
        /// <param name="replaceKey"></param>
        /// <param name="val"></param>
        private void ReplaceRowCellValue(ref XSSFSheet worksheet, int rowNum, int cellNum, string replaceKey, string val)
        {
            var tempVal = worksheet.GetRow(rowNum).GetCell(cellNum).StringCellValue.Replace(replaceKey, val);
            SetRowCellValue(ref worksheet, rowNum, cellNum, tempVal);
        }
        /// <summary>
        /// 設定指定Row、Cell內容
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="rowNum"></param>
        /// <param name="cellNum"></param>
        /// <param name="val"></param>
        private void SetRowCellValue(ref XSSFSheet worksheet, int rowNum, int cellNum, string val)
        {
            worksheet.GetRow(rowNum).GetCell(cellNum).SetCellValue(val);
        }
        // <summary>
        /// 設定指定Row、Cell內容、值為數字時
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="rowNum"></param>
        /// <param name="cellNum"></param>
        /// <param name="val"></param>
        private void SetRowCellValue(ref XSSFSheet worksheet, int rowNum, int cellNum, double val)
        {
            worksheet.GetRow(rowNum).GetCell(cellNum).SetCellValue(val);
        }
        /// <summary>
        /// 分頁Row拷貝
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="worksheet2"></param>
        /// <param name="sourceRowNum"></param>
        /// <param name="destinationRowNum"></param>
        /// <param name="IsCoverRow"></param>
        /// <param name="IsRemoveSrcRow"></param>
        /// <param name="copyRowHeight"></param>
        /// <param name="resetOriginalRowHeight"></param>
        private void CopySheetRow(ref XSSFSheet worksheet, ref XSSFSheet worksheet2, int sourceRowNum, int destinationRowNum, bool IsCoverRow = false, bool IsRemoveSrcRow = false, bool copyRowHeight = true, bool resetOriginalRowHeight = true)
        {
            XSSFRow newRow = worksheet2.GetRow(destinationRowNum) as XSSFRow;
            XSSFRow sourceRow = worksheet.GetRow(sourceRowNum) as XSSFRow;
            XSSFCell oldCell, newCell;
            int i;

            if (newRow == null)
                newRow = worksheet2.CreateRow(destinationRowNum) as XSSFRow;

            // Loop through source columns to add to new row
            for (i = 0; i < sourceRow.LastCellNum; i++)
            {
                // Grab a copy of the old/new cell
                oldCell = sourceRow.GetCell(i) as XSSFCell;
                newCell = newRow.GetCell(i) as XSSFCell;

                if (newCell == null)
                    newCell = newRow.CreateCell(i) as XSSFCell;

                // If the old cell is null jump to next cell
                if (oldCell == null)
                {
                    newCell = null;
                    continue;
                }

                // Copy style from old cell and apply to new cell
                newCell.CellStyle = oldCell.CellStyle;

                // If there is a cell comment, copy
                if (newCell.CellComment != null) newCell.CellComment = oldCell.CellComment;

                // If there is a cell hyperlink, copy
                if (oldCell.Hyperlink != null) newCell.Hyperlink = oldCell.Hyperlink;

                // Set the cell data value
                switch (oldCell.CellType)
                {
                    case CellType.Blank:
                        newCell.SetCellValue(oldCell.StringCellValue);
                        break;

                    case CellType.Boolean:
                        newCell.SetCellValue(oldCell.BooleanCellValue);
                        break;

                    case CellType.Error:
                        newCell.SetCellErrorValue(oldCell.ErrorCellValue);
                        break;

                    case CellType.Formula:
                        newCell.CellFormula = oldCell.CellFormula;
                        break;

                    case CellType.Numeric:
                        newCell.SetCellValue(oldCell.NumericCellValue);
                        break;

                    case CellType.String:
                        newCell.SetCellValue(oldCell.RichStringCellValue);
                        break;

                    case CellType.Unknown:
                        newCell.SetCellValue(oldCell.StringCellValue);
                        break;
                }
            }

            // If there are are any merged regions in the source row, copy to new row
            CellRangeAddress cellRangeAddress = null, newCellRangeAddress = null;
            for (i = 0; i < worksheet.NumMergedRegions; i++)
            {
                cellRangeAddress = worksheet.GetMergedRegion(i);
                if (cellRangeAddress.FirstRow == sourceRow.RowNum)
                {
                    newCellRangeAddress = new CellRangeAddress(newRow.RowNum,
                                                                                (newRow.RowNum +
                                                                                 (cellRangeAddress.LastRow -
                                                                                  cellRangeAddress.FirstRow)),
                                                                                cellRangeAddress.FirstColumn,
                                                                                cellRangeAddress.LastColumn);
                    worksheet2.AddMergedRegion(newCellRangeAddress);
                }
            }

            //複製行高到新列
            if (copyRowHeight)
                newRow.Height = sourceRow.Height;
            //重製原始列行高
            if (resetOriginalRowHeight)
                sourceRow.Height = worksheet.DefaultRowHeight;
            //清掉原列
            if (IsRemoveSrcRow == true)
                worksheet.RemoveRow(sourceRow);

           
        }
        /// <summary>
        /// 移除合併儲存格
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="rowIndex"></param>
        private void RemoveMergeCells(ref XSSFSheet worksheet, int rowIndex)
        {
            for (int i = 0; i < worksheet.NumMergedRegions; i++)
            {
                CellRangeAddress cellRangeAddress = worksheet.GetMergedRegion(i);
                if (cellRangeAddress.FirstRow == rowIndex)
                {
                    worksheet.RemoveMergedRegion(i);
                }
            }
        }

        /// <summary>
        /// 轉民國年
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public string ToSimpleTaiwanDate(DateTime datetime)
        {
            if (datetime == DateTime.MinValue)
                return " ";
            return string.Format("{0}/{1}/{2}", TaiwanDateYear(datetime), TaiwanDateMonther(datetime), TaiwanDateDays(datetime));
        }
        /// <summary>
        /// 取得民國年
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>         
        public string TaiwanDateYear(DateTime datetime)
        {
            if (datetime == DateTime.MinValue)
                return " ";
            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();
            return string.Format("{0:000}", taiwanCalendar.GetYear(datetime));
        }
        /// <summary>
        /// 取得民國月
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public string TaiwanDateMonther(DateTime datetime)
        {
            if (datetime == DateTime.MinValue)
                return " ";
            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();
            return string.Format("{0:00}", taiwanCalendar.GetMonth(datetime));
        }
        /// <summary>
        /// 取得民國日
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public string TaiwanDateDays(DateTime datetime)
        {
            if (datetime == DateTime.MinValue)
                return " ";
            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();
            return string.Format("{0:00}", taiwanCalendar.GetDayOfMonth(datetime));
        }
    }
}
