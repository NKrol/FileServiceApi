using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace FileServiceApi.Entities
{
    public class User : AEntities
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual FilePath FilePath { get; set; }
        public virtual HashSet<Files> FilesSet { get; set; }
    }
}
