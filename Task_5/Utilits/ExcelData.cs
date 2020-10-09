using System;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;
using Task_5.Models;
using Test.Framework.Logging;

namespace Task_5.Utilits
{
    public static class ExcelData
    {
        public static List<TestItem> GetData(string path)
        {
            Log.Info($"Get data from Excel file from {path}.");
            Excel.Application applicationExcel = null;
            List<TestItem> testItems = new List<TestItem>();
            try
            {
                applicationExcel = new Excel.Application();
                Excel.Workbook workbook = applicationExcel.Workbooks.Open(path);
                Excel.Worksheet worksheet = workbook.Sheets[1] as Excel.Worksheet;               
                var lastCell = worksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);
                for (int i = 2; i <= lastCell.Row; i++) {
                    TestItem testItem = new TestItem();
                    for (int j = 1; j <= lastCell.Column; j++)
                    {
                        Excel.Range range = (Excel.Range)worksheet.Cells[i, j];
                        switch (j)
                        {
                            case 1:
                                testItem.User.Login = range.Value2.ToString();
                                break;
                            case 2:
                                testItem.User.Password = range.Value2.ToString();
                                break;
                            case 3:
                                testItem.Product.OsName = range.Value2.ToString();
                                break;
                            case 4:
                                testItem.Product.Name = range.Value2.ToString();
                                break;
                            default:                                
                                Log.Error($"During to write {path} file to get unknown value in cell[{i},{j}].");                                
                                break;
                        }
                    }
                    testItems.Add(testItem);
                }                
                return testItems;
            }
            catch(Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred during getting data from excel file from {path}.");
                return testItems;
            }
            finally
            {
                applicationExcel.Quit();
            }
        }
    }
}
