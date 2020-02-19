using System;
using System.Collections.Generic;
using System.Text;

namespace Launch.CLI.Commands
{
    public class SaveCredRequest
    {
        public string Name { get; set; }
        public string Domain { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class SaveAppRequest
    {
        public string Name { get; set; }
        public string Target { get; set; }
        public string WorkingDirectory { get; set; }
        public string IconPath { get; set; }
        public bool AdminMode { get; set; }
    }
}
