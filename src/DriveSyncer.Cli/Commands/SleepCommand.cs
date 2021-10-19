using System.ComponentModel;
using System.Threading.Tasks;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DriveSyncer.Cli.Commands
{
    public class SleepCommand : AsyncCommand<SleepCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [CommandOption("-s|--sleep")]
            [Description("Sleep")]
            [DefaultValue(5)]
            public int Sleep { get; set; }
        }

        private Settings _setting;
        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            _setting = settings;
            await AnsiConsole.Status()
                .AutoRefresh(true)
                .Spinner(Spinner.Known.Default)
                .StartAsync("[red]Syncer[/]", StatusStart);
            return 0;
        }

        private async Task StatusStart(StatusContext ctx)
        {
            for (int i = 0; i < _setting.Sleep; i++)
            {
                for (int j = 0; j < 60; j++)
                {
                    ctx.Status($"[bold blue]Wait: {(j+1)} second(s)[/]");
                    await Task.Delay(1000);
                }
                AnsiConsole.MarkupLine($"[grey]MINUTES:[/] {(i+1)}[grey]...[/]");
            }
        }
    }
}