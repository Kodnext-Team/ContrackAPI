using ContrackAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContrackAPI
{
    public interface ICommonRepository
    {
        List<IconDTO> GetIcons();
    }
}