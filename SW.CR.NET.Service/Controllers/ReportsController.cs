using CrystalDecisions.CrystalReports.Engine;
using SW.CR.NET.Service.ActionResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace SW.CR.NET.Service.Controllers
{
    public class ReportsController : ApiController
    {

        public string Get()
        {
            return "Hello Web Api!";
        }

        public IHttpActionResult Get(string id)
        {
            var rpt = new ReportDocument();

            rpt.Load(@"Reports\" + id);

            var stream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            return new StreamActionResult(stream);
        }
    }
}
