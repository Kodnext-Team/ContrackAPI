using Microsoft.AspNetCore.Http;

namespace ContrackAPI
{
    public interface IDocumentService
    {
        APIResponse UploadDocument(UploadDocumentRequest request);

        APIResponse GetDocumentListUUID(string uuid, int type);

        APIResponse GetDocumentByUUID(string uuid);

        APIResponse DeleteDocument(string uuid);
    }
}