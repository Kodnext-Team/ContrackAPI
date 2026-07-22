using Microsoft.AspNetCore.Http;

namespace ContrackAPI
{
    public class DocumentDTO
    {
        public int documentid { get; set; }
        public string documentuuid { get; set; } = "";
        public int documenttypeid { get; set; }
        public string documenttype { get; set; } = "";
        public string? parentuuid { get; set; } = "";
        public string targetuuid { get; set; } = "";
        public int targetid { get; set; }

        public string filename { get; set; } = "";
        public string filepath { get; set; } = "";
        public string fileextension { get; set; } = "";
        public long filesize { get; set; }

        public DateTime createdat { get; set; }
        public string createdbyname { get; set; } = "";
    }
}