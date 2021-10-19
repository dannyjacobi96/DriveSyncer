using System.Threading.Tasks;
using DriveSyncer.Core;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DriveSyncer.Cli.Commands
{
    public class RunCommand : AsyncCommand
    {
        public override async Task<int> ExecuteAsync(CommandContext context)
        {
           await AnsiConsole.Status()
               .AutoRefresh(true)
               .Spinner(Spinner.Known.Default)
               .StartAsync("[red]Syncer[/]", StartStatus);
            return 0;
        }

        private async Task StartStatus(StatusContext ctx)
        {
            var log = new StatusSyncLog(ctx);
            var options = Utils.ReadOptions();
            Sync sync = new Sync();
            await sync.SyncDrive(options.SourceToken, options.DestToken, options.FolderSource, options.FolderDest,
                new Core.Options()
                {
                    Override = options.Override,
                    Count = options.Count
                }, log);
        }
        
    }
}