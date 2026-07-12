using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ContrackAPI
{
    public class ContainerSizeDTO
    {
        public EncryptedData sizeid { get; set; } = new EncryptedData();
        public string sizename { get; set; } = "";
        public string length { get; set; } = "";
        public string width { get; set; } = "";
        public string height { get; set; } = "";
        public int sizeorderby { get; set; } = 0;
        public int groupno { get; set; } = 0;
        public List<ContainerModelDTO> models { get; set; } = new List<ContainerModelDTO>();
    }
}