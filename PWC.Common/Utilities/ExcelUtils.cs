using PWC.Common.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PWC.Common.Utilities
{
    public static class ExcelUtils
    {
        private static ILogger log = ApplicationLogging.CreateLogger("ExcelUtils");

        #region Constants

        private const string RANGE = "Range";
        private const string A1 = "a1";
        private const string EXCEL_CONTENT_TYPE = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private const string EXCEL_CONTENT_DISPOSITION = "content-disposition";
        private const string ATTACHMENT_FILENAME = "attachment;  filename=";
        private const string EXCEL_EXTENSION = ".xlsx";
        private const string ID = "ID";

        #endregion
        /// <summary>
        /// get data from excel file stream and return datatable structur
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="hasHeader"></param>
        /// <returns></returns>
        public static DataTable ImportFromExcel(Stream stream, bool hasHeader = true, int sheetNumber = 1)
        {
            DataTable table = new DataTable();
            try
            {
                if (stream != null)
                {
                    using (var pck = new OfficeOpenXml.ExcelPackage())
                    {
                        pck.Load(stream);

                        var workSheet = pck.Workbook.Worksheets[sheetNumber];
                        if (workSheet != null)
                        {
                            foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
                            {
                                table.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                            }

                            var startRow = hasHeader ? 2 : 1;

                            for (var rowNum = startRow; rowNum <= workSheet.Dimension.End.Row; rowNum++)
                            {
                                System.Diagnostics.Trace.WriteLine(rowNum);

                                var wsRow = workSheet.Cells[rowNum, 1, rowNum, workSheet.Dimension.End.Column];
                                var row = table.NewRow();
                                foreach (var cell in wsRow)
                                {
                                    if (cell.Start.Column <= row.ItemArray.Length)
                                        row[cell.Start.Column - 1] = cell.Text;
                                }
                                table.Rows.Add(row);
                            }
                        }
                        else
                        {
                            table = null;
                        }
                    }
                }
                else
                {
                    table = null;
                }
            }
            catch (Exception ex)
            {
                table = null;
                log.LogTrace(ex, "Exception on ImportFromExcel Method: ");
            }
            return table;
        }

        public static DataTable ImportFromExcel(string path, bool hasHeader = true, int sheetNumber = 1)
        {
            DataTable table = new DataTable();
            try
            {
                if (!string.IsNullOrEmpty(path))
                {
                    var stream = File.OpenRead(path);
                    table = ImportFromExcel(stream, hasHeader, sheetNumber);
                    stream.Close();
                }
                else
                {
                    table = null;
                }
            }
            catch (Exception ex)
            {
                table = null;
                log.LogTrace(ex, "Exception on ImportFromExcel Method: ");
            }
            return table;
        }

        public static void ExportToExcel(DataTable dataTable, string sheetName, string fileName)
        {
            if (dataTable != null && !string.IsNullOrWhiteSpace(sheetName) && !string.IsNullOrWhiteSpace(fileName))
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet sheet;
                    ExcelNamedRange range;
                    dataTable.TableName = sheetName;

                    sheet = package.Workbook.Worksheets.Add(dataTable.TableName);
                    range = new ExcelNamedRange(RANGE, null, sheet, A1, 0);
                    range.LoadFromDataTable(dataTable, true);
                    range.Style.ReadingOrder = OfficeOpenXml.Style.ExcelReadingOrder.RightToLeft;
                    sheet.Row(1).Style.Font.Bold = true;
                    sheet.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    sheet.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);

                    for (int c = 1; c <= dataTable.Columns.Count; c++)
                    {
                        sheet.Column(c).AutoFit();
                    }

                    HttpContext ctx = SessionHelper.GetHttpContext();
                    HttpResponse response = ctx.Response;
                    response.Clear();
                    response.ContentType = EXCEL_CONTENT_TYPE;
                    response.Headers.Add(EXCEL_CONTENT_DISPOSITION, ATTACHMENT_FILENAME + fileName + EXCEL_EXTENSION);
                    byte[] packageAsByte = package.GetAsByteArray();
                    response.Body.Write(packageAsByte, 0, packageAsByte.Length);
                    response.Body.Flush();
                }
            }
        }

        public static DataTable ConvertListToDataTable<T>(List<T> items, bool CheckID = true, List<string> FieldsToHide = null)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            FieldsToHide = FieldsToHide == null ? new List<string>() : FieldsToHide;
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                if (FieldsToHide != null && !FieldsToHide.Contains(prop.Name))
                {
                    if (!prop.Name.Contains(ID) || !CheckID)
                    {
                        if (prop.PropertyType == typeof(int))
                            dataTable.Columns.Add(prop.Name, typeof(int));
                        else
                            dataTable.Columns.Add(prop.Name);
                    }
                }
            }

            foreach (T item in items)
            {
                var values = new object[dataTable.Columns.Count];
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    for (int j = 0; j < Props.Length; j++)
                    {
                        //inserting property values to datatable rows
                        if (dataTable.Columns[i].ColumnName == Props[j].Name)
                        {
                            values[i] = Props[j].GetValue(item, null);
                            break;
                        }
                    }
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static DataTable ConvertQueryResultToDataTable(List<List<string>> items, List<string> columns)
        {
            DataTable dataTable = new DataTable(typeof(List<string>).Name);
            foreach (var item in columns)
            {
                dataTable.Columns.Add(item);
            }

            foreach (var item in items)
            {
                var values = new object[dataTable.Columns.Count];
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    values[i] = item[i];
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static void ExportToExcelWithHeder(DataTable dataTable, string sheetName, string fileName, bool isRtl = false, bool downloadEmpty = false)
        {
            if ((downloadEmpty || dataTable != null) && !string.IsNullOrWhiteSpace(sheetName) && !string.IsNullOrWhiteSpace(fileName))
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet sheet;
                    ExcelNamedRange range;

                    if (dataTable != null)
                    {
                        dataTable.TableName = sheetName;
                        sheet = package.Workbook.Worksheets.Add(dataTable.TableName);

                    }
                    else
                    {
                        sheet = package.Workbook.Worksheets.Add(sheetName);
                    }
                    range = new ExcelNamedRange(RANGE, null, sheet, A1, 0);
                    if (dataTable != null)
                    {
                        range.LoadFromDataTable(dataTable, true);
                    }
                    sheet.Row(1).Style.Font.Bold = true;
                    sheet.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    sheet.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);

                    if (isRtl)
                    {
                        sheet.Row(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        sheet.View.RightToLeft = true;
                    }
                    if (dataTable != null)
                    {
                        for (int c = 1; c <= dataTable.Columns.Count; c++)
                        {
                            sheet.Column(c).AutoFit();
                        }
                    }

                    //Write it back to the client
                    HttpContext ctx = SessionHelper.GetHttpContext();
                    HttpResponse response = ctx.Response;
                    response.Clear();
                    response.ContentType = EXCEL_CONTENT_TYPE;
                    response.Headers.Add(EXCEL_CONTENT_DISPOSITION, ATTACHMENT_FILENAME + fileName + EXCEL_EXTENSION);
                    byte[] packageAsByte = package.GetAsByteArray();
                    response.Body.Write(packageAsByte, 0, packageAsByte.Length);
                    response.Body.Flush();
                }
            }
        }

        public static void ExportUsageReportToExcel(DataTable oDataTable, DataTable oHeaderDataTable, string appFloatDirection, string fileName, string sheetName, bool ExportEmpty = false)
        {

            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet sheet;
                ExcelNamedRange range;
                oDataTable.TableName = sheetName;
                sheet = package.Workbook.Worksheets.Add(oDataTable.TableName);
                range = new ExcelNamedRange("Range", null, sheet, A1, 0);

                if (oDataTable != null && (oDataTable.Rows.Count >= 1 || ExportEmpty))
                {
                    range.LoadFromDataTable(oHeaderDataTable, false);
                    for (int i = 1; i <= oHeaderDataTable.Rows.Count; i++)
                    {
                        sheet.Row(i).Style.Font.Bold = true;
                        sheet.Row(i).Style.HorizontalAlignment = "en" == appFloatDirection ? OfficeOpenXml.Style.ExcelHorizontalAlignment.Left : OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    }

                    int bodyStart = oHeaderDataTable.Rows.Count + 1;
                    range = new ExcelNamedRange("RangeBody", null, sheet, "a" + bodyStart, 0);

                    range.LoadFromDataTable(oDataTable, true);
                    sheet.Row(bodyStart).Style.Font.Bold = true;
                    sheet.Row(bodyStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    sheet.Row(bodyStart).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);

                    if (appFloatDirection == "ar")
                    {
                        sheet.Row(bodyStart).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        sheet.View.RightToLeft = true;
                    }

                    for (int c = 1; c <= oDataTable.Columns.Count; c++)
                    {
                        sheet.Column(c).AutoFit();
                    }
                }

                HttpContext ctx = SessionHelper.GetHttpContext();
                HttpResponse response = ctx.Response;
                response.Clear();
                response.ContentType = EXCEL_CONTENT_TYPE;
                response.Headers.Add(EXCEL_CONTENT_DISPOSITION, ATTACHMENT_FILENAME + fileName + EXCEL_EXTENSION);
                byte[] packageAsByte = package.GetAsByteArray();
                response.Body.Write(packageAsByte, 0, packageAsByte.Length);
                response.Body.Flush();
            }
        }
    }
}
