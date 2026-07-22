using ContrackAPI;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace ContrackAPI
{
    public class DocumentRepository : CustomException, IDocumentRepository
    {
        private readonly AppSettings _settings;

        public DocumentRepository(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
        }
        public Result UploadFile(DocumentDTO doc, IFormFile file)
        {
            Result result = Common.ErrorMessage("Cannot upload");

            try
            {
                if (file == null || file.Length == 0)
                    return Common.ErrorMessage("File is empty");

                doc.filename = Path.GetFileName(file.FileName);
                doc.fileextension = Path.GetExtension(file.FileName);

                string attachmentFolder = _settings.AttachmentFolder ?? string.Empty;

                string documentTypeName = AttachmentFileTypes.TypeNames.TryGetValue(doc.documenttypeid, out var typeName)
                    ? typeName
                    : "General";

                string folder = string.Empty;

                if (!string.IsNullOrEmpty(doc.parentuuid) &&
                    !string.IsNullOrEmpty(doc.targetuuid))
                {
                    folder = Path.Combine(
                        attachmentFolder,
                        $"Hub-{1}",
                        $"{doc.documenttypeid}-{documentTypeName}",
                        $"P-{doc.parentuuid}",
                        doc.targetuuid);
                }
                else if (!string.IsNullOrEmpty(doc.targetuuid))
                {
                    folder = Path.Combine(
                        attachmentFolder,
                        $"Hub-{1}",
                        $"{doc.documenttypeid}-{documentTypeName}",
                        doc.targetuuid);
                }
                else if (doc.targetid > 0)
                {
                    folder = Path.Combine(
                        attachmentFolder,
                        $"Hub-{1}",
                        $"{doc.documenttypeid}-{documentTypeName}",
                        doc.targetid.ToString());
                }
                else
                {
                    return Common.ErrorMessage("No Target");
                }

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string tempFileName =
                    $"{DateTime.Now:yyyy_MM_dd_HH_mm_ss}_{doc.filename}";

                doc.filepath = Path.Combine(folder, tempFileName);

                using (var stream = new FileStream(doc.filepath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                FileInfo fileInfo = new FileInfo(doc.filepath);
                doc.filesize = fileInfo.Length;

                result = SaveDocument(doc);
            }
            catch (Exception ex)
            {
                RecordException(ex);
                result = Common.ErrorMessage(ex.Message);
            }

            return result;
        }
        public Result SaveDocument(DocumentDTO doc)
        {
            Result result = new Result();

            try
            {
                using (SqlDB Db = new SqlDB())
                {
                    DataTable tbl = Db.GetDataTable(
                        "SELECT * from procurement.savedocument(" +
                        "p_hub_id := '" + 1 + "'," +
                        "p_created_by := '" + 2 + "'," +
                        "p_document_type_id := '" + doc.documenttypeid + "'," +
                        "p_file_name := '" + doc.filename + "'," +
                        "p_file_path := '" + doc.filepath + "'," +
                        "p_file_extension := '" + doc.fileextension + "'," +
                        "p_parent_uuid := " + Common.GetUUID(doc.parentuuid) + "," +
                        "p_target_uuid := " + Common.GetUUID(doc.targetuuid) + "," +
                        "p_target_id := '" + doc.targetid + "'," +
                        "p_file_size := '" + doc.filesize + "'" +
                        ");");

                    if (tbl.Rows.Count > 0)
                    {
                        if (tbl.Rows[0][0].ToString() == "1")
                        {
                            result = Common.SuccessMessage(tbl.Rows[0][1].ToString());
                        }
                        else
                        {
                            result = Common.ErrorMessage(tbl.Rows[0][1].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
                result = Common.ErrorMessage(ex.Message);
            }

            return result;
        }
        public List<DocumentDTO> GetDocumentListUUID(string uuid, int type)
        {
            List<DocumentDTO> list = new List<DocumentDTO>();
            try
            {
                using (SqlDB Db = new SqlDB())
                {

                    DataTable tbl = Db.GetDataTable("SELECT * FROM procurement.getbydocumentsbyuuid('" + 1 + "','" + uuid + "','" + type + "');");

                    list = (from DataRow dr in tbl.Rows
                            select new DocumentDTO()
                            {
                                documentid = Common.ToInt(dr["documentid"]),
                                documentuuid = Common.ToString(dr["documentuuid"]),
                                documenttypeid = Common.ToInt(dr["documenttypeid"]),
                                parentuuid = Common.ToString(dr["parentuuid"]),
                                targetuuid = Common.ToString(dr["targetuuid"]),
                                targetid = Common.ToInt(dr["targetid"]),
                                filename = Common.ToString(dr["filename"]),
                                filepath = Common.ToString(dr["filepath"]),
                                fileextension = Common.ToString(dr["fileextension"]),
                                filesize = Common.ToLong(dr["filesize"]),
                                createdat = Common.ToDateTime(dr["createdat"]),
                                createdbyname = Common.ToString(dr["fullname"]),
                            }).ToList();

                    //list.ForEach(x =>
                    //{
                    //    x.createdat = x.createdat.AddMinutes(Convert.ToDouble(SessionManager.TimeOffSet));
                    //});

                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }
            return list;
        }

        public DocumentDTO GetDocumentByUUID(string uuid)
        {
            DocumentDTO docu = new DocumentDTO();
            try
            {
                using (SqlDB Db = new SqlDB())
                {

                    DataTable tbl = Db.GetDataTable("SELECT * FROM procurement.getbydocumentuuid('" + 1 + "','" + uuid + "');");
                    if (tbl.Rows.Count > 0)
                    {
                        docu = new DocumentDTO()
                        {
                            documentid = Common.ToInt(tbl.Rows[0]["documentid"]),
                            documentuuid = Common.ToString(tbl.Rows[0]["documentuuid"]),
                            documenttypeid = Common.ToInt(tbl.Rows[0]["documenttypeid"]),
                            parentuuid = Common.ToString(tbl.Rows[0]["parentuuid"]),
                            targetuuid = Common.ToString(tbl.Rows[0]["targetuuid"]),
                            targetid = Common.ToInt(tbl.Rows[0]["targetid"]),
                            filename = Common.ToString(tbl.Rows[0]["filename"]),
                            filepath = Common.ToString(tbl.Rows[0]["filepath"]),
                            fileextension = Common.ToString(tbl.Rows[0]["fileextension"]),
                            filesize = Common.ToLong(tbl.Rows[0]["filesize"]),
                            createdat = Common.ToDateTime(tbl.Rows[0]["createdat"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }
            return docu;
        }

        public Result DeleteDocument(string p_document_uuid)
        {
            Result result = new Result();
            try
            {
                using (SqlDB Db = new SqlDB())
                {

                    DataTable tbl = Db.GetDataTable("SELECT * from procurement.DeleteByDocumentUUID(" +
                        "'" + 1 + "'," +
                        "'" + p_document_uuid + "'," +
                       "'" + 2 + "');");
                    if (tbl.Rows.Count != 0)
                    {
                        if (tbl.Rows[0][0].ToString() == "1")
                        {
                            result = Common.SuccessMessage(tbl.Rows[0][1].ToString());
                        }
                        else
                            result = Common.ErrorMessage(tbl.Rows[0][1].ToString());
                    }
                    else
                        result = Common.ErrorMessage("Cannot Delete Document");
                }
            }
            catch (Exception ex)
            {
                result = Common.ErrorMessage(ex.Message);
                RecordException(ex);
            }
            return result;
        }
    }
}