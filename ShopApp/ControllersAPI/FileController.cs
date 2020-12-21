using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace ShopApp.ControllersAPI
{
    public class FileController : ApiController
    {
        // GET: File
        [System.Web.Http.HttpPost]
       public DocumentUploadResponse PostDocument()
        {
            DocumentUploadResponse response = new DocumentUploadResponse();
            try
            {
                var httpRequest = HttpContext.Current.Request;
                if(httpRequest.Files.Count > 0)
                {
                    foreach(string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        var filePath = HttpContext.Current.Server.MapPath("~/XamarinFolder/" +postedFile.FileName);
                        postedFile.SaveAs(filePath);

                        response.DocumentUrls.Add(filePath);
                        response.IsUploaded = true;
                        response.Message = "Pliki dodane pomyślnie";
                    }
                }
                else
                {
                    response.IsUploaded = false;
                    response.Message = "Nie udało się zamieścić plików";
                }
            }
            catch(Exception ex)
            {
                response.Message = ex.Message; 
            }

            return response;

        }
    }
}