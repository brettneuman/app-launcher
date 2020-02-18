namespace Launch.Core.Services
{
    public class LaunchOptions
    {
        public string Domain { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FilePath { get; set; }
        public string WorkingDirectory { get; set; }
        public bool RunAsAdmin { get; set; }
    }
}
