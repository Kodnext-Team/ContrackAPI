using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContrackAPI
{
    public interface IContainerTypeRepository
    {
        Result SaveContainerType(ContainerTypeDTO type);
        Result DeleteContainerType(string containertypeid);
        List<ContainerTypeDTO> GetContainerTypesList();
        ContainerTypeDTO GetContainerTypeByID(string typeid);
        ContainerTypeDTO GetContainerTypeByUUID(string uuid);
    }
}
