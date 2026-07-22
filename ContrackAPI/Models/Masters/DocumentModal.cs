using Microsoft.AspNetCore.Http;

namespace ContrackAPI
{
    public class UploadDocumentRequest
    {
        public int documenttypeid { get; set; }

        public string? parentuuid { get; set; }

        public string targetuuid { get; set; }

        public int targetid { get; set; }

        public List<IFormFile> file { get; set; } = new List<IFormFile>();
    }
   
}