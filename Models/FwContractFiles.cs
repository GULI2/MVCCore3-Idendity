using System;
using System.Collections.Generic;

namespace JNGH_IDENDITY.Models
{
    public partial class FwContractFiles
    {
        public long Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileExtension { get; set; }
        public int SubmitType { get; set; }
        public byte[] FileDatas { get; set; }
    }
}
