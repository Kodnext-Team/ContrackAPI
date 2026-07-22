using Microsoft.AspNetCore.Http;

namespace ContrackAPI
{
    public interface IDocumentRepository
    {
        Result UploadFile(DocumentDTO doc, IFormFile file);

        Result SaveDocument(DocumentDTO doc);

        List<DocumentDTO> GetDocumentListUUID(string uuid, int type);

        DocumentDTO GetDocumentByUUID(string uuid);

        Result DeleteDocument(string uuid);
    }
}