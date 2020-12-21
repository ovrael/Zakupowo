using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopApp.ControllersAPI
{
    public class DocumentUploadResponse
    {
        public bool IsUploaded { get; set; }
        public string Message { get; set; } = "";
        public List<string> DocumentUrls { get; set; } = new List<string>();
    } 
}