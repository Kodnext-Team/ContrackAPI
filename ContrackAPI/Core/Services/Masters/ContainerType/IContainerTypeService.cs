using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContrackAPI
{
    public interface IContainerTypeService
    {
        void SaveContainerType(ContainerTypeDTO type);
        void DeleteContainerType(string containertypeid);
        List<ContainerTypeDTO> GetContainerTypesList();
        ContainerTypeDTO GetContainerTypeByID(string typeid);
        ContainerTypeDTO GetContainerTypeByUUID(string uuid);
    }
}
