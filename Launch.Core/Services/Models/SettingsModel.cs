using System;
using System.Collections.Generic;
using System.Linq;
using Launch.Core.Domain;

namespace Launch.Core.Services
{
    public class SettingsModel
    {
        public ICollection<Credential> Credentials { get; set; }
        public ICollection<Application> Applications { get; set; }

        public SettingsModel()
        {
            Credentials = new List<Credential>();
            Applications = new List<Application>();
        }
    }
}
