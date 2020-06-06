using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using iTextSharp.text.pdf.qrcode;
using System.Collections.Generic;
using FuncionOrquestador.Models;

namespace FuncionOrquestador
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", "delete", "Put", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var result = new List<Pagos>();
            /////////////////////////////////////////////////////////////////POST////////////////////////////////////////////////////////////////////////////
            if (req.HttpContext.Request.Method == HttpMethods.Post)
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                Pagos pago = JsonConvert.DeserializeObject<Pagos>(requestBody);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://insertarpendientedepagos.azurewebsites.net/api/InsertarPendientesDePago");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(pago);
                    streamWriter.Write(json);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var resultado = streamReader.ReadToEnd();
                }
                
            }



            /////////////////////////////////////////////////////////////////GET////////////////////////////////////////////////////////////////////////////
            if (req.HttpContext.Request.Method == HttpMethods.Get)
            {
                string responseFromServer = "";
                WebRequest request = WebRequest.Create("https://consultarpendientedepagos.azurewebsites.net/api/consultar/1");
                request.Credentials = CredentialCache.DefaultCredentials;
                WebResponse response = request.GetResponse();
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                using (Stream dataStream = response.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.
                    responseFromServer = reader.ReadToEnd();
                    // Display the content.
                    //Console.WriteLine(responseFromServer);
                    //productItem = JsonConvert.DeserializeObject<Pagos>(responseFromServer);
                    result = JsonConvert.DeserializeObject<List<Pagos>>(responseFromServer);
                }
                response.Close();

                if (result == null)
                {
                    return new NotFoundResult();
                }
                return new OkObjectResult(result);
            }
            return null;
        }
    }
}
