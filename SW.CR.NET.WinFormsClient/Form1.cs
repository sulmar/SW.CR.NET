using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Windows.Forms;

namespace SW.CR.NET.WinFormsClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var rpt = new ReportDocument();
            rpt.Load(@"Reports\SimpleReport.rpt");

            this.crystalReportViewer2.ReportSource = rpt;

          //  this.crystalReportViewer2.ExportReport();
        }
    }
}
