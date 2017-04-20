using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace SW.CR.NET.Service.ActionResults
{
    public class StreamActionResult : IHttpActionResult
    {
        private readonly Stream stream;

        public StreamActionResult(Stream stream)
        {
            this.stream = stream;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");

            return Task.FromResult(response);
        }
    }
}
