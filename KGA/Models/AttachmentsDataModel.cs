using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KGA.Models
{
    public class AttachmentsDataModel
    {
        public int FileID { get; set; }
        public Nullable<int> ProblemID { get; set; }
        public string UserName { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
    }
}