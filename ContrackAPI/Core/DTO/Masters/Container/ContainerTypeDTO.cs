using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ContrackAPI
{
    public class ContainerTypeDTO
    {
        public EncryptedData containertypeid { get; set; } = new EncryptedData();
        public string typeuuid { get; set; } = "";
        public string typename { get; set; } = "";
        public Icon icon { get; set; } = new Icon();
        public DateTime createdat { get; set; } = new DateTime();
        public string typeshortname { get; set; } = "";
        public AdditionalData addtionaldata { get; set; } = new AdditionalData(); // ex: isreefer
        public List<ContainerSizeDTO> sizes { get; set; } = new List<ContainerSizeDTO>();

    }
    public class AdditionalData
    {
        public bool isreefer { get; set; } = false;
    }
}