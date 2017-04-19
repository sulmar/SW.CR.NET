using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.CR.NET.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            LoadReportForPeriodTest();

            LoadReportWithParameterTest();

            LoadReportTest();

        }

        private static void LoadReportForPeriodTest()
        {
            var jednostkaId = 1;
            var osobaId = 1;
            var dataOd = DateTime.Parse("2016-01-01");
            var dataDo = DateTime.Parse("2017-12-01");

            var period = new ParameterRangeValue
            {
                StartValue = dataOd,
                EndValue = dataDo,
            };

            var rpt = new ReportDocument();

            rpt.Load(@"Reports\ReportForPeriod.rpt");

            if (rpt.IsLoaded)
            {
                // Logowanie do bazy danych
                // rpt.SetDatabaseLogon("user", "password");

                // Przekazanie parametru
                rpt.SetParameterValue("Jednostka", jednostkaId);
                rpt.SetParameterValue("Osoba", osobaId);
                rpt.SetParameterValue("Okres", period);

                //rpt.SetParameterValue("DataOd", dataOd);
                //rpt.SetParameterValue("DataDo", dataDo);

                if (rpt.HasSavedData)
                {
                    rpt.Refresh();
                }

                rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, "ReportForPeriod.pdf");
            }

            rpt.Close();

            rpt.Dispose();

            System.Diagnostics.Process.Start("ReportForPeriod.pdf");
        }

        private static void LoadReportWithParameterTest()
        {
            var jednostkaId = 3;
            var osobaId = 1;

            var rpt = new ReportDocument();

            rpt.Load(@"Reports\ReportWithParameter.rpt");

            if (rpt.IsLoaded)
            {
                // Logowanie do bazy danych
                // rpt.SetDatabaseLogon("user", "password");

                // Przekazanie parametru
                rpt.SetParameterValue("Jednostka", jednostkaId);
                rpt.SetParameterValue("Osoba", osobaId);

                if (rpt.HasSavedData)
                {
                    rpt.Refresh();
                }

                rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, "ReportWithParameter.pdf");
            }

            rpt.Close();

            rpt.Dispose();

            System.Diagnostics.Process.Start("ReportWithParameter.pdf");
        }

        private static void LoadReportTest()
        {
            var rpt = new ReportDocument();

            var reportFilename = @"Reports\SimpleReport.rpt";

            rpt.Load(reportFilename);

            if (rpt.IsLoaded)
            {

                if (rpt.HasSavedData)
                {
                    rpt.Refresh();
                }

                Console.WriteLine("Report was loaded.");

                rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, "SimpleReport.pdf");
            }

            rpt.Close();

            rpt.Dispose();

            System.Diagnostics.Process.Start("SimpleReport.pdf");
        }
    }
}
