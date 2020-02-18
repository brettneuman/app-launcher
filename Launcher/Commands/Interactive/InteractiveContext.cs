namespace Launch.CLI.Commands
{
    public interface IInteractiveContext
    {
        bool Interactive { get; set; }
    }

    public class InteractiveContext : IInteractiveContext
    {
        private bool _interactive = false;

        public bool Interactive { get { return _interactive; } set { _interactive = value; } }
    }
}
