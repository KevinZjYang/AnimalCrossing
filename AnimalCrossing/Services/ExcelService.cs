using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalCrossing.Services
{
    public static class ExcelService
    {

        public static string GetCellValue(ICell cell)
        {
            if (cell == null)
            {
                return string.Empty;
            }

            switch (cell.CellType)
            {
                case CellType.Formula:
                case CellType.String:
                    return cell.StringCellValue;

                case CellType.Numeric:
                    if (DateUtil.IsCellDateFormatted(cell))
                    {
                        return cell.DateCellValue.ToString();
                    }
                    else
                    {
                        return cell.NumericCellValue.ToString();
                    }
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();

                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// 读取Excel所有单元格数据
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="sheetName">Sheet名</param>
        /// <param name="startRow">读取开始行位置</param>
        /// <param name="columns">读取列表</param>
        /// <returns>单元格列表</returns>
        public static async Task<IList<ICell>> ReadAllCellsAsync(string path, string sheetName, int startRow = 1, IList<int> columns = null)
        {
            var ret = new List<ICell>();

            await Task.Factory.StartNew(() =>
            {
                using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    var book = WorkbookFactory.Create(fileStream);
                    var sheet = book?.GetSheet(sheetName);
                    if (sheet != null)
                    {
                        for (int row = startRow - 1; row <= sheet.LastRowNum; row++)
                        {
                            var rowValue = sheet.GetRow(row);
                            if (rowValue == null)
                            {
                                continue;
                            }

                            if (columns == null || columns?.Count <= 0)
                            {
                                columns = Enumerable.Range(1, rowValue.LastCellNum + 1).ToList();
                            }

                            foreach (int col in columns)
                            {
                                var cell = rowValue.GetCell(col - 1);
                                if (cell == null)
                                {
                                    continue;
                                }

                                ret.Add(cell);
                            }
                        }
                    }
                    book?.Close();
                }
            });

            return ret;
        }
    }
}
