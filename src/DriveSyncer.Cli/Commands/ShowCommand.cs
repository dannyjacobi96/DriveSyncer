using Spectre.Console.Cli;

namespace DriveSyncer.Cli.Commands
{
    public class ShowCommand : Command
    {
        public override int Execute(CommandContext context)
        {
            var options = Utils.ReadOptions();
            Utils.ShowOptions(options);
            return 0;
        }
    }
}
