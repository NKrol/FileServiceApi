using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileServiceApi
{
    public class Authentication
    {
        public string JwtKey { get; set; }
        public string JwtIssuer { get; set; }
        public int JwtExpire { get; set; }

    }
}
