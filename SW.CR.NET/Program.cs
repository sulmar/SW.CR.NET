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
            AddTextObjectTest();

            AddImageToReportTest();

            // OptionsTest();

            

           

            ModifyReportTest();

            GetSqlTest();

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

        private static void OptionsTest()
        {
            var rpt = new ReportDocument();
            rpt.Load(@"Reports\SimpleReport.rpt");


            var doc = rpt.ReportClientDocument;

            var options = doc.ReportOptions;

            options.EnableSaveDataWithReport = true;

            doc.SaveAs("OptionsReport.rpt", @"c:\temp\");

        }

        private static void AddTextObjectTest()
        {
            var rpt = new ReportDocument();
            rpt.Load(@"Reports\SimpleReport.rpt");

            //access the ReportClientDocument in the ReportDocument (EROM bridge)
            var doc = rpt.ReportClientDocument;

            //first determine which area and section to add the text field to - in this case the ReportHeaderArea first's section
            var section = doc.ReportDefController.ReportDefinition.ReportHeaderArea.Sections[0];

            //create the textobject
            var textObject = new CrystalDecisions.ReportAppServer.ReportDefModel.TextObject();

            //create the paragraph elements for the textobject 
            var paragraphs = new CrystalDecisions.ReportAppServer.ReportDefModel.Paragraphs();
            var paragraph = new CrystalDecisions.ReportAppServer.ReportDefModel.Paragraph();
            var paragraphElements = new CrystalDecisions.ReportAppServer.ReportDefModel.ParagraphElements();
            var paragraphTextElement = new CrystalDecisions.ReportAppServer.ReportDefModel.ParagraphTextElement();

            //set the text value for the text field
            paragraphTextElement.Text = "This is the added text field in the reportheader";
            paragraphTextElement.Kind = CrystalDecisions.ReportAppServer.ReportDefModel.CrParagraphElementKindEnum.crParagraphElementKindText;

            //fix some wierd defaults and behavior by explicitly setting default font
            var fontColor = new CrystalDecisions.ReportAppServer.ReportDefModel.FontColor();
            var font = new CrystalDecisions.ReportAppServer.ReportDefModel.Font();
            font.Bold = true;
            font.Name = "Arial";
            fontColor.Font = font;
            paragraphTextElement.FontColor = fontColor;

            paragraphElements.Add(paragraphTextElement);
            paragraph.ParagraphElements = paragraphElements;
            paragraphs.Add(paragraph);
            textObject.Paragraphs = paragraphs;

            //now add it to the section
            textObject.Left = 2000;
            textObject.Top = 1;
            textObject.Width = 5000;
            textObject.Height = 226;

            doc.ReportDefController.ReportObjectController.Add(textObject, section);

            rpt.SaveAs(@"Reports\AddTextObjectReport.rpt");

            rpt.ExportToDisk(ExportFormatType.PortableDocFormat, "AddTextObjectReport.pdf");

            rpt.Close();

            rpt.Dispose();

            System.Diagnostics.Process.Start("AddTextObjectReport.pdf");

        }

        private static void AddImageToReportTest()
        {
            var rpt = new ReportDocument();

            var path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            var filename = path + @"\Images\sulmar.png";

            rpt.Load(@"Reports\SimpleReport.rpt");

            var doc = rpt.ReportClientDocument;

            var section = doc.ReportDefController.ReportDefinition.ReportHeaderArea.Sections[0];

            var picture = doc.ReportDefController.ReportObjectController.ImportPicture(filename, section, 1, 1);

            rpt.SaveAs(@"Reports\AddImageReport.rpt");

            rpt.ExportToDisk(ExportFormatType.PortableDocFormat, "AddImageReport.pdf");

            rpt.Close();

            rpt.Dispose();

            System.Diagnostics.Process.Start("AddImageReport.pdf");
        }

        private static void ModifyReportTest()
        {
            var rpt = new ReportDocument();
            rpt.Load(@"Reports\SimpleReport.rpt");

            var rows = rpt.Rows.DataView;

            var section = rpt.ReportDefinition.Sections["DetailSection1"];

            section.Height = 5 * 1440;

            var field = rpt.ReportDefinition.ReportObjects[9];

            var fieldObjects = rpt.ReportDefinition.ReportObjects.OfType<FieldObject>().ToList();

            var root = fieldObjects.First();

            var number = 1;

            foreach (FieldObject fieldObject in fieldObjects)
            {
                fieldObject.ObjectFormat.HorizontalAlignment = Alignment.LeftAlign;
                fieldObject.Left = root.Left;
                fieldObject.Top = root.Top + 300 * number;

                if (fieldObject.Name=="Status1")
                {
                    fieldObject.ObjectFormat.EnableSuppress = true;
                }

                
                number++;
            }


            // field.Left += 3 * 1440;



            //field.Left = root.Left;
            //field.Top = root.Top + 1440;

            rpt.SaveAs(@"Reports\ModifiedReport.rpt");

            // rpt.ReportDefinition.ReportObjects
        }

        private static void GetSqlTest()
        {
            int jednostkaId = 3;
            int osobaId = 1;

            var rpt = new ReportDocument();

            rpt.Load(@"Reports\ReportWithParameter.rpt");

            rpt.SetParameterValue("Jednostka", jednostkaId);
            rpt.SetParameterValue("Osoba", osobaId);

            var temp = "";
            var groupPath = new CrystalDecisions.ReportAppServer.DataDefModel.GroupPath();

            var sql = rpt.ReportClientDocument.RowsetController.GetSQLStatement(groupPath, out temp);

            

            Console.WriteLine(sql);

            rpt.Close();

            rpt.Dispose();

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

        //private static void GetSqlTest()
        //{

        //    // assemblyref://CrystalDecisions.ReportAppServer.DataDefModel
        //    // assemblyref://CrystalDecisions.ReportAppServer.ClientDoc
        //    // assemblyref://CrystalDecisions.ReportAppServer.Controllers

        //    var rpt = new ReportDocument();

        //    rpt.Load(@"Reports\ReportWithOptionalParameter.rpt");

        //    // Uwaga: przez pobraniem zapytania SQL należy ustawić parametry jeśli są w raporcie
        //    rpt.SetParameterValue("OsobaId", 3);

        //    var temp = "";

        //    var boGroupPath = new CrystalDecisions.ReportAppServer.DataDefModel.GroupPath();

        //    var sql = rpt.ReportClientDocument.RowsetController.GetSQLStatement(boGroupPath, out temp);


        //    Console.WriteLine(sql);

        //    rpt.Clone();
        //    rpt.Dispose();
        //}

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
