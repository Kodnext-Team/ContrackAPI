using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ContrackAPI
{
    public class ContainerService : CustomException, IContainerService
    {
        private readonly IContainerRepository _repo;
        APIResponse response = new APIResponse();
        public ContainerService(IContainerRepository repo)
        {
            _repo = repo;
        }
        public APIResponse GetContainerList(ContainerFilterPage filter)
        {
            try
            {
                ProcessFilters(filter);
                var list = _repo.GetContainerList(filter);
                if (list.Count == 0)
                {
                    response.Result = Common.ErrorMessage("No data found");
                }
                else
                {
                    response.Result = Common.SuccessMessage("Success");
                }
                response.Data = list;
            }
            catch (Exception ex)
            {
                RecordException(ex);
                response.Result = Common.ErrorMessage(ex.Message);
            }
            return response;
        }
        private void ProcessFilters(ContainerFilterPage filter)
        {
            if (filter.filters == null)
                filter.filters = new ContainerFilter();
            // Container Type
            if (!string.IsNullOrWhiteSpace(filter.filters.containertype_encry))
            {
                filter.filters.containertypeids = filter.filters.containertype_encry
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => (long)Common.Decrypt(x.Trim()))
                    .Where(x => x > 0)
                    .ToList();
            }

            // Container Size
            if (!string.IsNullOrWhiteSpace(filter.filters.containersize_encry))
            {
                filter.filters.containersizeids = filter.filters.containersize_encry
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Common.ToInt(Common.Decrypt(x.Trim())))
                    .Where(x => x > 0)
                    .ToList();
            }
            if (!string.IsNullOrWhiteSpace(filter.filters.containermodel_encry))
            {
                filter.filters.containermodeluuids = filter.filters.containermodel_encry
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .ToList();
            }
            if (!string.IsNullOrWhiteSpace(filter.filters.location_encry))
            {
                filter.filters.locationuuids = filter.filters.location_encry
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .ToList();
            }
           
            // POL
            if (!string.IsNullOrWhiteSpace(filter.filters.pol_encry))
            {
                filter.filters.pols = filter.filters.pol_encry
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Common.ToInt(Common.Decrypt(x.Trim())))
                    .Where(x => x > 0)
                    .ToList();
            }

            // POD
            if (!string.IsNullOrWhiteSpace(filter.filters.pod_encry))
            {
                filter.filters.pods = filter.filters.pod_encry
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Common.ToInt(Common.Decrypt(x.Trim())))
                    .Where(x => x > 0)
                    .ToList();
            }

            // Voyage
            if (!string.IsNullOrWhiteSpace(filter.filters.voyage_encry))
            {
                filter.filters.voyageids = filter.filters.voyage_encry
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Common.ToInt(Common.Decrypt(x.Trim())))
                    .Where(x => x > 0)
                    .ToList();
            }

            // Move
            if (!string.IsNullOrWhiteSpace(filter.filters.move_encry))
            {
                filter.filters.moveids = filter.filters.move_encry
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Common.ToInt(Common.Decrypt(x.Trim())))
                    .Where(x => x > 0)
                    .ToList();
            }

            // Operator
            if (!string.IsNullOrWhiteSpace(filter.filters.operator_encry))
            {
                filter.filters.operatorids = filter.filters.operator_encry
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Common.ToInt(Common.Decrypt(x.Trim())))
                    .Where(x => x > 0)
                    .ToList();
            }
           

            // Status
            if (filter.filters.status > 0)
                filter.filters.status_list = new List<int> { filter.filters.status };
            else
                filter.filters.status_list = new List<int>();

          
        }
        public APIResponse GetContainerByUUID(string containeruuid)
        {
            try
            {
                var dto = _repo.GetContainerByUUID(containeruuid);
                if (dto != null && !string.IsNullOrEmpty(dto.containeruuid))
                {
                    var model = new ContainerModal { container = dto };
                    if (dto.manufacturedate.Value != DateTime.MinValue)
                    {
                        model.MakeMonth = dto.manufacturedate.Value.Month;
                        model.MakeYear = dto.manufacturedate.Value.Year;
                    }
                    response.Result = Common.SuccessMessage("Success");
                    response.Data = model;
                }
                else
                {
                    response.Result = Common.ErrorMessage("No data found");
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
                response.Result = Common.ErrorMessage(ex.Message);
            }
            return response;
        }
        public APIResponseType GetContainerByEquipmentno(string equipmentno)
        {
            APIResponseType response = new APIResponseType();
            try
            {

                var dto = _repo.GetContainerByEquipmentno(equipmentno);

                if (dto != null && dto.Containers.Count > 0)
                {
                    response.Result = Common.SuccessMessage("Success");
                    response.MatchType = dto.MatchType;
                    response.Data = dto.Containers;
                }
                else
                {
                    response.Result = Common.ErrorMessage("No data found");
                }
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