using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileServiceApi.Models
{
    public class OpenFileDto
    {
        public string FilePath { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}
