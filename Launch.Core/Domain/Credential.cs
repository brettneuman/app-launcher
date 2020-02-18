using System;
using System.Collections.Generic;
using System.Text;

namespace Launch.Core.Domain
{
    public class Credential
    {
        // [Key]
        public string Name { get; set; }
        public string Domain { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
