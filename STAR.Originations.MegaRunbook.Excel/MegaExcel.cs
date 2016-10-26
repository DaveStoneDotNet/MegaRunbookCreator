using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Aspose.Cells.Tables;

using STAR.Enterprise.Trace;

using Cells = Aspose.Cells;

namespace STAR.Originations.MegaRunbook.Excel
{
    public class MegaExcel
    {
        protected TraceSource traceSource = null;
        public TraceSource TraceSource => this.traceSource ?? (this.traceSource = new TraceSource("MegaExcel", SourceLevels.Information));

        #region Constructor

        #region MegaExcel
        public MegaExcel()
        {
            this.InitializeAsposeLicensing();
        }
        #endregion MegaExcel

        #endregion Constructor

        #region InitializeAsposeLicensing
        public void InitializeAsposeLicensing()
        {
            var stopwatch = MegaExcel.StartStopwatch();
            try
            {
                var excellicense = new Cells.License();
                var stream = new MemoryStream(Resources.Resources.Aspose_Total);
                excellicense.SetLicense(stream);
                this.TraceSource.TraceEvent(TraceEventType.Information, "COMPLETE: Aspose Excel Licensed", stopwatch.Elapsed, TraceStatus.Success);
            }
            catch (Exception ex)
            {
                this.TraceSource.TraceException(ex, TraceEventType.Error, "FAILED: Could not get License for Aspose Excel.", stopwatch.Elapsed, TraceStatus.Failed);
            }
        }
        #endregion InitializeAsposeLicensing

        #region SaveExcelFile
        public string SaveExcelFile()
        {
            var stopwatch = MegaExcel.StartStopwatch();

            var excelTemp = @"C:\BACKUPS\TEST_TEMP.xlsx";
            var excelFinal = @"C:\BACKUPS\TEST_FINAL.xlsx";

            try
            {
                using (var fileStream = new FileStream(excelTemp, FileMode.OpenOrCreate))
                {
                    var workbook = new Cells.Workbook(fileStream);
                    var worksheet = workbook.Worksheets[0];

                    // 'rowNumber' represents the current row number in the spreadsheet, starts at ONE, and is continually incremented.

                    //var listObjects = workbook.Worksheets[0].ListObjects;
                    //listObjects.Add(0, 0, 1, 11, true);

                    //var listObject = workbook.Worksheets[0].ListObjects.ElementAt(0);
                    //listObject.TableStyleType = TableStyleType.TableStyleMedium2;

                    //listObject.PutCellValue(1, 0, "A");
                    //listObject.PutCellValue(2, 1, "B");
                    //listObject.PutCellValue(3, 2, "C");

                    // ------------------------------------------------

                    // Adding a string value to the cell
                    worksheet.Cells["A1"].PutValue("Hello World");

                    // Adding a double value to the cell
                    worksheet.Cells["A2"].PutValue(20.5);

                    // Adding an integer value to the cell
                    worksheet.Cells["A3"].PutValue(15);

                    // Adding a boolean value to the cell
                    worksheet.Cells["A4"].PutValue(true);

                    // Adding a date/time value to the cell
                    worksheet.Cells["A5"].PutValue(DateTime.Now);

                    // Setting the display format of the date
                    var style = worksheet.Cells["A5"].GetStyle();
                    style.Number = 15;
                    worksheet.Cells["A5"].SetStyle(style);

                    // ------------------------------------------------

                    //listObject.ApplyStyleToRange();
                    //worksheet.AutoFitColumns();

                    workbook.Save(excelFinal);

                    this.TraceSource.TraceEvent(TraceEventType.Information, "COMPLETE", stopwatch.Elapsed, TraceStatus.Success);
                }
            }
            catch (Exception ex)
            {
                this.TraceSource.TraceException(ex, TraceEventType.Error, "FAILED", stopwatch.Elapsed, TraceStatus.Failed);
            }

            return excelFinal;
        }
        #endregion GetScheduleOfMortgagesExcel

        #region StartStopwatch
        public static Stopwatch StartStopwatch()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            return stopwatch;
        }
        #endregion StartStopwatch

    }
}
