using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace TaiFileAPI.Controllers
{
    public class DownloadController : ApiController
    {
        [HttpGet]
        public IHttpActionResult taiFile(string str)
        {
            if (str != null || str != "")
            {
                string filename = str.Substring(str.LastIndexOf('/')+1);
                string filepath = str.Substring(0,str.LastIndexOf('/')+1);
                var path = HttpContext.Current.Server.MapPath(filepath);
                if (System.IO.Directory.Exists(path))
                {
                    var databytes = File.ReadAllBytes(path+filename);
                    var dataStream = new MemoryStream(databytes);
                    return new ImageDownLoad(dataStream, Request, filename);
                }
                return BadRequest("cần đường dẫn không đúng");
            }
            else
                return BadRequest("cần đường dẫn");
        }
    }
    public class ImageDownLoad : IHttpActionResult
    {
        MemoryStream stream;
        string filename;
        HttpRequestMessage reQ;
        HttpResponseMessage reP;
        public ImageDownLoad(MemoryStream data, HttpRequestMessage request, string filenames)
        {
            stream = data;
            reQ = request;
            filename = filenames;

        }
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            reP = reQ.CreateResponse(HttpStatusCode.OK);
            reP.Content = new StreamContent(stream);
            reP.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = filename
            };
            reP.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            return Task.FromResult(reP);
                 

        }
    }



}
