using ContrackAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace ContrackAPI
{
    public class DocumentService : CustomException, IDocumentService
    {
        private readonly IDocumentRepository _repo;
        APIResponse response = new APIResponse();

        public DocumentService(IDocumentRepository repo)
        {
            _repo = repo;
        }

        public APIResponse UploadDocument(UploadDocumentRequest request)
        {
            try
            {
                DocumentDTO doc = new DocumentDTO
                {
                    documenttypeid = request.documenttypeid,
                    parentuuid = request.parentuuid,
                    targetuuid = request.targetuuid,
                    targetid = request.targetid
                };

                Result result = null;
                foreach (IFormFile file in request.file)
                {
                    result = _repo.UploadFile(doc, file);
                }
                response.Result = result;
            }
            catch (Exception ex)
            {
                RecordException(ex);
                response.Result = Common.ErrorMessage(ex.Message);
            }

            return response;
        }
     public APIResponse GetDocumentListUUID(string uuid, int type)
        {
            try
            {
                var list = _repo.GetDocumentListUUID(uuid, type);

                if (list != null && list.Count > 0)
                {
                    response.Data = list;
                    response.Result = Common.SuccessMessage("Success");
                }
                else
                {
                    response.Result = Common.ErrorMessage("No Data Found");
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
                response.Result = Common.ErrorMessage(ex.Message);
            }

            return response;
        }

        public APIResponse GetDocumentByUUID(string uuid)
        {
            try
            {
                var doc = _repo.GetDocumentByUUID(uuid);

                if (doc != null && !string.IsNullOrEmpty(doc.documentuuid))
                {
                    response.Data = doc;
                    response.Result = Common.SuccessMessage("Success");
                }
                else
                {
                    response.Result = Common.ErrorMessage("No Data Found");
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
                response.Result = Common.ErrorMessage(ex.Message);
            }

            return response;
        }

        public APIResponse DeleteDocument(string uuid)
        {
            try
            {
                response.Result = _repo.DeleteDocument(uuid);
            }
            catch (Exception ex)
            {
                RecordException(ex);
                response.Result = Common.ErrorMessage(ex.Message);
            }

            return response;
        }
    }
}