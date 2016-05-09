using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace StreamingDemo.Controllers
{
    [RoutePrefix("api/media")]
    public class MediaController: ApiController
    {
        const string FILE = @"c:\setups\_de_windows_10_enterprise_2015_ltsb_n_x64_dvd_6848328.iso";

        public HttpResponseMessage Get()
        {
            var response = new HttpResponseMessage();
            var stream = new FileStream(FILE, FileMode.Open);
            response.Content = new StreamContent(stream);
            return response;
        }


        [Route("pushcontent")]
        public HttpResponseMessage GetWithPushContent() {

            var response = new HttpResponseMessage();

            response.Content = new PushStreamContent((stream, httpContent, transportCtx) => {

                using (stream) {
                    using (var fileStream = new FileStream(FILE, FileMode.Open))
                    {
                        fileStream.CopyTo(stream);
                    }
                }
            });

            return response;
        }


        [Route("all")]
        public async Task PostMultipart() {
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            var multipart = await Request.Content.ReadAsMultipartAsync();

            foreach (var part in multipart.Contents) {
                Debug.WriteLine(part.Headers.ContentDisposition.FileName);
                var stream = await part.ReadAsStreamAsync();
                var buffer = new byte[1024*1024];
                while(stream.Read(buffer, 0, buffer.Length) > 0) {
                    Debug.Write(".");
                }
            }
            Debug.WriteLine("");

        }


        /// <summary>
        /// Father, forgive me!
        /// </summary>
        //public HttpResponseMessage Post() {
        //    // Beware: Requesting Resources via POST ist not RESTlike !!!
        //    return Get();            
        //}
        public async void Put() 
        {
            // var stream = HttpContext.Current.Request.GetBufferlessInputStream();
            Stream stream = await Request.Content.ReadAsStreamAsync();

            int size = 1024 * 1024;
            byte[] buffer = new byte[size];

            while (stream.Read(buffer,0, buffer.Length) > 0)
            {
                Debug.Write(".");
            }

        }



    }
}