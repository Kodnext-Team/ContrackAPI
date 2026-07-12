using System;
using System.Collections.Generic;

namespace ContrackAPI
{
    public class ContainerTypeService : CustomException, IContainerTypeService
    {
        public Result Result = new Result();
        private readonly IContainerTypeRepository _repo;

        public ContainerTypeService(IContainerTypeRepository repo)
        {
            _repo = repo;
        }

        public void SaveContainerType(ContainerTypeDTO type)
        {
            try
            {
                Result = _repo.SaveContainerType(type);
            }
            catch (Exception ex)
            {
                Result = Common.ErrorMessage(ex.ToString());
                RecordException(ex);
            }
        }

        public void DeleteContainerType(string containertypeid)
        {
            try
            {
                Result = _repo.DeleteContainerType(containertypeid);
            }
            catch (Exception ex)
            {
                Result = Common.ErrorMessage(ex.ToString());
                RecordException(ex);
            }
        }

        public List<ContainerTypeDTO> GetContainerTypesList()
        {
            try
            {
                return _repo.GetContainerTypesList();
            }
            catch (Exception ex)
            {
                RecordException(ex);
                return new List<ContainerTypeDTO>();
            }
        }

        public ContainerTypeDTO GetContainerTypeByID(string typeid)
        {
            try
            {
                return _repo.GetContainerTypeByID(typeid);
            }
            catch (Exception ex)
            {
                RecordException(ex);
                return new ContainerTypeDTO();
            }
        }

        public ContainerTypeDTO GetContainerTypeByUUID(string uuid)
        {
            try
            {
                return _repo.GetContainerTypeByUUID(uuid);
            }
            catch (Exception ex)
            {
                RecordException(ex);
                return new ContainerTypeDTO();
            }
        }
    }
}
