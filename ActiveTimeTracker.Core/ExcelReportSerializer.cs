using System;
using System.IO;
using ActiveTimeTracker.Resources;
using ActivityTimeTracker.Contracts;
using ActivityTimeTracker.Contracts.Data;
using JetBrains.Annotations;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Scar.Common.IO;

namespace ActiveTimeTracker.Core
{
    [UsedImplicitly]
    internal sealed class ExcelReportSerializer : IReportSerializer
    {
        [NotNull]
        private static readonly string ReportsPath = Path.Combine(CommonPaths.SettingsPath, "Reports");

        public string SerializeReport(ActivityReport report)
        {
            if (report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }

            var reportDirectoryPath = Path.Combine(ReportsPath, report.ReportDate.Year.ToString(), report.ReportDate.Month.ToString());
            if (!Directory.Exists(reportDirectoryPath))
            {
                Directory.CreateDirectory(reportDirectoryPath);
            }

            var reportPath = Path.Combine(reportDirectoryPath, report.ReportDate.ToString("yyyy-MM-dd") + ".xlsx");

            using (var stream = new FileStream(reportPath, FileMode.Create, FileAccess.Write))
            {
                var wb = new XSSFWorkbook();
                var timeCellStyle = CreateTimeCellStyle(wb);
                var boldCellStyle = CreateBoldStyle(wb);

                var sheet = wb.CreateSheet(Texts.WorkActivity);
                CreateHeader(sheet, boldCellStyle);

                var i = 1;
                foreach (var item in report.Items)
                {
                    CreateRow(report, sheet, i++, item, timeCellStyle);
                }

                CreateSummary(report, sheet, i, boldCellStyle);

                wb.Write(stream);
            }

            return reportPath;
        }

        [NotNull]
        private static ICellStyle CreateBoldStyle([NotNull] IWorkbook wb)
        {
            var font = wb.CreateFont();
            font.FontHeightInPoints = 11;
            font.FontName = "Calibri";
            font.Boldweight = (short)FontBoldWeight.Bold;
            var boldStyle = wb.CreateCellStyle();
            boldStyle.SetFont(font);
            return boldStyle;
        }

        [NotNull]
        private static ICellStyle CreateTimeCellStyle([NotNull] IWorkbook wb)
        {
            var timeCellStyle = wb.CreateCellStyle();
            var format = wb.CreateDataFormat();
            timeCellStyle.DataFormat = format.GetFormat("HH:mm:ss");
            return timeCellStyle;
        }

        private static void CreateSummary([NotNull] ActivityReport report, [NotNull] ISheet sheet, int i, [NotNull] ICellStyle boldCellStyle)
        {
            var row = sheet.CreateRow(i + 1);
            var cell = row.CreateCell(0);
            cell.SetCellValue(Texts.TotalWorking);
            cell.CellStyle = boldCellStyle;

            cell = row.CreateCell(1);
            cell.SetCellValue(report.TotalWorkingTime.ToPrettyFormat());
            cell.CellStyle = boldCellStyle;
            AutoSizeRow(row, sheet);

            row = sheet.CreateRow(i + 2);
            cell = row.CreateCell(0);
            cell.SetCellValue(Texts.TotalLeisure);
            cell.CellStyle = boldCellStyle;

            cell = row.CreateCell(1);
            cell.SetCellValue(report.TotalLeisureTime.ToPrettyFormat());
            cell.CellStyle = boldCellStyle;
            AutoSizeRow(row, sheet);
        }

        private static void CreateRow([NotNull] ActivityReport report, [NotNull] ISheet sheet, int i, [NotNull] ActivityReportItem item, [NotNull] ICellStyle timeCellStyle)
        {
            var row = sheet.CreateRow(i);

            var cell = row.CreateCell(0);
            string periodType;
            switch (item.PeriodType)
            {
                case PeriodType.Working:
                    periodType = Texts.Working;
                    break;
                case PeriodType.Leisure:
                    periodType = Texts.Leisure;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(item.PeriodType), item.PeriodType, null);
            }

            cell.SetCellValue(periodType);

            cell = row.CreateCell(1);
            cell.SetCellValue((item.Period ?? report.ReportDate - item.Start).ToPrettyFormat());

            cell = row.CreateCell(2);
            cell.CellStyle = timeCellStyle;
            cell.SetCellValue(item.Start);

            cell = row.CreateCell(3);
            cell.CellStyle = timeCellStyle;
            cell.SetCellValue(item.End ?? DateTime.Now);
            AutoSizeRow(row, sheet);
        }

        private static void CreateHeader([NotNull] ISheet sheet, [NotNull] ICellStyle boldCellStyle)
        {
            var row = sheet.CreateRow(0);
            var cell = row.CreateCell(0);

            cell.SetCellValue(Texts.PeriodType);
            cell.CellStyle = boldCellStyle;

            cell = row.CreateCell(1);
            cell.SetCellValue(Texts.Duration);
            cell.CellStyle = boldCellStyle;

            cell = row.CreateCell(2);
            cell.SetCellValue(Texts.Start);
            cell.CellStyle = boldCellStyle;

            cell = row.CreateCell(3);
            cell.SetCellValue(Texts.End);
            cell.CellStyle = boldCellStyle;
            AutoSizeRow(row, sheet);
        }

        private static void AutoSizeRow([NotNull] IRow row, [NotNull] ISheet sheet)
        {
            var numberOfColumns = row.PhysicalNumberOfCells;
            for (var i = 0; i <= numberOfColumns; i++)
            {
                sheet.AutoSizeColumn(i);
            }
        }
    }
}