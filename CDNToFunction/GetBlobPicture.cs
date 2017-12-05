using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Http.Headers;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CDNToBlobFunctionApp
{
    public static class GetBlobPicture
    {
        [FunctionName("GetBlobPicture")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "HttpTriggerCSharp/name/{name}")]HttpRequestMessage req, string name, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // Blob has beeen granted public access to simplify the code sharing
            string filePath = @"https://stepherz.blob.core.windows.net/reezocar/car.jpg";

            var result = new HttpResponseMessage(HttpStatusCode.OK);            
            result.Content = new ByteArrayContent(GetFileContent(filePath));
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            result.Headers.Add("Cache-Control", "max-age=360");
            return result;

        }

        public static byte[] GetFileContent(string fileName)
        {
            CloudBlockBlob blockBlob = new CloudBlockBlob(new System.Uri(fileName));
            blockBlob.FetchAttributes();
            long fileByteLength = blockBlob.Properties.Length;
            byte[] fileContent = new byte[fileByteLength];

            blockBlob.DownloadToByteArray(fileContent, 0);
            return fileContent;
        }
    }
}
