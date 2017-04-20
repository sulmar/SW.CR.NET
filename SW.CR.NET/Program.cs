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
            LoadReportOptionalParameterTest();
            
            ExportToPdfTest();


            PrintTest();

            SetSelectionFormulaTest();

            SetLocationTest();

            GetParametersTest();

            LoadReportParameterMultiTest();

            LoadReportForPeriodTest();

            LoadReportWithParameterTest();

            LoadReportTest();

        }

        // na podst.
        // https://apps.support.sap.com/sap/support/knowledge/public/en/1893554
        private static void LoadReportOptionalParameterTest()
        {
            int? osobaId = null;

            Console.WriteLine("Podaj identyfikator osoby");

            var input = Console.ReadLine();

            int id;

            if (int.TryParse(input, out id))
            {
                osobaId = id;
            }

            var rpt = new ReportDocument();

            rpt.Load(@"Reports\ReportWithOptionalParameter.rpt");

            if (osobaId.HasValue)
            {
                rpt.SetParameterValue("OsobaId", osobaId);
            }
            else
            {
                var parameter = rpt.ParameterFields["OsobaId"];

                parameter.CurrentValues.Clear();
                parameter.CurrentValues.IsNoValue = true;
            }

            rpt.ExportToDisk(ExportFormatType.PortableDocFormat, "ReportWithOptionalParameter.pdf");

            rpt.Close();

            rpt.Dispose();

            System.Diagnostics.Process.Start("ReportWithOptionalParameter.pdf");

        }

        private static void GetSqlTest()
        {

            // assemblyref://CrystalDecisions.ReportAppServer.DataDefModel
            // assemblyref://CrystalDecisions.ReportAppServer.ClientDoc
            // assemblyref://CrystalDecisions.ReportAppServer.Controllers

            var rpt = new ReportDocument();

            rpt.Load(@"Reports\ReportWithOptionalParameter.rpt");

            // Uwaga: przez pobraniem zapytania SQL należy ustawić parametry jeśli są w raporcie
            rpt.SetParameterValue("OsobaId", 3);

            var temp = "";

            var boGroupPath = new CrystalDecisions.ReportAppServer.DataDefModel.GroupPath();

            var sql = rpt.ReportClientDocument.RowsetController.GetSQLStatement(boGroupPath, out temp);


            Console.WriteLine(sql);

            rpt.Clone();
            rpt.Dispose();
        }

        private static void ExportToPdfTest()
        {
            var rpt = new ReportDocument();

            rpt.Load(@"Reports\SimpleReport.rpt");

        }

        private static void PrintTest()
        {
            var rpt = new ReportDocument();

            rpt.Load(@"Reports\SimpleReport.rpt");

            // Wydruk wszystkich stron
       
            rpt.PrintToPrinter(1, true, 0, 0);
        }

        private static void SetSelectionFormulaTest()
        {
            var rpt = new ReportDocument();

            rpt.Load(@"Reports\SimpleReport.rpt");

            rpt.RecordSelectionFormula = "{Orzeczenia.OsobaId} = 3";

            rpt.ExportToDisk(ExportFormatType.PortableDocFormat, "SetSelectionFormula.pdf");

            rpt.Close();

            rpt.Dispose();

            System.Diagnostics.Process.Start("SetSelectionFormula.pdf");


        }

        private static void SetLocationTest()
        {
            var connectionInfo = new ConnectionInfo
            {
                ServerName = @"localhost\SQLEXPRESS",
                DatabaseName = "CrystalReportsDb",
                IntegratedSecurity = true,
                //UserID = "myuser",
                //Password = "mypassword"
            };

            var rpt = new ReportDocument();
            rpt.Load(@"Reports\SimpleReport.rpt");

            foreach (Table table in rpt.Database.Tables)
            {
                // Zmiana źródła danych
                TableLogOnInfo logOnInfo = table.LogOnInfo;
                logOnInfo.ConnectionInfo = connectionInfo;

                table.ApplyLogOnInfo(logOnInfo);

                Console.WriteLine($"{table.Name}");
            }

            rpt.SaveAs(@"Reports\output.rpt");

            //rpt.ExportToDisk(ExportFormatType.CrystalReport, "SetLocationTest.rpt");
        }

        private static void GetParametersTest()
        {
            var rpt = new ReportDocument();

            rpt.Load(@"Reports\ReportForPeriod.rpt");

            // Pobierz wszystkie parametry
            var parameters = rpt.ParameterFields.Cast<ParameterField>().ToList();

            foreach (var parameter in parameters)
            {
                Console.WriteLine($"{parameter.Name} {parameter.PromptText} {parameter.ParameterValueType}");
            }


            // Pobierz tylko używane parametry
            var parametersUsage = parameters.Where(p => p.ParameterFieldUsage2.HasFlag(ParameterFieldUsage2.InUse));

            foreach (var parameter in parametersUsage)
            {
                Console.WriteLine($"{parameter.Name} {parameter.PromptText} {parameter.ParameterValueType}");
            }



        }

        private static void LoadReportParameterMultiTest()
        {

            var rpt = new ReportDocument();

            rpt.Load(@"Reports\ReportWithParameterMulti.rpt");

            var departments = new ParameterValues();

            departments.AddValue(1);
            departments.AddValue(3);
            departments.AddValue(4);

            var p1 = new ParameterDiscreteValue { Value = 5 };
            departments.Add(p1);


            rpt.SetParameterValue("Jednostki", departments);

            rpt.ExportToDisk(ExportFormatType.PortableDocFormat, "ReportWithParameterMulti.pdf");

            rpt.Close();

            rpt.Dispose();

            System.Diagnostics.Process.Start("ReportWithParameterMulti.pdf");

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
